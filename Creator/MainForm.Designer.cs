namespace Creator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.detailsTab = new System.Windows.Forms.TabPage();
            this.buildBtn = new System.Windows.Forms.Button();
            this.saveBtn = new System.Windows.Forms.Button();
            this.loadBtn = new System.Windows.Forms.Button();
            this.irGroup = new System.Windows.Forms.GroupBox();
            this.irIconLbl = new System.Windows.Forms.Label();
            this.irIconBox = new System.Windows.Forms.PictureBox();
            this.irNameBox = new System.Windows.Forms.TextBox();
            this.irNameLbl = new System.Windows.Forms.Label();
            this.itGroup = new System.Windows.Forms.GroupBox();
            this.itGenerateBtn = new System.Windows.Forms.Button();
            this.itGuidBox = new System.Windows.Forms.TextBox();
            this.itGuidLbl = new System.Windows.Forms.Label();
            this.itExecPathBox = new System.Windows.Forms.TextBox();
            this.itExecPathLbl = new System.Windows.Forms.Label();
            this.itDefaultDirBox = new System.Windows.Forms.TextBox();
            this.itDefaultDirLbl = new System.Windows.Forms.Label();
            this.gGroup = new System.Windows.Forms.GroupBox();
            this.gLinkBox = new System.Windows.Forms.TextBox();
            this.gLinksLbl = new System.Windows.Forms.Label();
            this.gVersionBox = new System.Windows.Forms.TextBox();
            this.gVersionLbl = new System.Windows.Forms.Label();
            this.gCompanyNameBox = new System.Windows.Forms.TextBox();
            this.gProgramNameBox = new System.Windows.Forms.TextBox();
            this.gCompanyNameLbl = new System.Windows.Forms.Label();
            this.gProgramNameLbL = new System.Windows.Forms.Label();
            this.filesTab = new System.Windows.Forms.TabPage();
            this.fAddFilesBtn = new System.Windows.Forms.Button();
            this.fAddFoldersBtn = new System.Windows.Forms.Button();
            this.fFilesList = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pathColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.typeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.associationTab = new System.Windows.Forms.TabPage();
            this.openIconDialog = new System.Windows.Forms.OpenFileDialog();
            this.openInstallerDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveInstallerDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveBuildDialog = new System.Windows.Forms.SaveFileDialog();
            this.fFileListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainTabs.SuspendLayout();
            this.detailsTab.SuspendLayout();
            this.irGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.irIconBox)).BeginInit();
            this.itGroup.SuspendLayout();
            this.gGroup.SuspendLayout();
            this.filesTab.SuspendLayout();
            this.fFileListContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.detailsTab);
            this.mainTabs.Controls.Add(this.filesTab);
            this.mainTabs.Controls.Add(this.associationTab);
            this.mainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabs.Location = new System.Drawing.Point(0, 0);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(521, 474);
            this.mainTabs.TabIndex = 0;
            // 
            // detailsTab
            // 
            this.detailsTab.Controls.Add(this.buildBtn);
            this.detailsTab.Controls.Add(this.saveBtn);
            this.detailsTab.Controls.Add(this.loadBtn);
            this.detailsTab.Controls.Add(this.irGroup);
            this.detailsTab.Controls.Add(this.itGroup);
            this.detailsTab.Controls.Add(this.gGroup);
            this.detailsTab.Location = new System.Drawing.Point(4, 22);
            this.detailsTab.Name = "detailsTab";
            this.detailsTab.Padding = new System.Windows.Forms.Padding(3);
            this.detailsTab.Size = new System.Drawing.Size(513, 448);
            this.detailsTab.TabIndex = 0;
            this.detailsTab.Text = "Details";
            this.detailsTab.UseVisualStyleBackColor = true;
            // 
            // buildBtn
            // 
            this.buildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buildBtn.AutoSize = true;
            this.buildBtn.Location = new System.Drawing.Point(220, 409);
            this.buildBtn.Name = "buildBtn";
            this.buildBtn.Size = new System.Drawing.Size(287, 33);
            this.buildBtn.TabIndex = 5;
            this.buildBtn.Text = "Build";
            this.buildBtn.UseVisualStyleBackColor = true;
            this.buildBtn.Click += new System.EventHandler(this.buildBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveBtn.Location = new System.Drawing.Point(114, 409);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(100, 33);
            this.saveBtn.TabIndex = 4;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // loadBtn
            // 
            this.loadBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.loadBtn.Location = new System.Drawing.Point(8, 409);
            this.loadBtn.Name = "loadBtn";
            this.loadBtn.Size = new System.Drawing.Size(100, 33);
            this.loadBtn.TabIndex = 3;
            this.loadBtn.Text = "Load";
            this.loadBtn.UseVisualStyleBackColor = true;
            this.loadBtn.Click += new System.EventHandler(this.loadBtn_Click);
            // 
            // irGroup
            // 
            this.irGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.irGroup.Controls.Add(this.irIconLbl);
            this.irGroup.Controls.Add(this.irIconBox);
            this.irGroup.Controls.Add(this.irNameBox);
            this.irGroup.Controls.Add(this.irNameLbl);
            this.irGroup.Location = new System.Drawing.Point(8, 281);
            this.irGroup.Name = "irGroup";
            this.irGroup.Size = new System.Drawing.Size(499, 122);
            this.irGroup.TabIndex = 2;
            this.irGroup.TabStop = false;
            this.irGroup.Text = "Installer";
            // 
            // irIconLbl
            // 
            this.irIconLbl.AutoSize = true;
            this.irIconLbl.Location = new System.Drawing.Point(16, 50);
            this.irIconLbl.Name = "irIconLbl";
            this.irIconLbl.Size = new System.Drawing.Size(28, 13);
            this.irIconLbl.TabIndex = 18;
            this.irIconLbl.Text = "Icon";
            // 
            // irIconBox
            // 
            this.irIconBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.irIconBox.Location = new System.Drawing.Point(113, 50);
            this.irIconBox.Name = "irIconBox";
            this.irIconBox.Size = new System.Drawing.Size(58, 58);
            this.irIconBox.TabIndex = 17;
            this.irIconBox.TabStop = false;
            this.irIconBox.Click += new System.EventHandler(this.irIconBox_Click);
            // 
            // irNameBox
            // 
            this.irNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.irNameBox.Location = new System.Drawing.Point(113, 24);
            this.irNameBox.Name = "irNameBox";
            this.irNameBox.Size = new System.Drawing.Size(366, 20);
            this.irNameBox.TabIndex = 16;
            this.irNameBox.Text = "MyProgram Installer";
            // 
            // irNameLbl
            // 
            this.irNameLbl.AutoSize = true;
            this.irNameLbl.Location = new System.Drawing.Point(16, 27);
            this.irNameLbl.Name = "irNameLbl";
            this.irNameLbl.Size = new System.Drawing.Size(74, 13);
            this.irNameLbl.TabIndex = 15;
            this.irNameLbl.Text = "Installer Name";
            // 
            // itGroup
            // 
            this.itGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itGroup.Controls.Add(this.itGenerateBtn);
            this.itGroup.Controls.Add(this.itGuidBox);
            this.itGroup.Controls.Add(this.itGuidLbl);
            this.itGroup.Controls.Add(this.itExecPathBox);
            this.itGroup.Controls.Add(this.itExecPathLbl);
            this.itGroup.Controls.Add(this.itDefaultDirBox);
            this.itGroup.Controls.Add(this.itDefaultDirLbl);
            this.itGroup.Location = new System.Drawing.Point(8, 158);
            this.itGroup.Name = "itGroup";
            this.itGroup.Size = new System.Drawing.Size(499, 117);
            this.itGroup.TabIndex = 1;
            this.itGroup.TabStop = false;
            this.itGroup.Text = "Installation";
            // 
            // itGenerateBtn
            // 
            this.itGenerateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itGenerateBtn.Location = new System.Drawing.Point(404, 75);
            this.itGenerateBtn.Name = "itGenerateBtn";
            this.itGenerateBtn.Size = new System.Drawing.Size(75, 23);
            this.itGenerateBtn.TabIndex = 14;
            this.itGenerateBtn.Text = "Generate";
            this.itGenerateBtn.UseVisualStyleBackColor = true;
            this.itGenerateBtn.Click += new System.EventHandler(this.itGenerateBtn_Click);
            // 
            // itGuidBox
            // 
            this.itGuidBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itGuidBox.Location = new System.Drawing.Point(113, 77);
            this.itGuidBox.Name = "itGuidBox";
            this.itGuidBox.Size = new System.Drawing.Size(285, 20);
            this.itGuidBox.TabIndex = 13;
            this.itGuidBox.Text = "{5c023135-e620-402b-b2ae-17c670e5f843}";
            // 
            // itGuidLbl
            // 
            this.itGuidLbl.AutoSize = true;
            this.itGuidLbl.Location = new System.Drawing.Point(16, 80);
            this.itGuidLbl.Name = "itGuidLbl";
            this.itGuidLbl.Size = new System.Drawing.Size(34, 13);
            this.itGuidLbl.TabIndex = 12;
            this.itGuidLbl.Text = "GUID";
            // 
            // itExecPathBox
            // 
            this.itExecPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itExecPathBox.Location = new System.Drawing.Point(113, 51);
            this.itExecPathBox.Name = "itExecPathBox";
            this.itExecPathBox.Size = new System.Drawing.Size(366, 20);
            this.itExecPathBox.TabIndex = 11;
            this.itExecPathBox.Text = "MyProgram.exe";
            // 
            // itExecPathLbl
            // 
            this.itExecPathLbl.AutoSize = true;
            this.itExecPathLbl.Location = new System.Drawing.Point(16, 54);
            this.itExecPathLbl.Name = "itExecPathLbl";
            this.itExecPathLbl.Size = new System.Drawing.Size(85, 13);
            this.itExecPathLbl.TabIndex = 10;
            this.itExecPathLbl.Text = "Executable Path";
            // 
            // itDefaultDirBox
            // 
            this.itDefaultDirBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.itDefaultDirBox.Location = new System.Drawing.Point(113, 25);
            this.itDefaultDirBox.Name = "itDefaultDirBox";
            this.itDefaultDirBox.Size = new System.Drawing.Size(366, 20);
            this.itDefaultDirBox.TabIndex = 9;
            this.itDefaultDirBox.Text = "%LocalAppData%/MyProgram";
            // 
            // itDefaultDirLbl
            // 
            this.itDefaultDirLbl.AutoSize = true;
            this.itDefaultDirLbl.Location = new System.Drawing.Point(16, 28);
            this.itDefaultDirLbl.Name = "itDefaultDirLbl";
            this.itDefaultDirLbl.Size = new System.Drawing.Size(86, 13);
            this.itDefaultDirLbl.TabIndex = 8;
            this.itDefaultDirLbl.Text = "Default Directory";
            // 
            // gGroup
            // 
            this.gGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gGroup.Controls.Add(this.gLinkBox);
            this.gGroup.Controls.Add(this.gLinksLbl);
            this.gGroup.Controls.Add(this.gVersionBox);
            this.gGroup.Controls.Add(this.gVersionLbl);
            this.gGroup.Controls.Add(this.gCompanyNameBox);
            this.gGroup.Controls.Add(this.gProgramNameBox);
            this.gGroup.Controls.Add(this.gCompanyNameLbl);
            this.gGroup.Controls.Add(this.gProgramNameLbL);
            this.gGroup.Location = new System.Drawing.Point(8, 6);
            this.gGroup.Name = "gGroup";
            this.gGroup.Size = new System.Drawing.Size(499, 146);
            this.gGroup.TabIndex = 0;
            this.gGroup.TabStop = false;
            this.gGroup.Text = "General";
            // 
            // gLinkBox
            // 
            this.gLinkBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gLinkBox.Location = new System.Drawing.Point(113, 103);
            this.gLinkBox.Name = "gLinkBox";
            this.gLinkBox.Size = new System.Drawing.Size(366, 20);
            this.gLinkBox.TabIndex = 7;
            this.gLinkBox.Text = "https://example.com/";
            // 
            // gLinksLbl
            // 
            this.gLinksLbl.AutoSize = true;
            this.gLinksLbl.Location = new System.Drawing.Point(16, 106);
            this.gLinksLbl.Name = "gLinksLbl";
            this.gLinksLbl.Size = new System.Drawing.Size(27, 13);
            this.gLinksLbl.TabIndex = 6;
            this.gLinksLbl.Text = "Link";
            // 
            // gVersionBox
            // 
            this.gVersionBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gVersionBox.Location = new System.Drawing.Point(113, 77);
            this.gVersionBox.Name = "gVersionBox";
            this.gVersionBox.Size = new System.Drawing.Size(366, 20);
            this.gVersionBox.TabIndex = 5;
            this.gVersionBox.Text = "0.0.1";
            // 
            // gVersionLbl
            // 
            this.gVersionLbl.AutoSize = true;
            this.gVersionLbl.Location = new System.Drawing.Point(16, 80);
            this.gVersionLbl.Name = "gVersionLbl";
            this.gVersionLbl.Size = new System.Drawing.Size(42, 13);
            this.gVersionLbl.TabIndex = 4;
            this.gVersionLbl.Text = "Version";
            // 
            // gCompanyNameBox
            // 
            this.gCompanyNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gCompanyNameBox.Location = new System.Drawing.Point(113, 51);
            this.gCompanyNameBox.Name = "gCompanyNameBox";
            this.gCompanyNameBox.Size = new System.Drawing.Size(366, 20);
            this.gCompanyNameBox.TabIndex = 3;
            this.gCompanyNameBox.Text = "MyCompany";
            // 
            // gProgramNameBox
            // 
            this.gProgramNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gProgramNameBox.Location = new System.Drawing.Point(113, 26);
            this.gProgramNameBox.Name = "gProgramNameBox";
            this.gProgramNameBox.Size = new System.Drawing.Size(366, 20);
            this.gProgramNameBox.TabIndex = 2;
            this.gProgramNameBox.Text = "MyProgram";
            // 
            // gCompanyNameLbl
            // 
            this.gCompanyNameLbl.AutoSize = true;
            this.gCompanyNameLbl.Location = new System.Drawing.Point(16, 54);
            this.gCompanyNameLbl.Name = "gCompanyNameLbl";
            this.gCompanyNameLbl.Size = new System.Drawing.Size(82, 13);
            this.gCompanyNameLbl.TabIndex = 1;
            this.gCompanyNameLbl.Text = "Company Name";
            // 
            // gProgramNameLbL
            // 
            this.gProgramNameLbL.AutoSize = true;
            this.gProgramNameLbL.Location = new System.Drawing.Point(16, 29);
            this.gProgramNameLbL.Name = "gProgramNameLbL";
            this.gProgramNameLbL.Size = new System.Drawing.Size(77, 13);
            this.gProgramNameLbL.TabIndex = 0;
            this.gProgramNameLbL.Text = "Program Name";
            // 
            // filesTab
            // 
            this.filesTab.Controls.Add(this.fAddFilesBtn);
            this.filesTab.Controls.Add(this.fAddFoldersBtn);
            this.filesTab.Controls.Add(this.fFilesList);
            this.filesTab.Location = new System.Drawing.Point(4, 22);
            this.filesTab.Name = "filesTab";
            this.filesTab.Padding = new System.Windows.Forms.Padding(3);
            this.filesTab.Size = new System.Drawing.Size(513, 448);
            this.filesTab.TabIndex = 1;
            this.filesTab.Text = "Files";
            this.filesTab.UseVisualStyleBackColor = true;
            // 
            // fAddFilesBtn
            // 
            this.fAddFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fAddFilesBtn.Location = new System.Drawing.Point(349, 420);
            this.fAddFilesBtn.Name = "fAddFilesBtn";
            this.fAddFilesBtn.Size = new System.Drawing.Size(75, 23);
            this.fAddFilesBtn.TabIndex = 2;
            this.fAddFilesBtn.Text = "Add Files";
            this.fAddFilesBtn.UseVisualStyleBackColor = true;
            this.fAddFilesBtn.Click += new System.EventHandler(this.fAddFilesBtn_Click);
            // 
            // fAddFoldersBtn
            // 
            this.fAddFoldersBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fAddFoldersBtn.Location = new System.Drawing.Point(430, 420);
            this.fAddFoldersBtn.Name = "fAddFoldersBtn";
            this.fAddFoldersBtn.Size = new System.Drawing.Size(75, 23);
            this.fAddFoldersBtn.TabIndex = 1;
            this.fAddFoldersBtn.Text = "Add Folders";
            this.fAddFoldersBtn.UseVisualStyleBackColor = true;
            this.fAddFoldersBtn.Click += new System.EventHandler(this.fAddFoldersBtn_Click);
            // 
            // fFilesList
            // 
            this.fFilesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.pathColumn,
            this.typeColumn});
            this.fFilesList.FullRowSelect = true;
            this.fFilesList.GridLines = true;
            this.fFilesList.HideSelection = false;
            this.fFilesList.Location = new System.Drawing.Point(6, 6);
            this.fFilesList.Name = "fFilesList";
            this.fFilesList.Size = new System.Drawing.Size(499, 410);
            this.fFilesList.TabIndex = 0;
            this.fFilesList.UseCompatibleStateImageBehavior = false;
            this.fFilesList.View = System.Windows.Forms.View.Details;
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 108;
            // 
            // pathColumn
            // 
            this.pathColumn.Text = "Path";
            this.pathColumn.Width = 287;
            // 
            // typeColumn
            // 
            this.typeColumn.Text = "Type";
            // 
            // associationTab
            // 
            this.associationTab.Location = new System.Drawing.Point(4, 22);
            this.associationTab.Name = "associationTab";
            this.associationTab.Size = new System.Drawing.Size(513, 448);
            this.associationTab.TabIndex = 2;
            this.associationTab.Text = "Association";
            this.associationTab.UseVisualStyleBackColor = true;
            // 
            // openIconDialog
            // 
            this.openIconDialog.FileName = "Icon.ico";
            this.openIconDialog.Filter = "Icon files (*.ico)|*.ico|All files (*.*)|*.*";
            this.openIconDialog.RestoreDirectory = true;
            this.openIconDialog.Title = "Choose an icon for the installer";
            // 
            // openInstallerDialog
            // 
            this.openInstallerDialog.FileName = "InstallerFile.irp";
            this.openInstallerDialog.Filter = "Installer Repack Project (*.irp)|*.irp|All files (*.*)|*.*";
            this.openInstallerDialog.RestoreDirectory = true;
            this.openInstallerDialog.Title = "Open a project file!";
            // 
            // saveInstallerDialog
            // 
            this.saveInstallerDialog.FileName = "MyProgramInstaller.irp";
            this.saveInstallerDialog.Filter = "Installer Repack Project (*.irp)|*.irp|All files (*.*)|*.*";
            this.saveInstallerDialog.RestoreDirectory = true;
            this.saveInstallerDialog.Title = "Save your project file!";
            // 
            // saveBuildDialog
            // 
            this.saveBuildDialog.FileName = "MyProgramInstaller.exe";
            this.saveBuildDialog.Filter = "Executable file (*.exe)|*.exe|All files (*.*)|*.*";
            this.saveBuildDialog.RestoreDirectory = true;
            this.saveBuildDialog.Title = "Save your installer file!";
            // 
            // fFileListContextMenu
            // 
            this.fFileListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem});
            this.fFileListContextMenu.Name = "fFileListContextMenu";
            this.fFileListContextMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 474);
            this.Controls.Add(this.mainTabs);
            this.MinimumSize = new System.Drawing.Size(482, 474);
            this.Name = "MainForm";
            this.Text = "Installer Repacker";
            this.mainTabs.ResumeLayout(false);
            this.detailsTab.ResumeLayout(false);
            this.detailsTab.PerformLayout();
            this.irGroup.ResumeLayout(false);
            this.irGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.irIconBox)).EndInit();
            this.itGroup.ResumeLayout(false);
            this.itGroup.PerformLayout();
            this.gGroup.ResumeLayout(false);
            this.gGroup.PerformLayout();
            this.filesTab.ResumeLayout(false);
            this.fFileListContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTabs;
        private System.Windows.Forms.TabPage detailsTab;
        private System.Windows.Forms.TabPage filesTab;
        private System.Windows.Forms.TabPage associationTab;
        private System.Windows.Forms.GroupBox gGroup;
        private System.Windows.Forms.TextBox gCompanyNameBox;
        private System.Windows.Forms.TextBox gProgramNameBox;
        private System.Windows.Forms.Label gCompanyNameLbl;
        private System.Windows.Forms.Label gProgramNameLbL;
        private System.Windows.Forms.TextBox gVersionBox;
        private System.Windows.Forms.Label gVersionLbl;
        private System.Windows.Forms.TextBox gLinkBox;
        private System.Windows.Forms.Label gLinksLbl;
        private System.Windows.Forms.GroupBox itGroup;
        private System.Windows.Forms.TextBox itExecPathBox;
        private System.Windows.Forms.Label itExecPathLbl;
        private System.Windows.Forms.TextBox itDefaultDirBox;
        private System.Windows.Forms.Label itDefaultDirLbl;
        private System.Windows.Forms.TextBox itGuidBox;
        private System.Windows.Forms.Label itGuidLbl;
        private System.Windows.Forms.Button itGenerateBtn;
        private System.Windows.Forms.GroupBox irGroup;
        private System.Windows.Forms.TextBox irNameBox;
        private System.Windows.Forms.Label irNameLbl;
        private System.Windows.Forms.PictureBox irIconBox;
        private System.Windows.Forms.Label irIconLbl;
        private System.Windows.Forms.Button buildBtn;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button loadBtn;
        private System.Windows.Forms.OpenFileDialog openIconDialog;
        private System.Windows.Forms.OpenFileDialog openInstallerDialog;
        private System.Windows.Forms.SaveFileDialog saveInstallerDialog;
        private System.Windows.Forms.SaveFileDialog saveBuildDialog;
        private System.Windows.Forms.ListView fFilesList;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader pathColumn;
        private System.Windows.Forms.Button fAddFoldersBtn;
        private System.Windows.Forms.Button fAddFilesBtn;
        private System.Windows.Forms.ColumnHeader typeColumn;
        private System.Windows.Forms.ContextMenuStrip fFileListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
    }
}

