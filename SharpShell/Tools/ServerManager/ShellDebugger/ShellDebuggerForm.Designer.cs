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
            this.shellTreeView = new ServerManager.ShellDebugger.ShellTreeView();
            this.splitContainerTreeAndDetails = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeAndDetails)).BeginInit();
            this.splitContainerTreeAndDetails.Panel1.SuspendLayout();
            this.splitContainerTreeAndDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // shellTreeView
            // 
            this.shellTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shellTreeView.Location = new System.Drawing.Point(0, 0);
            this.shellTreeView.Name = "shellTreeView";
            this.shellTreeView.ShowFiles = false;
            this.shellTreeView.ShowHiddenFilesAndFolders = false;
            this.shellTreeView.Size = new System.Drawing.Size(194, 441);
            this.shellTreeView.TabIndex = 0;
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
            this.splitContainerTreeAndDetails.Size = new System.Drawing.Size(584, 441);
            this.splitContainerTreeAndDetails.SplitterDistance = 194;
            this.splitContainerTreeAndDetails.TabIndex = 2;
            // 
            // ShellDebuggerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.splitContainerTreeAndDetails);
            this.Name = "ShellDebuggerForm";
            this.Text = "ShellDebuggerForm";
            this.splitContainerTreeAndDetails.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerTreeAndDetails)).EndInit();
            this.splitContainerTreeAndDetails.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShellTreeView shellTreeView;
        private System.Windows.Forms.SplitContainer splitContainerTreeAndDetails;

    }
}