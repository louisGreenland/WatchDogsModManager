using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

using System.IO;
using System.Diagnostics;

namespace WatchDogsModManager
{
    public partial class ModManager : Form
    {
        string gamePath;

        List<Mod> mods;
        List<Mod> enabledMods;

        IniFile ini;

        BackgroundWorker BW_Install;

        public ModManager() {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private void ModManager_Load(object sender, EventArgs e) {
            ini = new IniFile();

            string tempPath = ini.Read("gamePath");
            if(tempPath == "") {
                MessageBox.Show("Game path not set.", "Error");
                FindGamePath();
            }
            else {
                if (File.Exists(tempPath + "/bin/Watch_Dogs.exe")) {
                    gamePath = tempPath;
                    Console.WriteLine("Game path set to " + gamePath);
                }
                else {
                    MessageBox.Show("Game path has changed.", "Error");
                    FindGamePath();
                }
            }

            LoadModList();

            FormClosed += ModManager_FormClosed;
        }

        //Loads mods based on the modlist.txt
        void LoadModList() {
            mods = new List<Mod>();
            modlist.Items.Clear();

            enabledMods = new List<Mod>();
            enabledmodslist.Items.Clear();

            if (!File.Exists("./modlist.txt"))
                File.Create("./modlist.txt");

            string[] modLines = File.ReadAllLines("./modlist.txt");

            foreach (var modName in modLines) {
                if(Directory.Exists("./Mods/" + modName) || File.Exists("./Mods/" + modName)) {
                    Mod mod = new Mod {
                        modName = modName
                    };

                    enabledMods.Add(mod);
                }
            }

            //Search Mods folder for new mods. If mods that are in the folder don't exist in modlist.txt, add them.
            string[] newMods = Directory.GetDirectories("./Mods").Concat(Directory.GetFiles("./Mods", "*.fat")).ToArray();
            foreach (var modPath in newMods) {
                string modName = new DirectoryInfo(modPath).Name;
                if (!modLines.Contains(modName)) {
                    Mod mod = new Mod {
                        modName = modName
                    };

                    mods.Add(mod);
                }
            }

            RefreshLists();

            console.AppendText("\nModlist loaded, " + modLines.Length + " mods enabled");
        }

        void SaveModList() {
            Console.WriteLine("Saving mod list to file");
            console.AppendText("\nSaving mod list to file");
            List<string> modLines = new List<string>();
            foreach (var mod in enabledMods) {
                modLines.Add(mod.modName);
            }
            File.WriteAllLines("./modlist.txt", modLines.ToArray());
        }

        void InstallMods()
        {
            if (!enabledMods.Any())
            {
                MessageBox.Show("No mods are enabled.");
                return;
            }

            BW_Install = new BackgroundWorker();
            BW_Install.DoWork += BW_InstallMods;
            BW_Install.WorkerReportsProgress = true;

            BW_Install.RunWorkerAsync();
            console.Select();
        }

        private void BW_InstallMods(object sender, DoWorkEventArgs e) {
            if (gamePath == null) {
                MessageBox.Show("Game path not set");
                console.AppendText("\nGame path not set");
                FindGamePath();
                return;
            }

            if (!File.Exists("./patch.fat.bk"))
            {
                MessageBox.Show("The Modmanager will now create a backup of the original Patch. This will be used to restore to an unmodded version of the game. If you lose it, you'll have to verify the game's files.");
                File.Copy(gamePath + "/data_win64/patch.fat", "./patch.fat.bk", true);
                File.Copy(gamePath + "/data_win64/patch.dat", "./patch.dat.bk", true);
            }

            Console.WriteLine("Creating working folder");
            console.AppendText("\nCreating working folder");
            Directory.CreateDirectory("./Working/Unpack");
            
            SaveModList();

            Console.WriteLine("Copying mods to working folder");
            console.AppendText("\nCopying mods to working folder");
            foreach (var mod in enabledMods) {
                Console.WriteLine("Copying " + mod.modName);
                console.AppendText("\nCopying " + mod.modName);
                if (File.Exists("./Mods/" + mod.modName))
                {
                    Console.WriteLine(mod.modName + " is a patch, unpacking.");
                    console.AppendText("\n" + mod.modName + " is a patch, unpacking.");

                    Process unpackProc = new Process();
                    unpackProc.StartInfo.FileName = Application.StartupPath + "/bin/Gibbed.Disrupt.Unpack.exe";
                    unpackProc.StartInfo.Arguments = "./Mods/" + mod.modName + " ./Working/Unpack";
                    unpackProc.StartInfo.CreateNoWindow = true;
                    unpackProc.StartInfo.UseShellExecute = false;
                    unpackProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    unpackProc.Start();
                    unpackProc.WaitForExit();
                }
                else
                {
                    CopyFilesRecursively("./Mods/" + mod.modName, "./Working/Unpack");
                }
            }

            Console.WriteLine("Beginning packing");
            console.AppendText("\nBeginning packing");
            Process packProc = new Process();
            packProc.StartInfo.FileName = Application.StartupPath + "/bin/Gibbed.Disrupt.Pack.exe";
            packProc.StartInfo.Arguments = "./Working/patch.fat " + "./Working/Unpack";
            packProc.StartInfo.CreateNoWindow = true;
            packProc.StartInfo.UseShellExecute = false;
            packProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            Console.WriteLine("Creating Patch");
            console.AppendText("\nCreating Patch");
            packProc.Start();
            packProc.WaitForExit();
            Console.WriteLine("Finished creating Patch");
            console.AppendText("\nFinished creating Patch");

            Console.WriteLine("Copying to game folder");
            console.AppendText("\nCopying to game folder");
            File.Copy("./Working/patch.fat", gamePath + "/data_win64/patch.fat", true);
            File.Copy("./Working/patch.dat", gamePath + "/data_win64/patch.dat", true);

            Console.WriteLine("Cleaning up working directory");
            console.AppendText("\nCleaning up working directory");

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
            console.AppendText("\nFinished installing mods!");

            MessageBox.Show("Finished installing mods!");
        }

        private void CopyFilesRecursively(string sourcePath, string targetPath) {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories)) {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                console.AppendText("\nCopying " + dirPath);
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)) {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                console.AppendText("\nCopying " + newPath);
            }
        }

        private void btn_install_Click(object sender, EventArgs e) {
            InstallMods();
        }

        private void btn_uninstallmods_Click(object sender, EventArgs e)
        {
            console.AppendText("\nUninstalling mods");

            File.Copy("./patch.fat.bk", gamePath + "/data_win64/patch.fat", true);
            File.Copy("./patch.dat.bk", gamePath + "/data_win64/patch.dat", true);
        }

        private void enableMod_Click(object sender, EventArgs e)
        {
            if (modlist.SelectedIndex == -1) return;

            enabledMods.Add(mods[modlist.SelectedIndex]);
            mods.Remove(mods[modlist.SelectedIndex]);
            RefreshLists();
        }
        private void disableMod_Click(object sender, EventArgs e)
        {
            if(enabledmodslist.SelectedIndex == -1) return;

            int i = enabledmodslist.SelectedIndex;

            mods.Add(enabledMods[i]);
            enabledMods.Remove(enabledMods[i]);
            RefreshLists();
        }

        void RefreshLists()
        {
            enabledmodslist.Items.Clear();
            enabledmodslist.Items.AddRange(enabledMods.ToArray());

            modlist.Items.Clear();
            modlist.Items.AddRange(mods.ToArray());
        }

        private void btn_moveUp_Click(object sender, EventArgs e) {           
            if (enabledmodslist.SelectedIndex == -1) return;

            int i = enabledmodslist.SelectedIndex;
            int j = enabledmodslist.SelectedIndex - 1;

            if (j < 0) return;
            if (j > mods.Count - 1) return;

            Mod tmp = enabledMods[i];
            enabledMods[i] = enabledMods[j];
            enabledMods[j] = tmp;

            RefreshLists();
            enabledmodslist.SelectedIndex = j;
        }
        
        private void btn_moveDown_Click(object sender, EventArgs e) {
            if (enabledmodslist.SelectedIndex == -1) return;

            int i = enabledmodslist.SelectedIndex;
            int j = enabledmodslist.SelectedIndex + 1;

            if (j < 0) return;
            if (j > mods.Count - 1) return;

            Mod tmp = enabledMods[i];
            enabledMods[i] = enabledMods[j];
            enabledMods[j] = tmp;

            RefreshLists();
            enabledmodslist.SelectedIndex = j;
        }

        private void ModManager_FormClosed(object sender, FormClosedEventArgs e) {
            Console.WriteLine("Cleaning up working directory");
            console.AppendText("\nCleaning up working directory");

            DirectoryInfo dir = new DirectoryInfo("./Working/Unpack");

            foreach (FileInfo f in dir.GetFiles())
            {
                f.Delete();
            }
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                d.Delete(true);
            }

            SaveModList();
        }

        private void btn_removemods_Click(object sender, EventArgs e)
        {

        }

        private void menu_browseforgame_Click(object sender, EventArgs e)
        {
            FindGamePath();
        }

        private void FindGamePath()
        {
            if (gamePathBrowser.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(gamePathBrowser.SelectedPath + "/bin/Watch_Dogs.exe"))
                {
                    gamePath = gamePathBrowser.SelectedPath;
                    MessageBox.Show("Game path set to " + gamePath, "Success!");
                    Console.WriteLine("Game path set to " + gamePath);

                    ini.Write("gamePath", gamePath);
                }
                else
                {
                    MessageBox.Show("This is not the Watch_Dogs root folder " + gamePathBrowser.SelectedPath);
                    FindGamePath();
                    return;
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadModList();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_minimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private const int cGrip = 16;
        private const int cCaption = 32;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;
                }

                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        bool flag = false;
        int x;
        int y;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            flag = true;
            x = e.X;
            y = e.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)

        {
            //Check if Flag is True ??? if so then make form position same
            //as Cursor position
            if (flag == true)
            {
                int newX = Location.X + (e.X - x);
                int newY = Location.Y + (e.Y - y);
                Location = new Point(newX, newY);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            flag = false;
        }
    }
}