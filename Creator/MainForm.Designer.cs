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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            this.fFileListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.associationTab = new System.Windows.Forms.TabPage();
            this.fileTypeDescExt = new System.Windows.Forms.Label();
            this.fileTypeExtLbl = new System.Windows.Forms.Label();
            this.fileTypeDescBox = new System.Windows.Forms.TextBox();
            this.fileTypeExtBox = new System.Windows.Forms.TextBox();
            this.urlAssocTextBox = new System.Windows.Forms.TextBox();
            this.urlProtocolAddBtn = new System.Windows.Forms.Button();
            this.fileTypeAddBtn = new System.Windows.Forms.Button();
            this.urlProtocolLbl = new System.Windows.Forms.Label();
            this.fileTypeLbl = new System.Windows.Forms.Label();
            this.fileTypesList = new System.Windows.Forms.ListView();
            this.extensionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.descriptionColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fileTypeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.urlProtocolList = new System.Windows.Forms.ListBox();
            this.urlProtocolContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.openIconDialog = new System.Windows.Forms.OpenFileDialog();
            this.openInstallerDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveInstallerDialog = new System.Windows.Forms.SaveFileDialog();
            this.saveBuildDialog = new System.Windows.Forms.SaveFileDialog();
            this.msBuildLabel = new System.Windows.Forms.Label();
            this.msBuildBox = new System.Windows.Forms.TextBox();
            this.mainTabs.SuspendLayout();
            this.detailsTab.SuspendLayout();
            this.irGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.irIconBox)).BeginInit();
            this.itGroup.SuspendLayout();
            this.gGroup.SuspendLayout();
            this.filesTab.SuspendLayout();
            this.fFileListContextMenu.SuspendLayout();
            this.associationTab.SuspendLayout();
            this.fileTypeContextMenu.SuspendLayout();
            this.urlProtocolContextMenu.SuspendLayout();
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
            this.mainTabs.Size = new System.Drawing.Size(565, 507);
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
            this.detailsTab.Size = new System.Drawing.Size(557, 481);
            this.detailsTab.TabIndex = 0;
            this.detailsTab.Text = "Details";
            this.detailsTab.UseVisualStyleBackColor = true;
            // 
            // buildBtn
            // 
            this.buildBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buildBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buildBtn.Location = new System.Drawing.Point(220, 442);
            this.buildBtn.Name = "buildBtn";
            this.buildBtn.Size = new System.Drawing.Size(331, 33);
            this.buildBtn.TabIndex = 5;
            this.buildBtn.Text = "Build";
            this.buildBtn.UseVisualStyleBackColor = true;
            this.buildBtn.Click += new System.EventHandler(this.buildBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveBtn.Location = new System.Drawing.Point(114, 442);
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
            this.loadBtn.Location = new System.Drawing.Point(8, 442);
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
            this.irGroup.Controls.Add(this.msBuildBox);
            this.irGroup.Controls.Add(this.msBuildLabel);
            this.irGroup.Controls.Add(this.irIconLbl);
            this.irGroup.Controls.Add(this.irIconBox);
            this.irGroup.Controls.Add(this.irNameBox);
            this.irGroup.Controls.Add(this.irNameLbl);
            this.irGroup.Location = new System.Drawing.Point(8, 281);
            this.irGroup.Name = "irGroup";
            this.irGroup.Size = new System.Drawing.Size(543, 155);
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
            this.irIconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
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
            this.irNameBox.Size = new System.Drawing.Size(410, 20);
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
            this.itGroup.Size = new System.Drawing.Size(543, 117);
            this.itGroup.TabIndex = 1;
            this.itGroup.TabStop = false;
            this.itGroup.Text = "Installation";
            // 
            // itGenerateBtn
            // 
            this.itGenerateBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.itGenerateBtn.Location = new System.Drawing.Point(448, 75);
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
            this.itGuidBox.Size = new System.Drawing.Size(329, 20);
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
            this.itExecPathBox.Size = new System.Drawing.Size(410, 20);
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
            this.itDefaultDirBox.Size = new System.Drawing.Size(410, 20);
            this.itDefaultDirBox.TabIndex = 9;
            this.itDefaultDirBox.Text = "%LocalAppData%\\MyProgram";
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
            this.gGroup.Size = new System.Drawing.Size(543, 146);
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
            this.gLinkBox.Size = new System.Drawing.Size(410, 20);
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
            this.gVersionBox.Size = new System.Drawing.Size(410, 20);
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
            this.gCompanyNameBox.Size = new System.Drawing.Size(410, 20);
            this.gCompanyNameBox.TabIndex = 3;
            this.gCompanyNameBox.Text = "MyCompany";
            // 
            // gProgramNameBox
            // 
            this.gProgramNameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gProgramNameBox.Location = new System.Drawing.Point(113, 26);
            this.gProgramNameBox.Name = "gProgramNameBox";
            this.gProgramNameBox.Size = new System.Drawing.Size(410, 20);
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
            this.filesTab.Size = new System.Drawing.Size(557, 448);
            this.filesTab.TabIndex = 1;
            this.filesTab.Text = "Files";
            this.filesTab.UseVisualStyleBackColor = true;
            // 
            // fAddFilesBtn
            // 
            this.fAddFilesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fAddFilesBtn.Location = new System.Drawing.Point(393, 420);
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
            this.fAddFoldersBtn.Location = new System.Drawing.Point(474, 420);
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
            this.fFilesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fFilesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.pathColumn,
            this.typeColumn});
            this.fFilesList.ContextMenuStrip = this.fFileListContextMenu;
            this.fFilesList.FullRowSelect = true;
            this.fFilesList.GridLines = true;
            this.fFilesList.HideSelection = false;
            this.fFilesList.Location = new System.Drawing.Point(6, 6);
            this.fFilesList.Name = "fFilesList";
            this.fFilesList.Size = new System.Drawing.Size(543, 410);
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
            // associationTab
            // 
            this.associationTab.Controls.Add(this.fileTypeDescExt);
            this.associationTab.Controls.Add(this.fileTypeExtLbl);
            this.associationTab.Controls.Add(this.fileTypeDescBox);
            this.associationTab.Controls.Add(this.fileTypeExtBox);
            this.associationTab.Controls.Add(this.urlAssocTextBox);
            this.associationTab.Controls.Add(this.urlProtocolAddBtn);
            this.associationTab.Controls.Add(this.fileTypeAddBtn);
            this.associationTab.Controls.Add(this.urlProtocolLbl);
            this.associationTab.Controls.Add(this.fileTypeLbl);
            this.associationTab.Controls.Add(this.fileTypesList);
            this.associationTab.Controls.Add(this.urlProtocolList);
            this.associationTab.Location = new System.Drawing.Point(4, 22);
            this.associationTab.Name = "associationTab";
            this.associationTab.Size = new System.Drawing.Size(557, 448);
            this.associationTab.TabIndex = 2;
            this.associationTab.Text = "Association";
            this.associationTab.UseVisualStyleBackColor = true;
            // 
            // fileTypeDescExt
            // 
            this.fileTypeDescExt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fileTypeDescExt.AutoSize = true;
            this.fileTypeDescExt.Location = new System.Drawing.Point(8, 392);
            this.fileTypeDescExt.Name = "fileTypeDescExt";
            this.fileTypeDescExt.Size = new System.Drawing.Size(60, 13);
            this.fileTypeDescExt.TabIndex = 10;
            this.fileTypeDescExt.Text = "Description";
            // 
            // fileTypeExtLbl
            // 
            this.fileTypeExtLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fileTypeExtLbl.AutoSize = true;
            this.fileTypeExtLbl.Location = new System.Drawing.Point(8, 366);
            this.fileTypeExtLbl.Name = "fileTypeExtLbl";
            this.fileTypeExtLbl.Size = new System.Drawing.Size(53, 13);
            this.fileTypeExtLbl.TabIndex = 9;
            this.fileTypeExtLbl.Text = "Extension";
            // 
            // fileTypeDescBox
            // 
            this.fileTypeDescBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeDescBox.Location = new System.Drawing.Point(79, 389);
            this.fileTypeDescBox.Name = "fileTypeDescBox";
            this.fileTypeDescBox.Size = new System.Drawing.Size(230, 20);
            this.fileTypeDescBox.TabIndex = 8;
            this.fileTypeDescBox.Text = "My Program File";
            // 
            // fileTypeExtBox
            // 
            this.fileTypeExtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeExtBox.Location = new System.Drawing.Point(79, 363);
            this.fileTypeExtBox.Name = "fileTypeExtBox";
            this.fileTypeExtBox.Size = new System.Drawing.Size(230, 20);
            this.fileTypeExtBox.TabIndex = 7;
            this.fileTypeExtBox.Text = ".myprogram";
            // 
            // urlAssocTextBox
            // 
            this.urlAssocTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.urlAssocTextBox.Location = new System.Drawing.Point(315, 418);
            this.urlAssocTextBox.Name = "urlAssocTextBox";
            this.urlAssocTextBox.Size = new System.Drawing.Size(153, 20);
            this.urlAssocTextBox.TabIndex = 6;
            this.urlAssocTextBox.Text = "myprogram";
            // 
            // urlProtocolAddBtn
            // 
            this.urlProtocolAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.urlProtocolAddBtn.Location = new System.Drawing.Point(474, 416);
            this.urlProtocolAddBtn.Name = "urlProtocolAddBtn";
            this.urlProtocolAddBtn.Size = new System.Drawing.Size(75, 23);
            this.urlProtocolAddBtn.TabIndex = 5;
            this.urlProtocolAddBtn.Text = "Add";
            this.urlProtocolAddBtn.UseVisualStyleBackColor = true;
            this.urlProtocolAddBtn.Click += new System.EventHandler(this.urlProtocolAddBtn_Click);
            // 
            // fileTypeAddBtn
            // 
            this.fileTypeAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeAddBtn.Location = new System.Drawing.Point(234, 415);
            this.fileTypeAddBtn.Name = "fileTypeAddBtn";
            this.fileTypeAddBtn.Size = new System.Drawing.Size(75, 23);
            this.fileTypeAddBtn.TabIndex = 4;
            this.fileTypeAddBtn.Text = "Add";
            this.fileTypeAddBtn.UseVisualStyleBackColor = true;
            this.fileTypeAddBtn.Click += new System.EventHandler(this.fileTypeAddBtn_Click);
            // 
            // urlProtocolLbl
            // 
            this.urlProtocolLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.urlProtocolLbl.AutoSize = true;
            this.urlProtocolLbl.Location = new System.Drawing.Point(312, 8);
            this.urlProtocolLbl.Name = "urlProtocolLbl";
            this.urlProtocolLbl.Size = new System.Drawing.Size(70, 13);
            this.urlProtocolLbl.TabIndex = 3;
            this.urlProtocolLbl.Text = "Url Protocols:";
            // 
            // fileTypeLbl
            // 
            this.fileTypeLbl.AutoSize = true;
            this.fileTypeLbl.Location = new System.Drawing.Point(8, 8);
            this.fileTypeLbl.Name = "fileTypeLbl";
            this.fileTypeLbl.Size = new System.Drawing.Size(58, 13);
            this.fileTypeLbl.TabIndex = 2;
            this.fileTypeLbl.Text = "File Types:";
            // 
            // fileTypesList
            // 
            this.fileTypesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypesList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fileTypesList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.extensionColumn,
            this.descriptionColumn});
            this.fileTypesList.ContextMenuStrip = this.fileTypeContextMenu;
            this.fileTypesList.FullRowSelect = true;
            this.fileTypesList.GridLines = true;
            this.fileTypesList.HideSelection = false;
            this.fileTypesList.Location = new System.Drawing.Point(8, 29);
            this.fileTypesList.Name = "fileTypesList";
            this.fileTypesList.Size = new System.Drawing.Size(301, 328);
            this.fileTypesList.TabIndex = 1;
            this.fileTypesList.UseCompatibleStateImageBehavior = false;
            this.fileTypesList.View = System.Windows.Forms.View.Details;
            // 
            // extensionColumn
            // 
            this.extensionColumn.Text = "Extension";
            this.extensionColumn.Width = 88;
            // 
            // descriptionColumn
            // 
            this.descriptionColumn.Text = "Description";
            this.descriptionColumn.Width = 159;
            // 
            // fileTypeContextMenu
            // 
            this.fileTypeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem1});
            this.fileTypeContextMenu.Name = "fileTypeContextMenu";
            this.fileTypeContextMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem1
            // 
            this.removeToolStripMenuItem1.Name = "removeToolStripMenuItem1";
            this.removeToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem1.Text = "Remove";
            this.removeToolStripMenuItem1.Click += new System.EventHandler(this.removeToolStripMenuItem1_Click);
            // 
            // urlProtocolList
            // 
            this.urlProtocolList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.urlProtocolList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.urlProtocolList.ContextMenuStrip = this.urlProtocolContextMenu;
            this.urlProtocolList.FormattingEnabled = true;
            this.urlProtocolList.Location = new System.Drawing.Point(315, 29);
            this.urlProtocolList.Name = "urlProtocolList";
            this.urlProtocolList.Size = new System.Drawing.Size(234, 379);
            this.urlProtocolList.TabIndex = 0;
            // 
            // urlProtocolContextMenu
            // 
            this.urlProtocolContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeToolStripMenuItem2});
            this.urlProtocolContextMenu.Name = "urlProtocolContextMenu";
            this.urlProtocolContextMenu.Size = new System.Drawing.Size(118, 26);
            // 
            // removeToolStripMenuItem2
            // 
            this.removeToolStripMenuItem2.Name = "removeToolStripMenuItem2";
            this.removeToolStripMenuItem2.Size = new System.Drawing.Size(117, 22);
            this.removeToolStripMenuItem2.Text = "Remove";
            this.removeToolStripMenuItem2.Click += new System.EventHandler(this.removeToolStripMenuItem2_Click);
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
            // msBuildLabel
            // 
            this.msBuildLabel.AutoSize = true;
            this.msBuildLabel.Location = new System.Drawing.Point(16, 117);
            this.msBuildLabel.Name = "msBuildLabel";
            this.msBuildLabel.Size = new System.Drawing.Size(71, 13);
            this.msBuildLabel.TabIndex = 19;
            this.msBuildLabel.Text = "MSBuild Path";
            // 
            // msBuildBox
            // 
            this.msBuildBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msBuildBox.Location = new System.Drawing.Point(113, 114);
            this.msBuildBox.Name = "msBuildBox";
            this.msBuildBox.Size = new System.Drawing.Size(410, 20);
            this.msBuildBox.TabIndex = 20;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 507);
            this.Controls.Add(this.mainTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(581, 513);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Installer Repacker";
            this.mainTabs.ResumeLayout(false);
            this.detailsTab.ResumeLayout(false);
            this.irGroup.ResumeLayout(false);
            this.irGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.irIconBox)).EndInit();
            this.itGroup.ResumeLayout(false);
            this.itGroup.PerformLayout();
            this.gGroup.ResumeLayout(false);
            this.gGroup.PerformLayout();
            this.filesTab.ResumeLayout(false);
            this.fFileListContextMenu.ResumeLayout(false);
            this.associationTab.ResumeLayout(false);
            this.associationTab.PerformLayout();
            this.fileTypeContextMenu.ResumeLayout(false);
            this.urlProtocolContextMenu.ResumeLayout(false);
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
        private System.Windows.Forms.Label fileTypeLbl;
        private System.Windows.Forms.ListView fileTypesList;
        private System.Windows.Forms.ListBox urlProtocolList;
        private System.Windows.Forms.Label urlProtocolLbl;
        private System.Windows.Forms.ColumnHeader extensionColumn;
        private System.Windows.Forms.ColumnHeader descriptionColumn;
        private System.Windows.Forms.Button fileTypeAddBtn;
        private System.Windows.Forms.Button urlProtocolAddBtn;
        private System.Windows.Forms.TextBox urlAssocTextBox;
        private System.Windows.Forms.TextBox fileTypeDescBox;
        private System.Windows.Forms.TextBox fileTypeExtBox;
        private System.Windows.Forms.Label fileTypeExtLbl;
        private System.Windows.Forms.Label fileTypeDescExt;
        private System.Windows.Forms.ContextMenuStrip fileTypeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip urlProtocolContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem2;
        private System.Windows.Forms.TextBox msBuildBox;
        private System.Windows.Forms.Label msBuildLabel;
    }
}

