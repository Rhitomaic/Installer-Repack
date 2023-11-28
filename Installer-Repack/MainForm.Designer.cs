namespace Installer_Repack
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
            this.wizardPicture = new System.Windows.Forms.PictureBox();
            this.titleLabel = new System.Windows.Forms.Label();
            this.bottomBar = new System.Windows.Forms.Panel();
            this.LeftestButton = new System.Windows.Forms.Button();
            this.LeftButton = new System.Windows.Forms.Button();
            this.RightButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.welcomePanel = new System.Windows.Forms.Panel();
            this.optionsPanel = new System.Windows.Forms.Panel();
            this.associationPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.fileTypeLabel = new System.Windows.Forms.Label();
            this.startMenuIconBox = new System.Windows.Forms.CheckBox();
            this.desktopIconBox = new System.Windows.Forms.CheckBox();
            this.iconsLabel = new System.Windows.Forms.Label();
            this.optionDescLabel = new System.Windows.Forms.Label();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerDescription = new System.Windows.Forms.Label();
            this.headerTitle = new System.Windows.Forms.Label();
            this.installerHeaderIcon = new System.Windows.Forms.PictureBox();
            this.pathPanel = new System.Windows.Forms.Panel();
            this.freeSpaceLabel = new System.Windows.Forms.Label();
            this.requiredSpaceLabel = new System.Windows.Forms.Label();
            this.pathGroupBox = new System.Windows.Forms.GroupBox();
            this.browsePathBox = new System.Windows.Forms.Button();
            this.destinationPathBox = new System.Windows.Forms.TextBox();
            this.pathGuideLabel = new System.Windows.Forms.Label();
            this.pathDescLabel = new System.Windows.Forms.Label();
            this.pathHeaderPanel = new System.Windows.Forms.Panel();
            this.pathHeaderDesc = new System.Windows.Forms.Label();
            this.pathHeaderTitle = new System.Windows.Forms.Label();
            this.pathHeaderIcon = new System.Windows.Forms.PictureBox();
            this.installationPanel = new System.Windows.Forms.Panel();
            this.installationbar = new System.Windows.Forms.ProgressBar();
            this.installationProgLabel = new System.Windows.Forms.Label();
            this.installationTopLabel = new System.Windows.Forms.Label();
            this.installHeaderPanel = new System.Windows.Forms.Panel();
            this.installHeaderDesc = new System.Windows.Forms.Label();
            this.installationLabel = new System.Windows.Forms.Label();
            this.installationIcon = new System.Windows.Forms.PictureBox();
            this.finishPanel = new System.Windows.Forms.Panel();
            this.finishLaunchBox = new System.Windows.Forms.CheckBox();
            this.finishDescLabel = new System.Windows.Forms.Label();
            this.finishTitleLabel = new System.Windows.Forms.Label();
            this.finishImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.wizardPicture)).BeginInit();
            this.bottomBar.SuspendLayout();
            this.welcomePanel.SuspendLayout();
            this.optionsPanel.SuspendLayout();
            this.associationPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installerHeaderIcon)).BeginInit();
            this.pathPanel.SuspendLayout();
            this.pathGroupBox.SuspendLayout();
            this.pathHeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pathHeaderIcon)).BeginInit();
            this.installationPanel.SuspendLayout();
            this.installHeaderPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installationIcon)).BeginInit();
            this.finishPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.finishImage)).BeginInit();
            this.SuspendLayout();
            // 
            // wizardPicture
            // 
            this.wizardPicture.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wizardPicture.Image = global::Installer_Repack.InstallerResource.Wizard;
            this.wizardPicture.Location = new System.Drawing.Point(0, 1);
            this.wizardPicture.Name = "wizardPicture";
            this.wizardPicture.Size = new System.Drawing.Size(164, 314);
            this.wizardPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.wizardPicture.TabIndex = 0;
            this.wizardPicture.TabStop = false;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.titleLabel.Location = new System.Drawing.Point(176, 15);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(199, 25);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "{PROGRAM} Installer";
            // 
            // bottomBar
            // 
            this.bottomBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomBar.BackColor = System.Drawing.SystemColors.Control;
            this.bottomBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomBar.Controls.Add(this.LeftestButton);
            this.bottomBar.Controls.Add(this.LeftButton);
            this.bottomBar.Controls.Add(this.RightButton);
            this.bottomBar.Location = new System.Drawing.Point(-1, 308);
            this.bottomBar.Name = "bottomBar";
            this.bottomBar.Size = new System.Drawing.Size(486, 53);
            this.bottomBar.TabIndex = 2;
            // 
            // LeftestButton
            // 
            this.LeftestButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftestButton.Location = new System.Drawing.Point(237, 15);
            this.LeftestButton.Name = "LeftestButton";
            this.LeftestButton.Size = new System.Drawing.Size(75, 23);
            this.LeftestButton.TabIndex = 2;
            this.LeftestButton.Text = "< Back";
            this.LeftestButton.UseVisualStyleBackColor = true;
            this.LeftestButton.Visible = false;
            this.LeftestButton.Click += new System.EventHandler(this.LeftestButton_Click);
            // 
            // LeftButton
            // 
            this.LeftButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LeftButton.Location = new System.Drawing.Point(313, 15);
            this.LeftButton.Name = "LeftButton";
            this.LeftButton.Size = new System.Drawing.Size(75, 23);
            this.LeftButton.TabIndex = 0;
            this.LeftButton.Text = "Next >";
            this.LeftButton.UseVisualStyleBackColor = true;
            this.LeftButton.Click += new System.EventHandler(this.LeftButton_Click);
            // 
            // RightButton
            // 
            this.RightButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RightButton.Location = new System.Drawing.Point(394, 15);
            this.RightButton.Name = "RightButton";
            this.RightButton.Size = new System.Drawing.Size(75, 23);
            this.RightButton.TabIndex = 1;
            this.RightButton.Text = "Cancel";
            this.RightButton.UseVisualStyleBackColor = true;
            this.RightButton.Click += new System.EventHandler(this.RightButton_Click);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.descriptionLabel.Location = new System.Drawing.Point(178, 57);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(290, 116);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "This will install {PROGRAM} on your computer. It is recommended to close all your" +
    " applications before continuing. Click Next to continue.";
            // 
            // welcomePanel
            // 
            this.welcomePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.welcomePanel.Controls.Add(this.descriptionLabel);
            this.welcomePanel.Controls.Add(this.titleLabel);
            this.welcomePanel.Controls.Add(this.wizardPicture);
            this.welcomePanel.Location = new System.Drawing.Point(0, 0);
            this.welcomePanel.Name = "welcomePanel";
            this.welcomePanel.Size = new System.Drawing.Size(484, 308);
            this.welcomePanel.TabIndex = 4;
            // 
            // optionsPanel
            // 
            this.optionsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionsPanel.BackColor = System.Drawing.SystemColors.Control;
            this.optionsPanel.Controls.Add(this.associationPanel);
            this.optionsPanel.Controls.Add(this.startMenuIconBox);
            this.optionsPanel.Controls.Add(this.desktopIconBox);
            this.optionsPanel.Controls.Add(this.iconsLabel);
            this.optionsPanel.Controls.Add(this.optionDescLabel);
            this.optionsPanel.Controls.Add(this.headerPanel);
            this.optionsPanel.Location = new System.Drawing.Point(0, 0);
            this.optionsPanel.Name = "optionsPanel";
            this.optionsPanel.Size = new System.Drawing.Size(484, 308);
            this.optionsPanel.TabIndex = 5;
            // 
            // associationPanel
            // 
            this.associationPanel.Controls.Add(this.fileTypeLabel);
            this.associationPanel.Location = new System.Drawing.Point(20, 183);
            this.associationPanel.Name = "associationPanel";
            this.associationPanel.Size = new System.Drawing.Size(431, 100);
            this.associationPanel.TabIndex = 10;
            // 
            // fileTypeLabel
            // 
            this.fileTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileTypeLabel.Location = new System.Drawing.Point(3, 0);
            this.fileTypeLabel.Name = "fileTypeLabel";
            this.fileTypeLabel.Size = new System.Drawing.Size(179, 17);
            this.fileTypeLabel.TabIndex = 7;
            this.fileTypeLabel.Text = "File type association:";
            // 
            // startMenuIconBox
            // 
            this.startMenuIconBox.AutoSize = true;
            this.startMenuIconBox.Checked = true;
            this.startMenuIconBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.startMenuIconBox.Location = new System.Drawing.Point(24, 161);
            this.startMenuIconBox.Name = "startMenuIconBox";
            this.startMenuIconBox.Size = new System.Drawing.Size(138, 17);
            this.startMenuIconBox.TabIndex = 6;
            this.startMenuIconBox.Text = "Create start menu folder";
            this.startMenuIconBox.UseVisualStyleBackColor = true;
            // 
            // desktopIconBox
            // 
            this.desktopIconBox.AutoSize = true;
            this.desktopIconBox.Checked = true;
            this.desktopIconBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.desktopIconBox.Location = new System.Drawing.Point(24, 138);
            this.desktopIconBox.Name = "desktopIconBox";
            this.desktopIconBox.Size = new System.Drawing.Size(121, 17);
            this.desktopIconBox.TabIndex = 5;
            this.desktopIconBox.Text = "Create desktop icon";
            this.desktopIconBox.UseVisualStyleBackColor = true;
            // 
            // iconsLabel
            // 
            this.iconsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iconsLabel.Location = new System.Drawing.Point(24, 118);
            this.iconsLabel.Name = "iconsLabel";
            this.iconsLabel.Size = new System.Drawing.Size(435, 17);
            this.iconsLabel.TabIndex = 4;
            this.iconsLabel.Text = "Additonal Icons:";
            // 
            // optionDescLabel
            // 
            this.optionDescLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionDescLabel.Location = new System.Drawing.Point(22, 76);
            this.optionDescLabel.Name = "optionDescLabel";
            this.optionDescLabel.Size = new System.Drawing.Size(435, 42);
            this.optionDescLabel.TabIndex = 3;
            this.optionDescLabel.Text = "Select the additional options you would like the setup to perform while installin" +
    "g {PROGRAM}, then click Next.";
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.headerDescription);
            this.headerPanel.Controls.Add(this.headerTitle);
            this.headerPanel.Controls.Add(this.installerHeaderIcon);
            this.headerPanel.Location = new System.Drawing.Point(-1, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(486, 52);
            this.headerPanel.TabIndex = 0;
            // 
            // headerDescription
            // 
            this.headerDescription.AutoSize = true;
            this.headerDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerDescription.Location = new System.Drawing.Point(27, 27);
            this.headerDescription.Name = "headerDescription";
            this.headerDescription.Size = new System.Drawing.Size(219, 13);
            this.headerDescription.TabIndex = 2;
            this.headerDescription.Text = "Which additional tasks should be performed?";
            // 
            // headerTitle
            // 
            this.headerTitle.AutoSize = true;
            this.headerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerTitle.Location = new System.Drawing.Point(10, 11);
            this.headerTitle.Name = "headerTitle";
            this.headerTitle.Size = new System.Drawing.Size(150, 13);
            this.headerTitle.TabIndex = 1;
            this.headerTitle.Text = "Select Additional Options";
            // 
            // installerHeaderIcon
            // 
            this.installerHeaderIcon.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.installerHeaderIcon.Image = global::Installer_Repack.InstallerResource.Header;
            this.installerHeaderIcon.Location = new System.Drawing.Point(433, 0);
            this.installerHeaderIcon.Name = "installerHeaderIcon";
            this.installerHeaderIcon.Size = new System.Drawing.Size(53, 52);
            this.installerHeaderIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.installerHeaderIcon.TabIndex = 0;
            this.installerHeaderIcon.TabStop = false;
            // 
            // pathPanel
            // 
            this.pathPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathPanel.BackColor = System.Drawing.SystemColors.Control;
            this.pathPanel.Controls.Add(this.freeSpaceLabel);
            this.pathPanel.Controls.Add(this.requiredSpaceLabel);
            this.pathPanel.Controls.Add(this.pathGroupBox);
            this.pathPanel.Controls.Add(this.pathGuideLabel);
            this.pathPanel.Controls.Add(this.pathDescLabel);
            this.pathPanel.Controls.Add(this.pathHeaderPanel);
            this.pathPanel.Location = new System.Drawing.Point(0, 0);
            this.pathPanel.Name = "pathPanel";
            this.pathPanel.Size = new System.Drawing.Size(484, 308);
            this.pathPanel.TabIndex = 10;
            // 
            // freeSpaceLabel
            // 
            this.freeSpaceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.freeSpaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.freeSpaceLabel.Location = new System.Drawing.Point(16, 275);
            this.freeSpaceLabel.Name = "freeSpaceLabel";
            this.freeSpaceLabel.Size = new System.Drawing.Size(435, 17);
            this.freeSpaceLabel.TabIndex = 7;
            this.freeSpaceLabel.Text = "Available free space:";
            // 
            // requiredSpaceLabel
            // 
            this.requiredSpaceLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requiredSpaceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.requiredSpaceLabel.Location = new System.Drawing.Point(16, 256);
            this.requiredSpaceLabel.Name = "requiredSpaceLabel";
            this.requiredSpaceLabel.Size = new System.Drawing.Size(435, 17);
            this.requiredSpaceLabel.TabIndex = 6;
            this.requiredSpaceLabel.Text = "Required free space:";
            // 
            // pathGroupBox
            // 
            this.pathGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathGroupBox.Controls.Add(this.browsePathBox);
            this.pathGroupBox.Controls.Add(this.destinationPathBox);
            this.pathGroupBox.Location = new System.Drawing.Point(22, 160);
            this.pathGroupBox.Name = "pathGroupBox";
            this.pathGroupBox.Size = new System.Drawing.Size(432, 57);
            this.pathGroupBox.TabIndex = 5;
            this.pathGroupBox.TabStop = false;
            this.pathGroupBox.Text = "Destination Folder";
            // 
            // browsePathBox
            // 
            this.browsePathBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.browsePathBox.Location = new System.Drawing.Point(347, 21);
            this.browsePathBox.Name = "browsePathBox";
            this.browsePathBox.Size = new System.Drawing.Size(75, 23);
            this.browsePathBox.TabIndex = 1;
            this.browsePathBox.Text = "Browse...";
            this.browsePathBox.UseVisualStyleBackColor = true;
            this.browsePathBox.Click += new System.EventHandler(this.browsePathBox_Click);
            // 
            // destinationPathBox
            // 
            this.destinationPathBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.destinationPathBox.Location = new System.Drawing.Point(11, 23);
            this.destinationPathBox.Name = "destinationPathBox";
            this.destinationPathBox.Size = new System.Drawing.Size(330, 20);
            this.destinationPathBox.TabIndex = 0;
            this.destinationPathBox.Text = "DefaultPath";
            this.destinationPathBox.TextChanged += new System.EventHandler(this.destinationPathBox_TextChanged);
            // 
            // pathGuideLabel
            // 
            this.pathGuideLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathGuideLabel.Location = new System.Drawing.Point(21, 106);
            this.pathGuideLabel.Name = "pathGuideLabel";
            this.pathGuideLabel.Size = new System.Drawing.Size(435, 17);
            this.pathGuideLabel.TabIndex = 4;
            this.pathGuideLabel.Text = "To continue, click Next. If you would like to select a different folder, click Br" +
    "owse.";
            // 
            // pathDescLabel
            // 
            this.pathDescLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathDescLabel.Location = new System.Drawing.Point(22, 76);
            this.pathDescLabel.Name = "pathDescLabel";
            this.pathDescLabel.Size = new System.Drawing.Size(435, 42);
            this.pathDescLabel.TabIndex = 3;
            this.pathDescLabel.Text = "Setup will install {PROGRAM} in the folder shown below.";
            // 
            // pathHeaderPanel
            // 
            this.pathHeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathHeaderPanel.BackColor = System.Drawing.Color.White;
            this.pathHeaderPanel.Controls.Add(this.pathHeaderDesc);
            this.pathHeaderPanel.Controls.Add(this.pathHeaderTitle);
            this.pathHeaderPanel.Controls.Add(this.pathHeaderIcon);
            this.pathHeaderPanel.Location = new System.Drawing.Point(-1, 0);
            this.pathHeaderPanel.Name = "pathHeaderPanel";
            this.pathHeaderPanel.Size = new System.Drawing.Size(486, 52);
            this.pathHeaderPanel.TabIndex = 0;
            // 
            // pathHeaderDesc
            // 
            this.pathHeaderDesc.AutoSize = true;
            this.pathHeaderDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathHeaderDesc.Location = new System.Drawing.Point(27, 27);
            this.pathHeaderDesc.Name = "pathHeaderDesc";
            this.pathHeaderDesc.Size = new System.Drawing.Size(223, 13);
            this.pathHeaderDesc.TabIndex = 2;
            this.pathHeaderDesc.Text = "Please choose the directory for the installation";
            // 
            // pathHeaderTitle
            // 
            this.pathHeaderTitle.AutoSize = true;
            this.pathHeaderTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pathHeaderTitle.Location = new System.Drawing.Point(10, 11);
            this.pathHeaderTitle.Name = "pathHeaderTitle";
            this.pathHeaderTitle.Size = new System.Drawing.Size(149, 13);
            this.pathHeaderTitle.TabIndex = 1;
            this.pathHeaderTitle.Text = "Select Application Folder";
            // 
            // pathHeaderIcon
            // 
            this.pathHeaderIcon.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.pathHeaderIcon.Image = global::Installer_Repack.InstallerResource.Header;
            this.pathHeaderIcon.Location = new System.Drawing.Point(433, 0);
            this.pathHeaderIcon.Name = "pathHeaderIcon";
            this.pathHeaderIcon.Size = new System.Drawing.Size(53, 52);
            this.pathHeaderIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pathHeaderIcon.TabIndex = 0;
            this.pathHeaderIcon.TabStop = false;
            // 
            // installationPanel
            // 
            this.installationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.installationPanel.BackColor = System.Drawing.SystemColors.Control;
            this.installationPanel.Controls.Add(this.installationbar);
            this.installationPanel.Controls.Add(this.installationProgLabel);
            this.installationPanel.Controls.Add(this.installationTopLabel);
            this.installationPanel.Controls.Add(this.installHeaderPanel);
            this.installationPanel.Location = new System.Drawing.Point(0, 0);
            this.installationPanel.Name = "installationPanel";
            this.installationPanel.Size = new System.Drawing.Size(484, 308);
            this.installationPanel.TabIndex = 10;
            // 
            // installationbar
            // 
            this.installationbar.Location = new System.Drawing.Point(22, 112);
            this.installationbar.Name = "installationbar";
            this.installationbar.Size = new System.Drawing.Size(436, 23);
            this.installationbar.TabIndex = 5;
            // 
            // installationProgLabel
            // 
            this.installationProgLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installationProgLabel.Location = new System.Drawing.Point(19, 94);
            this.installationProgLabel.Name = "installationProgLabel";
            this.installationProgLabel.Size = new System.Drawing.Size(435, 17);
            this.installationProgLabel.TabIndex = 4;
            this.installationProgLabel.Text = "Preparing...";
            // 
            // installationTopLabel
            // 
            this.installationTopLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installationTopLabel.Location = new System.Drawing.Point(19, 74);
            this.installationTopLabel.Name = "installationTopLabel";
            this.installationTopLabel.Size = new System.Drawing.Size(435, 42);
            this.installationTopLabel.TabIndex = 3;
            this.installationTopLabel.Text = "Installing...";
            // 
            // installHeaderPanel
            // 
            this.installHeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.installHeaderPanel.BackColor = System.Drawing.Color.White;
            this.installHeaderPanel.Controls.Add(this.installHeaderDesc);
            this.installHeaderPanel.Controls.Add(this.installationLabel);
            this.installHeaderPanel.Controls.Add(this.installationIcon);
            this.installHeaderPanel.Location = new System.Drawing.Point(-1, 0);
            this.installHeaderPanel.Name = "installHeaderPanel";
            this.installHeaderPanel.Size = new System.Drawing.Size(486, 52);
            this.installHeaderPanel.TabIndex = 0;
            // 
            // installHeaderDesc
            // 
            this.installHeaderDesc.AutoSize = true;
            this.installHeaderDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installHeaderDesc.Location = new System.Drawing.Point(27, 27);
            this.installHeaderDesc.Name = "installHeaderDesc";
            this.installHeaderDesc.Size = new System.Drawing.Size(320, 13);
            this.installHeaderDesc.TabIndex = 2;
            this.installHeaderDesc.Text = "Please wait while the setup installs {PROGRAM} on your computer";
            // 
            // installationLabel
            // 
            this.installationLabel.AutoSize = true;
            this.installationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installationLabel.Location = new System.Drawing.Point(10, 11);
            this.installationLabel.Name = "installationLabel";
            this.installationLabel.Size = new System.Drawing.Size(58, 13);
            this.installationLabel.TabIndex = 1;
            this.installationLabel.Text = "Installing";
            // 
            // installationIcon
            // 
            this.installationIcon.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.installationIcon.Image = global::Installer_Repack.InstallerResource.Header;
            this.installationIcon.Location = new System.Drawing.Point(433, 0);
            this.installationIcon.Name = "installationIcon";
            this.installationIcon.Size = new System.Drawing.Size(53, 52);
            this.installationIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.installationIcon.TabIndex = 0;
            this.installationIcon.TabStop = false;
            // 
            // finishPanel
            // 
            this.finishPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.finishPanel.Controls.Add(this.finishLaunchBox);
            this.finishPanel.Controls.Add(this.finishDescLabel);
            this.finishPanel.Controls.Add(this.finishTitleLabel);
            this.finishPanel.Controls.Add(this.finishImage);
            this.finishPanel.Location = new System.Drawing.Point(0, 0);
            this.finishPanel.Name = "finishPanel";
            this.finishPanel.Size = new System.Drawing.Size(484, 308);
            this.finishPanel.TabIndex = 5;
            // 
            // finishLaunchBox
            // 
            this.finishLaunchBox.AutoSize = true;
            this.finishLaunchBox.Checked = true;
            this.finishLaunchBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.finishLaunchBox.Location = new System.Drawing.Point(181, 127);
            this.finishLaunchBox.Name = "finishLaunchBox";
            this.finishLaunchBox.Size = new System.Drawing.Size(170, 17);
            this.finishLaunchBox.TabIndex = 4;
            this.finishLaunchBox.Text = "Launch {PROGRAM} on finish";
            this.finishLaunchBox.UseVisualStyleBackColor = true;
            // 
            // finishDescLabel
            // 
            this.finishDescLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F);
            this.finishDescLabel.Location = new System.Drawing.Point(178, 79);
            this.finishDescLabel.Name = "finishDescLabel";
            this.finishDescLabel.Size = new System.Drawing.Size(290, 116);
            this.finishDescLabel.TabIndex = 3;
            this.finishDescLabel.Text = "Setup has finished installing {PROGRAM} on your computer! Click Finish to exit se" +
    "tup.";
            // 
            // finishTitleLabel
            // 
            this.finishTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.finishTitleLabel.Location = new System.Drawing.Point(176, 15);
            this.finishTitleLabel.Name = "finishTitleLabel";
            this.finishTitleLabel.Size = new System.Drawing.Size(273, 65);
            this.finishTitleLabel.TabIndex = 1;
            this.finishTitleLabel.Text = "Successfully installed {PROGRAM} on your computer!";
            // 
            // finishImage
            // 
            this.finishImage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.finishImage.Image = global::Installer_Repack.InstallerResource.Wizard;
            this.finishImage.Location = new System.Drawing.Point(0, 1);
            this.finishImage.Name = "finishImage";
            this.finishImage.Size = new System.Drawing.Size(164, 314);
            this.finishImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.finishImage.TabIndex = 0;
            this.finishImage.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.bottomBar);
            this.Controls.Add(this.welcomePanel);
            this.Controls.Add(this.pathPanel);
            this.Controls.Add(this.installationPanel);
            this.Controls.Add(this.optionsPanel);
            this.Controls.Add(this.finishPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.Text = "Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.wizardPicture)).EndInit();
            this.bottomBar.ResumeLayout(false);
            this.welcomePanel.ResumeLayout(false);
            this.welcomePanel.PerformLayout();
            this.optionsPanel.ResumeLayout(false);
            this.optionsPanel.PerformLayout();
            this.associationPanel.ResumeLayout(false);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installerHeaderIcon)).EndInit();
            this.pathPanel.ResumeLayout(false);
            this.pathGroupBox.ResumeLayout(false);
            this.pathGroupBox.PerformLayout();
            this.pathHeaderPanel.ResumeLayout(false);
            this.pathHeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pathHeaderIcon)).EndInit();
            this.installationPanel.ResumeLayout(false);
            this.installHeaderPanel.ResumeLayout(false);
            this.installHeaderPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.installationIcon)).EndInit();
            this.finishPanel.ResumeLayout(false);
            this.finishPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.finishImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox wizardPicture;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Panel bottomBar;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Button LeftButton;
        private System.Windows.Forms.Button RightButton;
        private System.Windows.Forms.Panel welcomePanel;
        private System.Windows.Forms.Panel optionsPanel;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox installerHeaderIcon;
        private System.Windows.Forms.Label headerTitle;
        private System.Windows.Forms.Label headerDescription;
        private System.Windows.Forms.Label optionDescLabel;
        private System.Windows.Forms.Label iconsLabel;
        private System.Windows.Forms.CheckBox startMenuIconBox;
        private System.Windows.Forms.CheckBox desktopIconBox;
        private System.Windows.Forms.Label fileTypeLabel;
        private System.Windows.Forms.Panel pathPanel;
        private System.Windows.Forms.Label pathGuideLabel;
        private System.Windows.Forms.Label pathDescLabel;
        private System.Windows.Forms.Panel pathHeaderPanel;
        private System.Windows.Forms.Label pathHeaderDesc;
        private System.Windows.Forms.Label pathHeaderTitle;
        private System.Windows.Forms.PictureBox pathHeaderIcon;
        private System.Windows.Forms.GroupBox pathGroupBox;
        private System.Windows.Forms.Button browsePathBox;
        private System.Windows.Forms.TextBox destinationPathBox;
        private System.Windows.Forms.Label requiredSpaceLabel;
        private System.Windows.Forms.Label freeSpaceLabel;
        private System.Windows.Forms.Panel installationPanel;
        private System.Windows.Forms.Label installationProgLabel;
        private System.Windows.Forms.Label installationTopLabel;
        private System.Windows.Forms.Panel installHeaderPanel;
        private System.Windows.Forms.Label installHeaderDesc;
        private System.Windows.Forms.Label installationLabel;
        private System.Windows.Forms.PictureBox installationIcon;
        private System.Windows.Forms.ProgressBar installationbar;
        private System.Windows.Forms.Panel finishPanel;
        private System.Windows.Forms.Label finishDescLabel;
        private System.Windows.Forms.PictureBox finishImage;
        private System.Windows.Forms.CheckBox finishLaunchBox;
        private System.Windows.Forms.Label finishTitleLabel;
        private System.Windows.Forms.Button LeftestButton;
        private System.Windows.Forms.FlowLayoutPanel associationPanel;
    }
}

