namespace ServerManager.TestShell
{
    partial class TestShellForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestShellForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelAttributes = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainerViewAndProperties = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.shellTreeView = new Apex.WinForms.Controls.ShellTreeView();
            this.shellListView = new Apex.WinForms.Controls.ShellListView();
            this.tabControlDetails = new System.Windows.Forms.TabControl();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.propertyGridSelectedObject = new System.Windows.Forms.PropertyGrid();
            this.tabPagePreview = new System.Windows.Forms.TabPage();
            this.shellPreviewHost1 = new ServerManager.TestShell.ShellPreviewHost();
            this.tabPageThumbnail = new System.Windows.Forms.TabPage();
            this.shellThumbnailHost1 = new ServerManager.TestShell.ShellThumbnailHost();
            this.tabPageOverlays = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBoxIconOverlay = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButtonChangeYourView = new System.Windows.Forms.ToolStripSplitButton();
            this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSmallIcons = new System.Windows.Forms.ToolStripMenuItem();
            this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowProperties = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShellOpenDialog = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerViewAndProperties)).BeginInit();
            this.splitContainerViewAndProperties.Panel1.SuspendLayout();
            this.splitContainerViewAndProperties.Panel2.SuspendLayout();
            this.splitContainerViewAndProperties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlDetails.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            this.tabPagePreview.SuspendLayout();
            this.tabPageThumbnail.SuspendLayout();
            this.tabPageOverlays.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIconOverlay)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainerViewAndProperties);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(784, 515);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(784, 562);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelAttributes});
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 0;
            // 
            // toolStripStatusLabelAttributes
            // 
            this.toolStripStatusLabelAttributes.Name = "toolStripStatusLabelAttributes";
            this.toolStripStatusLabelAttributes.Size = new System.Drawing.Size(65, 17);
            this.toolStripStatusLabelAttributes.Text = "Attributes: ";
            // 
            // splitContainerViewAndProperties
            // 
            this.splitContainerViewAndProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerViewAndProperties.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainerViewAndProperties.Location = new System.Drawing.Point(0, 0);
            this.splitContainerViewAndProperties.Name = "splitContainerViewAndProperties";
            // 
            // splitContainerViewAndProperties.Panel1
            // 
            this.splitContainerViewAndProperties.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainerViewAndProperties.Panel2
            // 
            this.splitContainerViewAndProperties.Panel2.Controls.Add(this.tabControlDetails);
            this.splitContainerViewAndProperties.Size = new System.Drawing.Size(784, 515);
            this.splitContainerViewAndProperties.SplitterDistance = 550;
            this.splitContainerViewAndProperties.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.shellTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.shellListView);
            this.splitContainer1.Size = new System.Drawing.Size(550, 515);
            this.splitContainer1.SplitterDistance = 183;
            this.splitContainer1.TabIndex = 0;
            // 
            // shellTreeView
            // 
            this.shellTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellTreeView.HideSelection = false;
            this.shellTreeView.Location = new System.Drawing.Point(0, 0);
            this.shellTreeView.Name = "shellTreeView";
            this.shellTreeView.ShowFiles = false;
            this.shellTreeView.ShowHiddenFilesAndFolders = false;
            this.shellTreeView.Size = new System.Drawing.Size(183, 515);
            this.shellTreeView.TabIndex = 0;
            this.shellTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.shellTreeView_AfterSelect);
            this.shellTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.shellTreeView1_MouseUp);
            // 
            // shellListView
            // 
            this.shellListView.AssociationTreeView = this.shellTreeView;
            this.shellListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellListView.Location = new System.Drawing.Point(0, 0);
            this.shellListView.Name = "shellListView";
            this.shellListView.ShowFolders = true;
            this.shellListView.ShowHiddenFilesAndFolders = false;
            this.shellListView.ShowItemToolTips = true;
            this.shellListView.Size = new System.Drawing.Size(363, 515);
            this.shellListView.TabIndex = 0;
            this.shellListView.UseCompatibleStateImageBehavior = false;
            this.shellListView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.shellListView_ItemDrag);
            this.shellListView.SelectedIndexChanged += new System.EventHandler(this.shellListView_SelectedIndexChanged);
            this.shellListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.shellListView_DragDrop);
            this.shellListView.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.shellListView_GiveFeedback);
            this.shellListView.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.shellListView_QueryContinueDrag);
            this.shellListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.shellListView1_MouseUp);
            // 
            // tabControlDetails
            // 
            this.tabControlDetails.Controls.Add(this.tabPageProperties);
            this.tabControlDetails.Controls.Add(this.tabPagePreview);
            this.tabControlDetails.Controls.Add(this.tabPageThumbnail);
            this.tabControlDetails.Controls.Add(this.tabPageOverlays);
            this.tabControlDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlDetails.Location = new System.Drawing.Point(0, 0);
            this.tabControlDetails.Name = "tabControlDetails";
            this.tabControlDetails.SelectedIndex = 0;
            this.tabControlDetails.Size = new System.Drawing.Size(230, 515);
            this.tabControlDetails.TabIndex = 0;
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Controls.Add(this.propertyGridSelectedObject);
            this.tabPageProperties.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Size = new System.Drawing.Size(222, 489);
            this.tabPageProperties.TabIndex = 0;
            this.tabPageProperties.Text = "Properties";
            this.tabPageProperties.UseVisualStyleBackColor = true;
            // 
            // propertyGridSelectedObject
            // 
            this.propertyGridSelectedObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridSelectedObject.Location = new System.Drawing.Point(0, 0);
            this.propertyGridSelectedObject.Name = "propertyGridSelectedObject";
            this.propertyGridSelectedObject.Size = new System.Drawing.Size(222, 489);
            this.propertyGridSelectedObject.TabIndex = 0;
            // 
            // tabPagePreview
            // 
            this.tabPagePreview.Controls.Add(this.shellPreviewHost1);
            this.tabPagePreview.Location = new System.Drawing.Point(4, 22);
            this.tabPagePreview.Name = "tabPagePreview";
            this.tabPagePreview.Size = new System.Drawing.Size(222, 489);
            this.tabPagePreview.TabIndex = 1;
            this.tabPagePreview.Text = "Preview";
            this.tabPagePreview.UseVisualStyleBackColor = true;
            // 
            // shellPreviewHost1
            // 
            this.shellPreviewHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellPreviewHost1.Location = new System.Drawing.Point(0, 0);
            this.shellPreviewHost1.Name = "shellPreviewHost1";
            this.shellPreviewHost1.Size = new System.Drawing.Size(222, 489);
            this.shellPreviewHost1.TabIndex = 0;
            // 
            // tabPageThumbnail
            // 
            this.tabPageThumbnail.Controls.Add(this.shellThumbnailHost1);
            this.tabPageThumbnail.Location = new System.Drawing.Point(4, 22);
            this.tabPageThumbnail.Name = "tabPageThumbnail";
            this.tabPageThumbnail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageThumbnail.Size = new System.Drawing.Size(222, 489);
            this.tabPageThumbnail.TabIndex = 2;
            this.tabPageThumbnail.Text = "Thumbnail";
            this.tabPageThumbnail.UseVisualStyleBackColor = true;
            // 
            // shellThumbnailHost1
            // 
            this.shellThumbnailHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellThumbnailHost1.Location = new System.Drawing.Point(3, 3);
            this.shellThumbnailHost1.Name = "shellThumbnailHost1";
            this.shellThumbnailHost1.Size = new System.Drawing.Size(216, 483);
            this.shellThumbnailHost1.TabIndex = 0;
            // 
            // tabPageOverlays
            // 
            this.tabPageOverlays.Controls.Add(this.label2);
            this.tabPageOverlays.Controls.Add(this.pictureBoxIconOverlay);
            this.tabPageOverlays.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverlays.Name = "tabPageOverlays";
            this.tabPageOverlays.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverlays.Size = new System.Drawing.Size(222, 489);
            this.tabPageOverlays.TabIndex = 3;
            this.tabPageOverlays.Text = "Overlays";
            this.tabPageOverlays.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Overlay Icon";
            // 
            // pictureBoxIconOverlay
            // 
            this.pictureBoxIconOverlay.Location = new System.Drawing.Point(16, 31);
            this.pictureBoxIconOverlay.Name = "pictureBoxIconOverlay";
            this.pictureBoxIconOverlay.Size = new System.Drawing.Size(29, 32);
            this.pictureBoxIconOverlay.TabIndex = 2;
            this.pictureBoxIconOverlay.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonChangeYourView,
            this.toolStripSeparator1,
            this.toolStripButtonShowProperties,
            this.toolStripButtonShellOpenDialog});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(96, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripSplitButtonChangeYourView
            // 
            this.toolStripSplitButtonChangeYourView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButtonChangeYourView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.largeIconsToolStripMenuItem,
            this.toolStripMenuItemSmallIcons,
            this.listToolStripMenuItem,
            this.detailsToolStripMenuItem,
            this.tileToolStripMenuItem});
            this.toolStripSplitButtonChangeYourView.Image = global::ServerManager.Properties.Resources.View_LargeIcons;
            this.toolStripSplitButtonChangeYourView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonChangeYourView.Name = "toolStripSplitButtonChangeYourView";
            this.toolStripSplitButtonChangeYourView.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButtonChangeYourView.Text = "Change Your View";
            this.toolStripSplitButtonChangeYourView.ToolTipText = "Change Your View";
            this.toolStripSplitButtonChangeYourView.ButtonClick += new System.EventHandler(this.toolStripSplitButtonChangeYourView_ButtonClick);
            // 
            // largeIconsToolStripMenuItem
            // 
            this.largeIconsToolStripMenuItem.Image = global::ServerManager.Properties.Resources.View_LargeIcons;
            this.largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
            this.largeIconsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.largeIconsToolStripMenuItem.Text = "Large Icons";
            this.largeIconsToolStripMenuItem.Click += new System.EventHandler(this.largeIconsToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSmallIcons
            // 
            this.toolStripMenuItemSmallIcons.Image = global::ServerManager.Properties.Resources.View_SmallIcons;
            this.toolStripMenuItemSmallIcons.Name = "toolStripMenuItemSmallIcons";
            this.toolStripMenuItemSmallIcons.Size = new System.Drawing.Size(134, 22);
            this.toolStripMenuItemSmallIcons.Text = "Small Icons";
            this.toolStripMenuItemSmallIcons.ToolTipText = "Small Icons";
            this.toolStripMenuItemSmallIcons.Click += new System.EventHandler(this.toolStripMenuItemSmallIcons_Click);
            // 
            // listToolStripMenuItem
            // 
            this.listToolStripMenuItem.Image = global::ServerManager.Properties.Resources.View_List;
            this.listToolStripMenuItem.Name = "listToolStripMenuItem";
            this.listToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.listToolStripMenuItem.Text = "List";
            this.listToolStripMenuItem.Click += new System.EventHandler(this.listToolStripMenuItem_Click);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Image = global::ServerManager.Properties.Resources.View_Details;
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.detailsToolStripMenuItem_Click);
            // 
            // tileToolStripMenuItem
            // 
            this.tileToolStripMenuItem.Image = global::ServerManager.Properties.Resources.View_Tiles;
            this.tileToolStripMenuItem.Name = "tileToolStripMenuItem";
            this.tileToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.tileToolStripMenuItem.Text = "Tile";
            this.tileToolStripMenuItem.Click += new System.EventHandler(this.tileToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonShowProperties
            // 
            this.toolStripButtonShowProperties.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShowProperties.Image = global::ServerManager.Properties.Resources.PropertySheet;
            this.toolStripButtonShowProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowProperties.Name = "toolStripButtonShowProperties";
            this.toolStripButtonShowProperties.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShowProperties.Text = "Show Properties";
            this.toolStripButtonShowProperties.Click += new System.EventHandler(this.toolStripButtonShowProperties_Click);
            // 
            // toolStripButtonShellOpenDialog
            // 
            this.toolStripButtonShellOpenDialog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonShellOpenDialog.Image = global::ServerManager.Properties.Resources.openHS;
            this.toolStripButtonShellOpenDialog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShellOpenDialog.Name = "toolStripButtonShellOpenDialog";
            this.toolStripButtonShellOpenDialog.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonShellOpenDialog.Text = "Shell Open Dialog";
            this.toolStripButtonShellOpenDialog.Click += new System.EventHandler(this.toolStripButtonShellOpenDialog_Click);
            // 
            // TestShellForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestShellForm";
            this.Text = "Test Shell";
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainerViewAndProperties.Panel1.ResumeLayout(false);
            this.splitContainerViewAndProperties.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerViewAndProperties)).EndInit();
            this.splitContainerViewAndProperties.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlDetails.ResumeLayout(false);
            this.tabPageProperties.ResumeLayout(false);
            this.tabPagePreview.ResumeLayout(false);
            this.tabPageThumbnail.ResumeLayout(false);
            this.tabPageOverlays.ResumeLayout(false);
            this.tabPageOverlays.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIconOverlay)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Apex.WinForms.Controls.ShellTreeView shellTreeView;
        private Apex.WinForms.Controls.ShellListView shellListView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAttributes;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonChangeYourView;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSmallIcons;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonShellOpenDialog;
        private System.Windows.Forms.SplitContainer splitContainerViewAndProperties;
        private System.Windows.Forms.TabControl tabControlDetails;
        private System.Windows.Forms.TabPage tabPageProperties;
        private System.Windows.Forms.TabPage tabPagePreview;
        private System.Windows.Forms.PropertyGrid propertyGridSelectedObject;
        private ShellPreviewHost shellPreviewHost1;
        private System.Windows.Forms.TabPage tabPageThumbnail;
        private ShellThumbnailHost shellThumbnailHost1;
        private System.Windows.Forms.TabPage tabPageOverlays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBoxIconOverlay;
    }
}