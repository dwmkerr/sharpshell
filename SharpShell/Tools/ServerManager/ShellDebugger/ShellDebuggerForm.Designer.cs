namespace ServerManager.ShellDebugger
{
    partial class ShellDebuggerForm
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
            this.splitContainerTreeAndDetails = new System.Windows.Forms.SplitContainer();
            this.shellTreeView = new ServerManager.ShellDebugger.ShellTreeView();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStripViewMode = new System.Windows.Forms.ToolStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeAndDetails)).BeginInit();
            this.splitContainerTreeAndDetails.Panel1.SuspendLayout();
            this.splitContainerTreeAndDetails.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerTreeAndDetails
            // 
            this.splitContainerTreeAndDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerTreeAndDetails.Location = new System.Drawing.Point(0, 0);
            this.splitContainerTreeAndDetails.Name = "splitContainerTreeAndDetails";
            // 
            // splitContainerTreeAndDetails.Panel1
            // 
            this.splitContainerTreeAndDetails.Panel1.Controls.Add(this.shellTreeView);
            this.splitContainerTreeAndDetails.Size = new System.Drawing.Size(584, 416);
            this.splitContainerTreeAndDetails.SplitterDistance = 194;
            this.splitContainerTreeAndDetails.TabIndex = 2;
            // 
            // shellTreeView
            // 
            this.shellTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellTreeView.Location = new System.Drawing.Point(0, 0);
            this.shellTreeView.Name = "shellTreeView";
            this.shellTreeView.ShowFiles = false;
            this.shellTreeView.ShowHiddenFilesAndFolders = false;
            this.shellTreeView.Size = new System.Drawing.Size(194, 416);
            this.shellTreeView.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainerTreeAndDetails);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(584, 416);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(584, 441);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStripViewMode);
            // 
            // toolStripViewMode
            // 
            this.toolStripViewMode.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripViewMode.Location = new System.Drawing.Point(3, 0);
            this.toolStripViewMode.Name = "toolStripViewMode";
            this.toolStripViewMode.Size = new System.Drawing.Size(43, 25);
            this.toolStripViewMode.TabIndex = 0;
            // 
            // ShellDebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "ShellDebuggerForm";
            this.Text = "Shell Debugger";
            this.Load += new System.EventHandler(this.ShellDebuggerForm_Load);
            this.splitContainerTreeAndDetails.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeAndDetails)).EndInit();
            this.splitContainerTreeAndDetails.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShellTreeView shellTreeView;
        private System.Windows.Forms.SplitContainer splitContainerTreeAndDetails;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStripViewMode;

    }
}