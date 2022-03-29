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
                MessageBox.Show("Game path not set. Please set the path under 'File > Browse for Game'");
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

            modListBox.Items.AddRange(mods.ToArray());

            for (int i = 0; i < mods.Count; i++) {
                modListBox.SetItemChecked(i, mods[i].enabled);
            }           

            for (int i = 0; i < modListBox.Items.Count; i++) {
                mods[i].enabled = modListBox.GetItemChecked(i);
            }

            FormClosed += ModManager_FormClosed;
        }

        //Loads mods into memory based on the modlist.txt
        void LoadModList() {
            mods = new List<Mod>();

            if (!File.Exists("./modlist.txt"))
                File.Create("./modlist.txt");

            string[] modLines = File.ReadAllLines("./modlist.txt");
            List<string> modNames = new List<string>();

            foreach (var modLine in modLines) {
                string modName = modLine.Split('=')[0];
                modNames.Add(modName);
                if(Directory.Exists("./Mods/" + modName)) {
                    Mod mod = new Mod {
                        modName = modName,
                        enabled = modLine.Split('=')[1] == "true"
                    };

                    mods.Add(mod);
                }                
            }

            //Search Mods folder for new mods. If mods that are in the folder don't exist in modlist.txt, add them.
            foreach (var modPath in Directory.GetDirectories("./Mods")) {
                string modName = new DirectoryInfo(modPath).Name;
                if (!modNames.Contains(modName)) {
                    Mod mod = new Mod {
                        modName = modName,
                        enabled = false
                    };

                    mods.Add(mod);
                }
            }
        }

        void SaveModList() {
            Console.WriteLine("Saving mod list to file");
            List<string> modLines = new List<string>();
            foreach (var mod in mods) {
                string modString = mod.modName + "=" + (mod.enabled ? "true" : "false");
                modLines.Add(modString);
            }
            File.WriteAllLines("./modlist.txt", modLines.ToArray());
        }

        void InstallMods()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += BW_ApplyMods;
            bw.RunWorkerAsync();
        }

        private void BW_ApplyMods(object sender, DoWorkEventArgs e) {
            if (gamePath == null) {
                MessageBox.Show("Game path not set");
                return;
            }

            if (!File.Exists("./patch.fat.bk"))
            {
                File.Copy(gamePath + "/data_win64/patch.fat", "./patch.fat.bk", true);
                File.Copy(gamePath + "/data_win64/patch.dat", "./patch.dat.bk", true);
            }

            Console.WriteLine("Creating working folder");
            Directory.CreateDirectory("./Working/Unpack");
            
            UpdateModsEnabledState();
            SaveModList();

            Console.WriteLine("Copying mods to working folder");
            foreach (var mod in mods) {
                if (mod.enabled) {
                    Console.WriteLine("Copying " + mod.modName);
                    CopyFilesRecursively("./Mods/" + mod.modName, "./Working/Unpack");
                }
            }

            Console.WriteLine("Beginning packing");
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

            Console.WriteLine("Copying to game folder");
            File.Copy("./Working/patch.fat", gamePath + "/data_win64/patch.fat", true);
            File.Copy("./Working/patch.dat", gamePath + "/data_win64/patch.dat", true);

            Console.WriteLine("Cleaning up working directory");

            DirectoryInfo dir = new DirectoryInfo("./Working/Unpack");

            foreach (FileInfo f in dir.GetFiles())
            {
                f.Delete();
            }
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                d.Delete(true);
            }

            Console.WriteLine("Finished installing mods!");

            MessageBox.Show("Finished installing mods!");
        }

        private static void CopyFilesRecursively(string sourcePath, string targetPath) {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        private void toggleall_CheckedChanged(object sender, EventArgs e) {
            allChecked = !allChecked;
            for (int i = 0; i < modListBox.Items.Count; i++) {
                modListBox.SetItemChecked(i, allChecked);
            }
            UpdateModsEnabledState();
        }

        void UpdateModsEnabledState() {
            for (int i = 0; i < modListBox.Items.Count; i++) {
                mods[i].enabled = modListBox.GetItemChecked(i);
            }
        }

        private void btn_install_Click(object sender, EventArgs e) {
            InstallMods();
        }

        void ModListBox_ItemCheck(Object sender, ItemCheckEventArgs e)
        {
            
        }

        private void btn_moveUp_Click(object sender, EventArgs e) {           
            if (modListBox.SelectedIndex == -1) return;

            int i = modListBox.SelectedIndex;
            int j = modListBox.SelectedIndex - 1;

            if (j < 0) return;
            if (j > mods.Count - 1) return;

            Mod tmp = mods[i];
            mods[i] = mods[j];
            mods[j] = tmp;

            modListBox.Items.Clear();
            modListBox.Items.AddRange(mods.ToArray());
            modListBox.SelectedIndex = j;

            for (int o = 0; o < mods.Count; o++)
            {
                modListBox.SetItemChecked(o, mods[o].enabled);
            }
        }
        
        private void btn_moveDown_Click(object sender, EventArgs e) {
            if (modListBox.SelectedIndex == -1) return;

            int i = modListBox.SelectedIndex;
            int j = modListBox.SelectedIndex + 1;

            if (j < 0) return;
            if (j > mods.Count - 1) return;

            Mod tmp = mods[i];
            mods[i] = mods[j];
            mods[j] = tmp;

            modListBox.Items.Clear();
            modListBox.Items.AddRange(mods.ToArray());
            modListBox.SelectedIndex = j;

            for (int o = 0; o < mods.Count; o++)
            {
                modListBox.SetItemChecked(o, mods[o].enabled);
            }
        }

        private void ModManager_FormClosed(object sender, FormClosedEventArgs e) {;
            SaveModList();
        }

        private void menu_browseforgame_Click(object sender, EventArgs e)
        {
            if (gamePathBrowser.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(gamePathBrowser.SelectedPath + "/bin/Watch_Dogs.exe"))
                {
                    gamePath = gamePathBrowser.SelectedPath;
                    MessageBox.Show("Game path set to " + gamePath);
                    Console.WriteLine("Game path set to " + gamePath);

                    ini.Write("gamePath", gamePath);
                }
                else
                {
                    MessageBox.Show("Watch_Dogs.exe not found in " + gamePathBrowser.SelectedPath);
                    return;
                }
            }

        }

        private void menu_openmodsfolder_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + "/Mods/");
        }
    }
}