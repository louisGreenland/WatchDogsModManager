using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Diagnostics;

namespace WatchDogsModManager
{
    public partial class ModManager : Form
    {
        bool allChecked;
        string gamePath;

        List<Mod> mods;

        IniFile ini;

        public ModManager() {
            InitializeComponent();
        }

        private void ModManager_Load(object sender, EventArgs e) {
            ini = new IniFile();

            string tempPath = ini.Read("gamePath");
            if(tempPath == "") {
                MessageBox.Show("Game path not set. Please set the path using the 'Browse for Game' button");
            }
            else {
                if (File.Exists(tempPath + "/bin/Watch_Dogs.exe")) {
                    gamePath = tempPath;
                    Console.WriteLine("Game path set to " + gamePath);
                }
                else {
                    MessageBox.Show("Watch_Dogs.exe not found in " + tempPath);
                }
            }

            LoadModList();
            SaveModList();

            modListBox.Items.AddRange(mods.ToArray());

            //File.Copy(gamePath + "/data_win64/patch.fat", "./patch.fat.bk", true);
            //File.Copy(gamePath + "/data_win64/patch.dat", "./patch.dat.bk", true);
        }

        void LoadModList() {
            mods = new List<Mod>();

            //Search Mods folder for new mods
            if (!File.Exists("./modlist.txt"))
                File.Create("./modlist.txt");

            string[] modNames = File.ReadAllLines("./modlist.txt");

            foreach (var modName in modNames) {
                if(Directory.Exists("./Mods/" + modName)) {
                    Mod mod = new Mod();
                    mod.modPath = "./Mods/" + modName;
                    mod.modName = new DirectoryInfo(mod.modPath).Name;
                    mod.enabled = true;
                    mods.Add(mod);
                }
            }

            foreach (var modPath in Directory.GetDirectories("./Mods")) {
                string modName = new DirectoryInfo(modPath).Name;
                if (!modNames.Contains(modName)) {
                    Mod mod = new Mod();
                    mod.modName = modName;
                    mod.modPath = modPath;
                    mod.enabled = true;
                    mods.Add(mod);
                }
            }
        }

        void SaveModList() {
            List<string> modNames = new List<string>();
            foreach (var mod in mods) {
                modNames.Add(mod.modName);
            }
            File.WriteAllLines("./modlist.txt", modNames.ToArray());
        }

        private void btn_getpath_Click(object sender, EventArgs e) {
            if (gamePathBrowser.ShowDialog() == DialogResult.OK) {
                if (File.Exists(gamePathBrowser.SelectedPath + "/bin/Watch_Dogs.exe")) {
                    gamePath = gamePathBrowser.SelectedPath;
                    MessageBox.Show("Game path set to " + gamePath);
                    Console.WriteLine("Game path set to " + gamePath);

                    ini.Write("gamePath", gamePath);
                }
                else {
                    MessageBox.Show("Watch_Dogs.exe not found in " + gamePathBrowser.SelectedPath);
                    return;
                }
            }
        }

        private void ApplyMods() {
            if (gamePath == null) {
                MessageBox.Show("Game path not set");
                return;
            }

            Directory.CreateDirectory("./Working/Unpack");

            foreach (var mod in mods) {
                if (mod.enabled) {
                    Console.WriteLine("Copying " + mod.modName);
                    CopyFilesRecursively(mod.modPath, "./Working/Unpack");
                }
            }

            Process packProc = new Process();
            packProc.StartInfo.FileName = Application.StartupPath + "/bin/Gibbed.Disrupt.Pack.exe";
            packProc.StartInfo.Arguments = "./Working/patch.fat " + "./Working/Unpack";
            packProc.StartInfo.CreateNoWindow = true;
            packProc.StartInfo.UseShellExecute = false;
            packProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Console.WriteLine("Creating Patch");
            packProc.Start();
            packProc.WaitForExit();
            Console.WriteLine("Finished creating Patch");

            //File.Copy("./Working/patch.fat", gamePath + "/data_win64", true);
            //File.Copy("./Working/patch.dat", gamePath + "/data_win64", true);
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath) {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        private void toggleall_CheckedChanged(object sender, EventArgs e) {
            allChecked = !allChecked;
            for (int i = 0; i < modListBox.Items.Count; i++) {
                modListBox.SetItemChecked(i, allChecked);
            }
        }

        private void btn_install_Click(object sender, EventArgs e) {
            ApplyMods();
        }

        private void btn_moveUp_Click(object sender, EventArgs e) {           
            int i = modListBox.SelectedIndex;
            int j = modListBox.SelectedIndex - 1;
            if (j < 0) return;

            Mod tmp = mods[i];
            mods[i] = mods[j];
            mods[j] = tmp;

            modListBox.Items.Clear();
            modListBox.Items.AddRange(mods.ToArray());
            modListBox.SelectedIndex = j;

            SaveModList();
        }
        
        private void btn_moveDown_Click(object sender, EventArgs e) {
            int i = modListBox.SelectedIndex;
            int j = modListBox.SelectedIndex + 1;
            if (j > mods.Count - 1) return;

            Mod tmp = mods[i];
            mods[i] = mods[j];
            mods[j] = tmp;

            modListBox.Items.Clear();
            modListBox.Items.AddRange(mods.ToArray());
            modListBox.SelectedIndex = j;

            SaveModList();
        }
    }
}