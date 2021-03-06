
namespace WatchDogsModManager
{
    partial class ModManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModManager));
            this.modListBox = new System.Windows.Forms.CheckedListBox();
            this.btn_install = new System.Windows.Forms.Button();
            this.gamePathBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.btn_moveDown = new System.Windows.Forms.Button();
            this.btn_moveUp = new System.Windows.Forms.Button();
            this.toggleall = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.Title = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menu_openmodsfolder = new System.Windows.Forms.MenuItem();
            this.menu_addmod = new System.Windows.Forms.MenuItem();
            this.menu_browseforgame = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // modListBox
            // 
            this.modListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modListBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.modListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.modListBox.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modListBox.ForeColor = System.Drawing.Color.White;
            this.modListBox.FormattingEnabled = true;
            this.modListBox.Location = new System.Drawing.Point(12, 37);
            this.modListBox.Name = "modListBox";
            this.modListBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.modListBox.ScrollAlwaysVisible = true;
            this.modListBox.Size = new System.Drawing.Size(469, 651);
            this.modListBox.TabIndex = 0;
            // 
            // btn_install
            // 
            this.btn_install.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_install.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btn_install.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_install.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_install.ForeColor = System.Drawing.Color.White;
            this.btn_install.Location = new System.Drawing.Point(12, 696);
            this.btn_install.Name = "btn_install";
            this.btn_install.Size = new System.Drawing.Size(198, 39);
            this.btn_install.TabIndex = 1;
            this.btn_install.Text = "Install Mods";
            this.btn_install.UseVisualStyleBackColor = false;
            this.btn_install.Click += new System.EventHandler(this.btn_install_Click);
            // 
            // gamePathBrowser
            // 
            this.gamePathBrowser.Description = "Browse for game folder containing \'Watch_Dogs.exe\'";
            this.gamePathBrowser.ShowNewFolderButton = false;
            // 
            // btn_moveDown
            // 
            this.btn_moveDown.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_moveDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btn_moveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_moveDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_moveDown.ForeColor = System.Drawing.Color.White;
            this.btn_moveDown.Location = new System.Drawing.Point(490, 369);
            this.btn_moveDown.Name = "btn_moveDown";
            this.btn_moveDown.Size = new System.Drawing.Size(142, 42);
            this.btn_moveDown.TabIndex = 2;
            this.btn_moveDown.Text = "Move Down";
            this.btn_moveDown.UseVisualStyleBackColor = false;
            this.btn_moveDown.Click += new System.EventHandler(this.btn_moveDown_Click);
            // 
            // btn_moveUp
            // 
            this.btn_moveUp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btn_moveUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(55)))));
            this.btn_moveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_moveUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_moveUp.ForeColor = System.Drawing.Color.White;
            this.btn_moveUp.Location = new System.Drawing.Point(490, 321);
            this.btn_moveUp.Name = "btn_moveUp";
            this.btn_moveUp.Size = new System.Drawing.Size(142, 42);
            this.btn_moveUp.TabIndex = 3;
            this.btn_moveUp.Text = "Move Up";
            this.btn_moveUp.UseVisualStyleBackColor = false;
            this.btn_moveUp.Click += new System.EventHandler(this.btn_moveUp_Click);
            // 
            // toggleall
            // 
            this.toggleall.AutoSize = true;
            this.toggleall.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleall.ForeColor = System.Drawing.Color.White;
            this.toggleall.Location = new System.Drawing.Point(12, 7);
            this.toggleall.Name = "toggleall";
            this.toggleall.Size = new System.Drawing.Size(97, 24);
            this.toggleall.TabIndex = 4;
            this.toggleall.Text = "Toggle All";
            this.toggleall.UseVisualStyleBackColor = true;
            this.toggleall.CheckedChanged += new System.EventHandler(this.toggleall_CheckedChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(216, 696);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(547, 39);
            this.progressBar1.TabIndex = 5;
            // 
            // Title
            // 
            this.Title.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.Color.White;
            this.Title.Location = new System.Drawing.Point(486, 37);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(227, 22);
            this.Title.TabIndex = 6;
            this.Title.Text = "Watch_Dogs Mod Manager";
            this.Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(487, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(276, 91);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menu_openmodsfolder,
            this.menu_addmod,
            this.menu_browseforgame});
            this.menuItem1.Text = "File";
            // 
            // menu_openmodsfolder
            // 
            this.menu_openmodsfolder.Index = 0;
            this.menu_openmodsfolder.Text = "Open Mods Folder";
            this.menu_openmodsfolder.Click += new System.EventHandler(this.menu_openmodsfolder_Click);
            // 
            // menu_addmod
            // 
            this.menu_addmod.Index = 1;
            this.menu_addmod.Text = "Add Mod";
            this.menu_addmod.Visible = false;
            // 
            // menu_browseforgame
            // 
            this.menu_browseforgame.Index = 2;
            this.menu_browseforgame.Text = "Browse for Game";
            this.menu_browseforgame.Click += new System.EventHandler(this.menu_browseforgame_Click);
            // 
            // ModManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(770, 741);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.toggleall);
            this.Controls.Add(this.btn_moveUp);
            this.Controls.Add(this.btn_moveDown);
            this.Controls.Add(this.btn_install);
            this.Controls.Add(this.modListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Menu = this.mainMenu1;
            this.Name = "ModManager";
            this.Text = "Watch Dogs Mod Manager";
            this.Load += new System.EventHandler(this.ModManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox modListBox;
        private System.Windows.Forms.Button btn_install;
        private System.Windows.Forms.FolderBrowserDialog gamePathBrowser;
        private System.Windows.Forms.Button btn_moveDown;
        private System.Windows.Forms.Button btn_moveUp;
        private System.Windows.Forms.CheckBox toggleall;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menu_addmod;
        private System.Windows.Forms.MenuItem menu_browseforgame;
        private System.Windows.Forms.MenuItem menu_openmodsfolder;
    }
}