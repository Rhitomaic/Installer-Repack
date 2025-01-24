namespace Uninstaller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.bottomBar = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.installationPanel = new System.Windows.Forms.Panel();
            this.deleteLabel = new System.Windows.Forms.Label();
            this.installationbar = new System.Windows.Forms.ProgressBar();
            this.installationTopLabel = new System.Windows.Forms.Label();
            this.installHeaderPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.installationLabel = new System.Windows.Forms.Label();
            this.bottomBar.SuspendLayout();
            this.installationPanel.SuspendLayout();
            this.installHeaderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // bottomBar
            // 
            this.bottomBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomBar.BackColor = System.Drawing.SystemColors.Control;
            this.bottomBar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottomBar.Controls.Add(this.cancelButton);
            this.bottomBar.Location = new System.Drawing.Point(-1, 308);
            this.bottomBar.Name = "bottomBar";
            this.bottomBar.Size = new System.Drawing.Size(486, 53);
            this.bottomBar.TabIndex = 11;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Enabled = false;
            this.cancelButton.Location = new System.Drawing.Point(394, 15);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // installationPanel
            // 
            this.installationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.installationPanel.BackColor = System.Drawing.SystemColors.Control;
            this.installationPanel.Controls.Add(this.deleteLabel);
            this.installationPanel.Controls.Add(this.installationbar);
            this.installationPanel.Controls.Add(this.installationTopLabel);
            this.installationPanel.Controls.Add(this.installHeaderPanel);
            this.installationPanel.Location = new System.Drawing.Point(0, 0);
            this.installationPanel.Name = "installationPanel";
            this.installationPanel.Size = new System.Drawing.Size(484, 308);
            this.installationPanel.TabIndex = 12;
            // 
            // deleteLabel
            // 
            this.deleteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteLabel.Location = new System.Drawing.Point(19, 130);
            this.deleteLabel.Name = "deleteLabel";
            this.deleteLabel.Size = new System.Drawing.Size(435, 42);
            this.deleteLabel.TabIndex = 6;
            this.deleteLabel.Text = "Deleting file:";
            // 
            // installationbar
            // 
            this.installationbar.Location = new System.Drawing.Point(22, 100);
            this.installationbar.Name = "installationbar";
            this.installationbar.Size = new System.Drawing.Size(436, 23);
            this.installationbar.TabIndex = 5;
            // 
            // installationTopLabel
            // 
            this.installationTopLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installationTopLabel.Location = new System.Drawing.Point(19, 72);
            this.installationTopLabel.Name = "installationTopLabel";
            this.installationTopLabel.Size = new System.Drawing.Size(435, 42);
            this.installationTopLabel.TabIndex = 3;
            this.installationTopLabel.Text = "Uninstalling...";
            // 
            // installHeaderPanel
            // 
            this.installHeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.installHeaderPanel.BackColor = System.Drawing.Color.White;
            this.installHeaderPanel.Controls.Add(this.label4);
            this.installHeaderPanel.Controls.Add(this.installationLabel);
            this.installHeaderPanel.Location = new System.Drawing.Point(-1, 0);
            this.installHeaderPanel.Name = "installHeaderPanel";
            this.installHeaderPanel.Size = new System.Drawing.Size(486, 52);
            this.installHeaderPanel.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(27, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(292, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Please wait until the program is removed from your computer.";
            // 
            // installationLabel
            // 
            this.installationLabel.AutoSize = true;
            this.installationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installationLabel.Location = new System.Drawing.Point(10, 11);
            this.installationLabel.Name = "installationLabel";
            this.installationLabel.Size = new System.Drawing.Size(96, 13);
            this.installationLabel.TabIndex = 1;
            this.installationLabel.Text = "Uninstall Status";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.bottomBar);
            this.Controls.Add(this.installationPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Uninstall";
            this.bottomBar.ResumeLayout(false);
            this.installationPanel.ResumeLayout(false);
            this.installHeaderPanel.ResumeLayout(false);
            this.installHeaderPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel bottomBar;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel installationPanel;
        private System.Windows.Forms.ProgressBar installationbar;
        private System.Windows.Forms.Label installationTopLabel;
        private System.Windows.Forms.Panel installHeaderPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label installationLabel;
        private System.Windows.Forms.Label deleteLabel;
    }
}

