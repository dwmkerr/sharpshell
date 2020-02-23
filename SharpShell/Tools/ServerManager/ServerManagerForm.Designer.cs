using ServerManager.Views;

namespace ServerManager
{
    partial class ServerManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerManagerForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelOSProcessor = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelProcessProcessor = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.listViewServers = new System.Windows.Forms.ListView();
            this.columnHeaderServerName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderServerType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.serverDetailsView = new ServerManager.Views.ServerDetailsView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.installServerx86ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerServerx86ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unregisterServerx86ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallServerx86ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.installServerx64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerServerx64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unregisterServerx64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uninstallServerx64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.testServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.explorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alwaysUnloadDLLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.desktopProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearMostRecentlyUsedServersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sharpShellProjectHomePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.requestAFeatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonOpenTestShell = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTestServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonOpenShellDialog = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonAttachDebugger = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShellDebugger = new System.Windows.Forms.ToolStripButton();
            this.panelPleaseWait = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.panelPleaseWait.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1084, 574);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1084, 661);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabelOSProcessor,
            this.toolStripStatusLabelProcessProcessor});
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1084, 24);
            this.statusStrip.TabIndex = 0;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(307, 19);
            this.toolStripStatusLabel1.Text = "Drop SharpShell Server DLLs onto the list to analyse them";
            // 
            // toolStripStatusLabelOSProcessor
            // 
            this.toolStripStatusLabelOSProcessor.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabelOSProcessor.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabelOSProcessor.Name = "toolStripStatusLabelOSProcessor";
            this.toolStripStatusLabelOSProcessor.Size = new System.Drawing.Size(83, 19);
            this.toolStripStatusLabelOSProcessor.Text = "Windows: x64";
            // 
            // toolStripStatusLabelProcessProcessor
            // 
            this.toolStripStatusLabelProcessProcessor.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabelProcessProcessor.BorderStyle = System.Windows.Forms.Border3DStyle.Etched;
            this.toolStripStatusLabelProcessProcessor.Name = "toolStripStatusLabelProcessProcessor";
            this.toolStripStatusLabelProcessProcessor.Size = new System.Drawing.Size(74, 19);
            this.toolStripStatusLabelProcessProcessor.Text = "Process: x86";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.listViewServers);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.serverDetailsView);
            this.splitContainer.Size = new System.Drawing.Size(1084, 574);
            this.splitContainer.SplitterDistance = 686;
            this.splitContainer.TabIndex = 1;
            // 
            // listViewServers
            // 
            this.listViewServers.AllowDrop = true;
            this.listViewServers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderServerName,
            this.columnHeaderServerType,
            this.columnHeader});
            this.listViewServers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewServers.FullRowSelect = true;
            this.listViewServers.Location = new System.Drawing.Point(0, 0);
            this.listViewServers.Name = "listViewServers";
            this.listViewServers.Size = new System.Drawing.Size(686, 574);
            this.listViewServers.SmallImageList = this.imageList1;
            this.listViewServers.TabIndex = 0;
            this.listViewServers.UseCompatibleStateImageBehavior = false;
            this.listViewServers.View = System.Windows.Forms.View.Details;
            this.listViewServers.SelectedIndexChanged += new System.EventHandler(this.listViewServers_SelectedIndexChanged);
            this.listViewServers.DragDrop += new System.Windows.Forms.DragEventHandler(this.listViewServers_DragDrop);
            this.listViewServers.DragEnter += new System.Windows.Forms.DragEventHandler(this.listViewServers_DragEnter);
            this.listViewServers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewServers_KeyDown);
            // 
            // columnHeaderServerName
            // 
            this.columnHeaderServerName.Text = "Server Name";
            this.columnHeaderServerName.Width = 230;
            // 
            // columnHeaderServerType
            // 
            this.columnHeaderServerType.Text = "Type";
            this.columnHeaderServerType.Width = 230;
            // 
            // columnHeader
            // 
            this.columnHeader.Text = "CLSID";
            this.columnHeader.Width = 230;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ContextMenu.png");
            this.imageList1.Images.SetKeyName(1, "Icon.png");
            this.imageList1.Images.SetKeyName(2, "PropertySheet.png");
            this.imageList1.Images.SetKeyName(3, "InfoTip.png");
            this.imageList1.Images.SetKeyName(4, "IconOverlayHandler.png");
            // 
            // serverDetailsView
            // 
            this.serverDetailsView.AutoScroll = true;
            this.serverDetailsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverDetailsView.ExtensionEntry = null;
            this.serverDetailsView.Location = new System.Drawing.Point(0, 0);
            this.serverDetailsView.Margin = new System.Windows.Forms.Padding(6);
            this.serverDetailsView.Name = "serverDetailsView";
            this.serverDetailsView.Size = new System.Drawing.Size(394, 574);
            this.serverDetailsView.TabIndex = 1;
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.serverToolStripMenuItem,
            this.explorerToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1084, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadServerToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // loadServerToolStripMenuItem
            // 
            this.loadServerToolStripMenuItem.Name = "loadServerToolStripMenuItem";
            this.loadServerToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.loadServerToolStripMenuItem.Text = "&Load Server...";
            this.loadServerToolStripMenuItem.Click += new System.EventHandler(this.loadServerToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(141, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // serverToolStripMenuItem
            // 
            this.serverToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.installToolStripMenuItem,
            this.uninstallToolStripMenuItem,
            this.toolStripSeparator7,
            this.installServerx86ToolStripMenuItem,
            this.registerServerx86ToolStripMenuItem,
            this.unregisterServerx86ToolStripMenuItem,
            this.uninstallServerx86ToolStripMenuItem,
            this.toolStripSeparator3,
            this.installServerx64ToolStripMenuItem,
            this.registerServerx64ToolStripMenuItem,
            this.unregisterServerx64ToolStripMenuItem,
            this.uninstallServerx64ToolStripMenuItem,
            this.toolStripSeparator1,
            this.testServerToolStripMenuItem});
            this.serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            this.serverToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
            this.serverToolStripMenuItem.Text = "&Server";
            // 
            // installToolStripMenuItem
            // 
            this.installToolStripMenuItem.Name = "installToolStripMenuItem";
            this.installToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.installToolStripMenuItem.Text = "&Install Assembly";
            this.installToolStripMenuItem.Click += new System.EventHandler(this.installToolStripMenuItem_Click);
            // 
            // uninstallToolStripMenuItem
            // 
            this.uninstallToolStripMenuItem.Name = "uninstallToolStripMenuItem";
            this.uninstallToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.uninstallToolStripMenuItem.Text = "&Uninstall Assembly";
            this.uninstallToolStripMenuItem.Click += new System.EventHandler(this.uninstallToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(188, 6);
            // 
            // installServerx86ToolStripMenuItem
            // 
            this.installServerx86ToolStripMenuItem.Name = "installServerx86ToolStripMenuItem";
            this.installServerx86ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.installServerx86ToolStripMenuItem.Text = "Install Server (x86)";
            this.installServerx86ToolStripMenuItem.Click += new System.EventHandler(this.installServerX86ToolStripMenuItem_Click);
            // 
            // registerServerx86ToolStripMenuItem
            // 
            this.registerServerx86ToolStripMenuItem.Name = "registerServerx86ToolStripMenuItem";
            this.registerServerx86ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.registerServerx86ToolStripMenuItem.Text = "Register Server (x86)";
            this.registerServerx86ToolStripMenuItem.Click += new System.EventHandler(this.registerServerX86ToolStripMenuItem_Click);
            // 
            // unregisterServerx86ToolStripMenuItem
            // 
            this.unregisterServerx86ToolStripMenuItem.Name = "unregisterServerx86ToolStripMenuItem";
            this.unregisterServerx86ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.unregisterServerx86ToolStripMenuItem.Text = "Unregister Server (x86)";
            this.unregisterServerx86ToolStripMenuItem.Click += new System.EventHandler(this.unregisterServerX86ToolStripMenuItem_Click);
            // 
            // uninstallServerx86ToolStripMenuItem
            // 
            this.uninstallServerx86ToolStripMenuItem.Name = "uninstallServerx86ToolStripMenuItem";
            this.uninstallServerx86ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.uninstallServerx86ToolStripMenuItem.Text = "Uninstall Server (x86)";
            this.uninstallServerx86ToolStripMenuItem.Click += new System.EventHandler(this.uninstallServerX86ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // installServerx64ToolStripMenuItem
            // 
            this.installServerx64ToolStripMenuItem.Name = "installServerx64ToolStripMenuItem";
            this.installServerx64ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.installServerx64ToolStripMenuItem.Text = "Install Server (x64)";
            this.installServerx64ToolStripMenuItem.Click += new System.EventHandler(this.installServerX64ToolStripMenuItem_Click);
            // 
            // registerServerx64ToolStripMenuItem
            // 
            this.registerServerx64ToolStripMenuItem.Name = "registerServerx64ToolStripMenuItem";
            this.registerServerx64ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.registerServerx64ToolStripMenuItem.Text = "Register Server (x64)";
            this.registerServerx64ToolStripMenuItem.Click += new System.EventHandler(this.registerServerX64ToolStripMenuItem_Click);
            // 
            // unregisterServerx64ToolStripMenuItem
            // 
            this.unregisterServerx64ToolStripMenuItem.Name = "unregisterServerx64ToolStripMenuItem";
            this.unregisterServerx64ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.unregisterServerx64ToolStripMenuItem.Text = "Unregister Server (x64)";
            this.unregisterServerx64ToolStripMenuItem.Click += new System.EventHandler(this.unregisterServerX64ToolStripMenuItem_Click);
            // 
            // uninstallServerx64ToolStripMenuItem
            // 
            this.uninstallServerx64ToolStripMenuItem.Name = "uninstallServerx64ToolStripMenuItem";
            this.uninstallServerx64ToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.uninstallServerx64ToolStripMenuItem.Text = "Unin&stall Server (x64)";
            this.uninstallServerx64ToolStripMenuItem.Click += new System.EventHandler(this.uninstallServerX64ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(188, 6);
            this.toolStripSeparator1.Visible = false;
            // 
            // testServerToolStripMenuItem
            // 
            this.testServerToolStripMenuItem.Name = "testServerToolStripMenuItem";
            this.testServerToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.testServerToolStripMenuItem.Text = "&Test Server...";
            this.testServerToolStripMenuItem.Visible = false;
            this.testServerToolStripMenuItem.Click += new System.EventHandler(this.testServerToolStripMenuItem_Click);
            // 
            // explorerToolStripMenuItem
            // 
            this.explorerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.alwaysUnloadDLLToolStripMenuItem,
            this.desktopProcessToolStripMenuItem,
            this.restartExplorerToolStripMenuItem});
            this.explorerToolStripMenuItem.Name = "explorerToolStripMenuItem";
            this.explorerToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.explorerToolStripMenuItem.Text = "&Explorer";
            // 
            // alwaysUnloadDLLToolStripMenuItem
            // 
            this.alwaysUnloadDLLToolStripMenuItem.CheckOnClick = true;
            this.alwaysUnloadDLLToolStripMenuItem.Name = "alwaysUnloadDLLToolStripMenuItem";
            this.alwaysUnloadDLLToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.alwaysUnloadDLLToolStripMenuItem.Text = "&Always Unload DLL";
            this.alwaysUnloadDLLToolStripMenuItem.Click += new System.EventHandler(this.alwaysUnloadDLLToolStripMenuItem_Click);
            // 
            // desktopProcessToolStripMenuItem
            // 
            this.desktopProcessToolStripMenuItem.CheckOnClick = true;
            this.desktopProcessToolStripMenuItem.Name = "desktopProcessToolStripMenuItem";
            this.desktopProcessToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.desktopProcessToolStripMenuItem.Text = "&Desktop Process";
            this.desktopProcessToolStripMenuItem.Click += new System.EventHandler(this.desktopProcessToolStripMenuItem_Click);
            // 
            // restartExplorerToolStripMenuItem
            // 
            this.restartExplorerToolStripMenuItem.Name = "restartExplorerToolStripMenuItem";
            this.restartExplorerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.restartExplorerToolStripMenuItem.Text = "&Restart Explorer";
            this.restartExplorerToolStripMenuItem.Click += new System.EventHandler(this.restartExplorerToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearMostRecentlyUsedServersToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // clearMostRecentlyUsedServersToolStripMenuItem
            // 
            this.clearMostRecentlyUsedServersToolStripMenuItem.Name = "clearMostRecentlyUsedServersToolStripMenuItem";
            this.clearMostRecentlyUsedServersToolStripMenuItem.Size = new System.Drawing.Size(243, 22);
            this.clearMostRecentlyUsedServersToolStripMenuItem.Text = "&Clear most recently used servers";
            this.clearMostRecentlyUsedServersToolStripMenuItem.Click += new System.EventHandler(this.clearMostRecentlyUsedServersToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sharpShellProjectHomePageToolStripMenuItem,
            this.toolStripSeparator4,
            this.reportABugToolStripMenuItem,
            this.requestAFeatureToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // sharpShellProjectHomePageToolStripMenuItem
            // 
            this.sharpShellProjectHomePageToolStripMenuItem.Name = "sharpShellProjectHomePageToolStripMenuItem";
            this.sharpShellProjectHomePageToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.sharpShellProjectHomePageToolStripMenuItem.Text = "&SharpShell Project Home Page";
            this.sharpShellProjectHomePageToolStripMenuItem.Click += new System.EventHandler(this.sharpShellProjectHomePageToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(231, 6);
            // 
            // reportABugToolStripMenuItem
            // 
            this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
            this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.reportABugToolStripMenuItem.Text = "&Report a Bug";
            this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuItem_Click);
            // 
            // requestAFeatureToolStripMenuItem
            // 
            this.requestAFeatureToolStripMenuItem.Name = "requestAFeatureToolStripMenuItem";
            this.requestAFeatureToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.requestAFeatureToolStripMenuItem.Text = "Request a &Feature";
            this.requestAFeatureToolStripMenuItem.Click += new System.EventHandler(this.requestAFeatureToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(231, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
            this.aboutToolStripMenuItem.Text = "&About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpenTestShell,
            this.toolStripButtonTestServer,
            this.toolStripButtonOpenShellDialog,
            this.toolStripSeparator6,
            this.toolStripButtonAttachDebugger,
            this.toolStripButtonShellDebugger});
            this.toolStrip.Location = new System.Drawing.Point(3, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(616, 39);
            this.toolStrip.TabIndex = 3;
            // 
            // toolStripButtonOpenTestShell
            // 
            this.toolStripButtonOpenTestShell.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenTestShell.Name = "toolStripButtonOpenTestShell";
            this.toolStripButtonOpenTestShell.Size = new System.Drawing.Size(92, 36);
            this.toolStripButtonOpenTestShell.Text = "Open Test Shell";
            this.toolStripButtonOpenTestShell.Click += new System.EventHandler(this.toolStripButtonOpenTestShell_Click);
            // 
            // toolStripButtonTestServer
            // 
            this.toolStripButtonTestServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTestServer.Name = "toolStripButtonTestServer";
            this.toolStripButtonTestServer.Size = new System.Drawing.Size(132, 36);
            this.toolStripButtonTestServer.Text = "Test Server in Test Shell";
            this.toolStripButtonTestServer.Visible = false;
            this.toolStripButtonTestServer.Click += new System.EventHandler(this.toolStripButtonTestServer_Click);
            // 
            // toolStripButtonOpenShellDialog
            // 
            this.toolStripButtonOpenShellDialog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonOpenShellDialog.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpenShellDialog.Image")));
            this.toolStripButtonOpenShellDialog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonOpenShellDialog.Name = "toolStripButtonOpenShellDialog";
            this.toolStripButtonOpenShellDialog.Size = new System.Drawing.Size(105, 36);
            this.toolStripButtonOpenShellDialog.Text = "Open Shell Dialog";
            this.toolStripButtonOpenShellDialog.Click += new System.EventHandler(this.toolStripButtonOpenShellDialog_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 39);
            // 
            // toolStripButtonAttachDebugger
            // 
            this.toolStripButtonAttachDebugger.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAttachDebugger.Image = global::ServerManager.Properties.Resources.process_16xLG;
            this.toolStripButtonAttachDebugger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAttachDebugger.Name = "toolStripButtonAttachDebugger";
            this.toolStripButtonAttachDebugger.Size = new System.Drawing.Size(36, 36);
            this.toolStripButtonAttachDebugger.Text = "Attach Debugger";
            this.toolStripButtonAttachDebugger.Click += new System.EventHandler(this.toolStripButtonAttachDebugger_Click);
            // 
            // toolStripButtonShellDebugger
            // 
            this.toolStripButtonShellDebugger.Image = global::ServerManager.Properties.Resources.Debug;
            this.toolStripButtonShellDebugger.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShellDebugger.Name = "toolStripButtonShellDebugger";
            this.toolStripButtonShellDebugger.Size = new System.Drawing.Size(202, 36);
            this.toolStripButtonShellDebugger.Text = "Shell Debugger (Experimental)";
            this.toolStripButtonShellDebugger.Click += new System.EventHandler(this.toolStripButtonShellDebugger_Click);
            // 
            // panelPleaseWait
            // 
            this.panelPleaseWait.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelPleaseWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPleaseWait.Controls.Add(this.label1);
            this.panelPleaseWait.Controls.Add(this.progressBar1);
            this.panelPleaseWait.Location = new System.Drawing.Point(400, 300);
            this.panelPleaseWait.Name = "panelPleaseWait";
            this.panelPleaseWait.Size = new System.Drawing.Size(300, 50);
            this.panelPleaseWait.TabIndex = 1;
            this.panelPleaseWait.Visible = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Please wait ...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 23);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(292, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Value = 33;
            // 
            // ServerManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1084, 661);
            this.Controls.Add(this.panelPleaseWait);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "ServerManagerForm";
            this.Text = "Server Manager";
            this.Shown += new System.EventHandler(this.ServerManagerForm_Shown);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelPleaseWait.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ListView listViewServers;
        private System.Windows.Forms.ColumnHeader columnHeaderServerName;
        private System.Windows.Forms.ColumnHeader columnHeader;
        private System.Windows.Forms.ColumnHeader columnHeaderServerType;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alwaysUnloadDLLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem desktopProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installServerx86ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallServerx86ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem loadServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SplitContainer splitContainer;
        private ServerDetailsView serverDetailsView;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearMostRecentlyUsedServersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installServerx64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallServerx64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem registerServerx86ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerServerx64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unregisterServerx86ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unregisterServerx64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restartExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testServerToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonTestServer;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sharpShellProjectHomePageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem requestAFeatureToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenTestShell;
        private System.Windows.Forms.ToolStripButton toolStripButtonOpenShellDialog;
        private System.Windows.Forms.ToolStripButton toolStripButtonShellDebugger;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelOSProcessor;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProcessProcessor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButtonAttachDebugger;
        private System.Windows.Forms.ToolStripMenuItem installToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uninstallToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.Panel panelPleaseWait;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

