namespace FileDialogs
{
    partial class FileDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileDialog));
            this.lookInLabel = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.backToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.upOneLevelToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.searchTheWebToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.deleteToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.createNewFolderToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.viewsToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.thumbnailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sideBySideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectNetworkDriveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.propertyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fileNameLabel = new System.Windows.Forms.Label();
            this.fileNameComboBox = new System.Windows.Forms.ComboBox();
            this.fileTypeLabel = new System.Windows.Forms.Label();
            this.fileTypeComboBox = new System.Windows.Forms.ComboBox();
            this.okButton = new FileDialogs.SplitButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.placesBar = new System.Windows.Forms.ToolStrip();
            this.lookInComboBox = new FileDialogs.LookInComboBox();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lookInLabel
            // 
            this.lookInLabel.AutoSize = true;
            this.lookInLabel.Location = new System.Drawing.Point(35, 6);
            this.lookInLabel.Name = "lookInLabel";
            this.lookInLabel.Size = new System.Drawing.Size(44, 13);
            this.lookInLabel.TabIndex = 6;
            this.lookInLabel.Text = "Look &in:";
            // 
            // toolStrip
            // 
            this.toolStrip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backToolStripSplitButton,
            this.upOneLevelToolStripButton,
            this.toolStripSeparator1,
            this.searchTheWebToolStripButton,
            this.deleteToolStripButton,
            this.createNewFolderToolStripButton,
            this.viewsToolStripSplitButton,
            this.toolsToolStripDropDownButton});
            this.toolStrip.Location = new System.Drawing.Point(333, 3);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip.Size = new System.Drawing.Size(210, 25);
            this.toolStrip.TabIndex = 8;
            this.toolStrip.TabStop = true;
            // 
            // backToolStripSplitButton
            // 
            this.backToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backToolStripSplitButton.Enabled = false;
            this.backToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("backToolStripSplitButton.Image")));
            this.backToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.backToolStripSplitButton.Name = "backToolStripSplitButton";
            this.backToolStripSplitButton.Size = new System.Drawing.Size(32, 22);
            this.backToolStripSplitButton.ButtonClick += new System.EventHandler(this.backToolStripSplitButton_ButtonClick);
            this.backToolStripSplitButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.backToolStripSplitButton_DropDownItemClicked);
            // 
            // upOneLevelToolStripButton
            // 
            this.upOneLevelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.upOneLevelToolStripButton.Enabled = false;
            this.upOneLevelToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("upOneLevelToolStripButton.Image")));
            this.upOneLevelToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.upOneLevelToolStripButton.Name = "upOneLevelToolStripButton";
            this.upOneLevelToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.upOneLevelToolStripButton.ToolTipText = "Up One Level (Alt+2)";
            this.upOneLevelToolStripButton.Click += new System.EventHandler(this.upOneLevelToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // searchTheWebToolStripButton
            // 
            this.searchTheWebToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.searchTheWebToolStripButton.Enabled = false;
            this.searchTheWebToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("searchTheWebToolStripButton.Image")));
            this.searchTheWebToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.searchTheWebToolStripButton.Name = "searchTheWebToolStripButton";
            this.searchTheWebToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.searchTheWebToolStripButton.ToolTipText = "Search the Web (Alt+3)";
            // 
            // deleteToolStripButton
            // 
            this.deleteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.deleteToolStripButton.Enabled = false;
            this.deleteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripButton.Image")));
            this.deleteToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.deleteToolStripButton.Name = "deleteToolStripButton";
            this.deleteToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.deleteToolStripButton.ToolTipText = "Delete (Del)";
            this.deleteToolStripButton.Click += new System.EventHandler(this.deleteToolStripButton_Click);
            // 
            // createNewFolderToolStripButton
            // 
            this.createNewFolderToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.createNewFolderToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("createNewFolderToolStripButton.Image")));
            this.createNewFolderToolStripButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.createNewFolderToolStripButton.Name = "createNewFolderToolStripButton";
            this.createNewFolderToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.createNewFolderToolStripButton.ToolTipText = "Create New Folder (Alt+5)";
            this.createNewFolderToolStripButton.Click += new System.EventHandler(this.createNewFolderToolStripButton_Click);
            // 
            // viewsToolStripSplitButton
            // 
            this.viewsToolStripSplitButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.viewsToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thumbnailsToolStripMenuItem,
            this.sideBySideToolStripMenuItem,
            this.iconsToolStripMenuItem,
            this.listToolStripMenuItem,
            this.detailsToolStripMenuItem});
            this.viewsToolStripSplitButton.Image = ((System.Drawing.Image)(resources.GetObject("viewsToolStripSplitButton.Image")));
            this.viewsToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.viewsToolStripSplitButton.Name = "viewsToolStripSplitButton";
            this.viewsToolStripSplitButton.Size = new System.Drawing.Size(32, 22);
            this.viewsToolStripSplitButton.ToolTipText = "Views";
            this.viewsToolStripSplitButton.ButtonClick += new System.EventHandler(this.viewsToolStripSplitButton_ButtonClick);
            this.viewsToolStripSplitButton.DropDownOpening += new System.EventHandler(this.viewsToolStripSplitButton_DropDownOpening);
            this.viewsToolStripSplitButton.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.viewsToolStripSplitButton_DropDownItemClicked);
            // 
            // thumbnailsToolStripMenuItem
            // 
            this.thumbnailsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("thumbnailsToolStripMenuItem.Image")));
            this.thumbnailsToolStripMenuItem.Name = "thumbnailsToolStripMenuItem";
            this.thumbnailsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.thumbnailsToolStripMenuItem.Text = "&Thumbnails";
            // 
            // sideBySideToolStripMenuItem
            // 
            this.sideBySideToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("sideBySideToolStripMenuItem.Image")));
            this.sideBySideToolStripMenuItem.Name = "sideBySideToolStripMenuItem";
            this.sideBySideToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.sideBySideToolStripMenuItem.Text = "Tile&s";
            // 
            // iconsToolStripMenuItem
            // 
            this.iconsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("iconsToolStripMenuItem.Image")));
            this.iconsToolStripMenuItem.Name = "iconsToolStripMenuItem";
            this.iconsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.iconsToolStripMenuItem.Text = "Ico&ns";
            // 
            // listToolStripMenuItem
            // 
            this.listToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("listToolStripMenuItem.Image")));
            this.listToolStripMenuItem.Name = "listToolStripMenuItem";
            this.listToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.listToolStripMenuItem.Text = "&List";
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("detailsToolStripMenuItem.Image")));
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.detailsToolStripMenuItem.Text = "&Details";
            // 
            // toolsToolStripDropDownButton
            // 
            this.toolsToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolsToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.connectNetworkDriveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.propertyToolStripMenuItem});
            this.toolsToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("toolsToolStripDropDownButton.Image")));
            this.toolsToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolsToolStripDropDownButton.Name = "toolsToolStripDropDownButton";
            this.toolsToolStripDropDownButton.Size = new System.Drawing.Size(45, 22);
            this.toolsToolStripDropDownButton.Text = "Too&ls";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("deleteToolStripMenuItem.Image")));
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeyDisplayString = "Del";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripButton_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.renameToolStripMenuItem.ShowShortcutKeys = false;
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.renameToolStripMenuItem.Text = "&Rename";
            this.renameToolStripMenuItem.Visible = false;
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // connectNetworkDriveToolStripMenuItem
            // 
            this.connectNetworkDriveToolStripMenuItem.Name = "connectNetworkDriveToolStripMenuItem";
            this.connectNetworkDriveToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.connectNetworkDriveToolStripMenuItem.Text = "Map &Network Drive...";
            this.connectNetworkDriveToolStripMenuItem.Click += new System.EventHandler(this.connectNetworkDriveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(185, 6);
            this.toolStripMenuItem1.Visible = false;
            // 
            // propertyToolStripMenuItem
            // 
            this.propertyToolStripMenuItem.Name = "propertyToolStripMenuItem";
            this.propertyToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.propertyToolStripMenuItem.Text = "&Properties";
            this.propertyToolStripMenuItem.Visible = false;
            this.propertyToolStripMenuItem.Click += new System.EventHandler(this.propertyToolStripMenuItem_Click);
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(95, 310);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(56, 13);
            this.fileNameLabel.TabIndex = 0;
            this.fileNameLabel.Text = "File &name:";
            // 
            // fileNameComboBox
            // 
            this.fileNameComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameComboBox.FormattingEnabled = true;
            this.fileNameComboBox.Location = new System.Drawing.Point(171, 309);
            this.fileNameComboBox.Name = "fileNameComboBox";
            this.fileNameComboBox.Size = new System.Drawing.Size(299, 21);
            this.fileNameComboBox.TabIndex = 1;
            this.fileNameComboBox.TextChanged += new System.EventHandler(this.fileNameComboBox_TextChanged);
            // 
            // fileTypeLabel
            // 
            this.fileTypeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.fileTypeLabel.AutoSize = true;
            this.fileTypeLabel.Location = new System.Drawing.Point(95, 336);
            this.fileTypeLabel.Name = "fileTypeLabel";
            this.fileTypeLabel.Size = new System.Drawing.Size(70, 13);
            this.fileTypeLabel.TabIndex = 2;
            this.fileTypeLabel.Text = "Files of &type:";
            // 
            // fileTypeComboBox
            // 
            this.fileTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.fileTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fileTypeComboBox.FormattingEnabled = true;
            this.fileTypeComboBox.Location = new System.Drawing.Point(171, 335);
            this.fileTypeComboBox.Name = "fileTypeComboBox";
            this.fileTypeComboBox.Size = new System.Drawing.Size(299, 21);
            this.fileTypeComboBox.TabIndex = 3;
            this.fileTypeComboBox.SelectionChangeCommitted += new System.EventHandler(this.fileTypeComboBox_SelectionChangeCommitted);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.AutoSize = true;
            this.okButton.Location = new System.Drawing.Point(491, 309);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(96, 23);
            this.okButton.TabIndex = 4;
            this.okButton.Text = "&Open";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            this.okButton.EnabledChanged += new System.EventHandler(this.okButton_EnabledChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(492, 334);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(95, 22);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // placesBar
            // 
            this.placesBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.placesBar.AutoSize = false;
            this.placesBar.Dock = System.Windows.Forms.DockStyle.None;
            this.placesBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.placesBar.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.placesBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
            this.placesBar.Location = new System.Drawing.Point(5, 28);
            this.placesBar.Name = "placesBar";
            this.placesBar.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.placesBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.placesBar.Size = new System.Drawing.Size(87, 327);
            this.placesBar.Stretch = true;
            this.placesBar.TabIndex = 10;
            this.placesBar.TabStop = true;
            this.placesBar.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.placesStrip_ItemClicked);
            // 
            // lookInComboBox
            // 
            this.lookInComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lookInComboBox.CurrentItem = null;
            this.lookInComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lookInComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.lookInComboBox.FormattingEnabled = true;
            this.lookInComboBox.Location = new System.Drawing.Point(98, 3);
            this.lookInComboBox.Name = "lookInComboBox";
            this.lookInComboBox.Size = new System.Drawing.Size(216, 22);
            this.lookInComboBox.TabIndex = 7;
            this.lookInComboBox.SelectionChangeCommitted += new System.EventHandler(this.lookInComboBox_SelectionChangeCommitted);
            this.lookInComboBox.DropDown += new System.EventHandler(this.lookInComboBox_DropDown);
            // 
            // FileDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(593, 359);
            this.Controls.Add(this.placesBar);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fileTypeComboBox);
            this.Controls.Add(this.fileTypeLabel);
            this.Controls.Add(this.fileNameComboBox);
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.lookInComboBox);
            this.Controls.Add(this.lookInLabel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(601, 393);
            this.Name = "FileDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private FileDialogs.LookInComboBox  lookInComboBox;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripSplitButton backToolStripSplitButton;
        private System.Windows.Forms.ToolStripButton upOneLevelToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton deleteToolStripButton;
        private System.Windows.Forms.ToolStripButton createNewFolderToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton toolsToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem thumbnailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sideBySideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem propertyToolStripMenuItem;
        internal System.Windows.Forms.ComboBox fileTypeComboBox;
        private System.Windows.Forms.ToolStrip placesBar;
        private System.Windows.Forms.ToolStripMenuItem connectNetworkDriveToolStripMenuItem;
        internal System.Windows.Forms.ComboBox fileNameComboBox;
        internal FileDialogs.SplitButton okButton;
        internal System.Windows.Forms.Button cancelButton;
        internal System.Windows.Forms.Label lookInLabel;
        internal System.Windows.Forms.Label fileNameLabel;
        internal System.Windows.Forms.Label fileTypeLabel;
        internal System.Windows.Forms.ToolStripButton searchTheWebToolStripButton;
        internal System.Windows.Forms.ToolStripSplitButton viewsToolStripSplitButton;

    }
}