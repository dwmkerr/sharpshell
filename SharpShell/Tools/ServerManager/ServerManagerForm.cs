using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ServerManager.Properties;
using ServerManager.ShellDebugger;
using ServerManager.TestShell;
using ServerManager.Views;
using SharpShell;
using SharpShell.Diagnostics;
using SharpShell.ServerRegistration;

namespace ServerManager
{
    /// <summary>
    ///     The main class
    /// </summary>
    internal partial class ServerManagerForm : Form
    {
        /// <summary>
        ///     The explorer configuration manager.
        /// </summary>
        private readonly ExplorerConfigurationManager
            _explorerConfigurationManager = new ExplorerConfigurationManager();

        public ServerManagerForm()
        {
            InitializeComponent();

            //  Setup the statusbar.
            toolStripStatusLabelOSProcessor.Text =
                Environment.Is64BitOperatingSystem ? "Windows (x64)" : "Windows (x86)";
            toolStripStatusLabelProcessProcessor.Text = Environment.Is64BitProcess ? "Process (x64)" : "Process (x86)";

            //  Set the settings.
            desktopProcessToolStripMenuItem.Checked = _explorerConfigurationManager.DesktopProcess;
            alwaysUnloadDLLToolStripMenuItem.Checked = _explorerConfigurationManager.AlwaysUnloadDll;

            //  Set initial UI command state.
            UpdateUserInterfaceCommands();
        }

        #region Control Event Handlers

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog(this);
        }

        private void alwaysUnloadDLLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _explorerConfigurationManager.AlwaysUnloadDll = alwaysUnloadDLLToolStripMenuItem.Checked;
        }

        private void CheckIfRegisterOrUnregisterRequiresExplorerRestart(params SharpShellServerInfo[] servers)
        {
            if (servers?.Any(server => server?.ServerType == ServerType.ShellIconOverlayHandler) == true)
            {
                if (MessageBox.Show(this,
                        @"This change will not take effect until Windows Explorer is restarted. Would you " +
                        @"like to restart Windows Explorer now?", @"Restart Explorer?", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) ==
                    DialogResult.Yes)
                {
                    ExplorerManager.RestartExplorer();
                }
            }
        }

        private void clearMostRecentlyUsedServersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearRecentlyUsed();
        }

        private void desktopProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _explorerConfigurationManager.DesktopProcess = desktopProcessToolStripMenuItem.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void installServerX64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Install the server.
            InstallServer(SelectedEntry, RegistrationScope.OS64Bit);
        }

        private void installServerX86ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Install the server.
            InstallServer(SelectedEntry, RegistrationScope.OS32Bit);
        }

        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Bail if we have no server selected.
            if (SelectedEntry?.SharpShellServerInfo?.AssemblyInfo == null)
            {
                return;
            }

            var assembly = SelectedEntry.SharpShellServerInfo.AssemblyInfo;
            var fromGac = string.IsNullOrEmpty(SelectedEntry.SharpShellServerInfo.AssemblyInfo.AssemblyPath);
            var success = InstallAssembly(assembly, !fromGac);

            //  Inform the user of the result.
            if (success)
            {
                MessageBox.Show(@"Installed server successfully.", @"Install Server", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(@"Failed to install, check the SharpShell log for details.", @"Install Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void listViewServers_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] files)
                {
                    foreach (var file in files)
                    {
                        AddServersFromFile(file);
                    }
                }
            }
        }

        private void listViewServers_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = ((string[])e.Data.GetData(DataFormats.FileDrop)).Any()
                    ? DragDropEffects.Copy
                    : DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listViewServers_KeyDown(object sender, KeyEventArgs e)
        {
            if (SelectedEntry == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var selectedServerEntry = SelectedEntry;

                //  If we have a server selected, remove it.
                if (selectedServerEntry?.InstallationInfo32 == null &&
                    selectedServerEntry?.InstallationInfo64 == null &&
                    selectedServerEntry?.RegistrationInfo32 == null &&
                    selectedServerEntry?.RegistrationInfo64 == null)
                {
                    RemoveExtensionEntryFromList(selectedServerEntry);

                    //  Remove from the most recently used files.
                    RemoveRecentlyUsed(selectedServerEntry?.SharpShellServerInfo?.AssemblyInfo?.AssemblyPath);
                }
            }
        }

        private void listViewServers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  Update the user interface commands.
            UpdateUserInterfaceCommands();

            // Update list view
            AddOrUpdateEntry(SelectedEntry);
        }

        private void loadServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Create a file open dialog.
            var fileOpenDialog = new OpenFileDialog
            {
                Filter = @"COM Servers (*.dll)|*.dll|All Files (*.*)|*.*"
            };

            if (fileOpenDialog.ShowDialog(this) == DialogResult.OK)
            {
                //  Try and add  the server.
                AddServersFromFile(fileOpenDialog.FileName);
            }
        }

        private void registerServerX64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Register the server
            RegisterServer(SelectedEntry, RegistrationScope.OS64Bit);
        }

        private void registerServerX86ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Register the server
            RegisterServer(SelectedEntry, RegistrationScope.OS32Bit);
        }

        private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Resources.UrlReportABug);
        }

        private void requestAFeatureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Resources.URlSuggestAFeature);
        }

        private void restartExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExplorerManager.RestartExplorer();
        }


        private void ServerManagerForm_Shown(object sender, EventArgs e)
        {
            PrepareServerList();
        }

        private void sharpShellProjectHomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(Resources.UrlSharpShellProjectHomePage);
        }

        private void testServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Call the test server command.
            DoTestServer();
        }

        private void toolStripButtonAttachDebugger_Click(object sender, EventArgs e)
        {
            Debugger.Launch();
        }

        private void toolStripButtonOpenShellDialog_Click(object sender, EventArgs e)
        {
            //  Show a shell dialog.
            new OpenFileDialog().ShowDialog(this);
        }

        private void toolStripButtonOpenTestShell_Click(object sender, EventArgs e)
        {
            new TestShellForm().ShowDialog(this);
        }

        private void toolStripButtonShellDebugger_Click(object sender, EventArgs e)
        {
            //  Create and show a new shell debugger.
            new ShellDebuggerForm().ShowDialog(this);
        }

        private void toolStripButtonTestServer_Click(object sender, EventArgs e)
        {
            //  Call the test server command.
            DoTestServer();
        }

        private void uninstallServerX64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Uninstall the server.
            UninstallServer(SelectedEntry, RegistrationScope.OS64Bit);
        }

        private void uninstallServerX86ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Uninstall the server.
            UninstallServer(SelectedEntry, RegistrationScope.OS32Bit);
        }


        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Bail if we have no server selected.
            if (SelectedEntry?.SharpShellServerInfo?.AssemblyInfo == null)
            {
                return;
            }

            var assemblyInfo = SelectedEntry.SharpShellServerInfo.AssemblyInfo;

            //  Inform the user of the result.
            if (UninstallAssembly(assemblyInfo))
            {
                MessageBox.Show(@"Uninstalled server successfully.", @"Uninstall Server", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(@"Failed to uninstall, check the SharpShell log for details.", @"Uninstall Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void unregisterServerX64ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Unregister the server
            UnregisterServer(SelectedEntry, RegistrationScope.OS64Bit);
        }

        private void unregisterServerX86ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  Unregister the server
            UnregisterServer(SelectedEntry, RegistrationScope.OS32Bit);
        }

        #endregion

        #region Main Logic

        public IEnumerable<ShellExtensionEntry> Entries
        {
            get
            {
                return listViewServers.Items
                    .OfType<ServerListViewItem>()
                    .Select(item => item.ExtensionEntry);
            }
        }

        public ShellExtensionEntry SelectedEntry
        {
            get => listViewServers.SelectedItems.OfType<ServerListViewItem>().FirstOrDefault()?.ExtensionEntry;
        }

        /// <summary>
        ///     Tests the selected serer.
        /// </summary>
        private void DoTestServer()
        {
            //  If we don't have a server, bail.
            if (SelectedEntry == null)
            {
                return;
            }

            // TODO

            //  Create a test shell form.
            //var testShellForm = new TestShellForm { TestServer = SelectedEntry };

            //  Show the form.
            //testShellForm.ShowDialog(this);
        }

        private void UpdateUserInterfaceCommands()
        {
            //  Install/Uninstall etc etc only available if we have a selection.
            installServerx86ToolStripMenuItem.Enabled = SelectedEntry?.SharpShellServerInfo != null;
            installServerx64ToolStripMenuItem.Enabled = SelectedEntry?.SharpShellServerInfo != null;
            registerServerx86ToolStripMenuItem.Enabled = SelectedEntry?.SharpShellServerInfo != null;
            registerServerx64ToolStripMenuItem.Enabled = SelectedEntry?.SharpShellServerInfo != null;
            unregisterServerx86ToolStripMenuItem.Enabled = SelectedEntry != null;
            unregisterServerx64ToolStripMenuItem.Enabled = SelectedEntry != null;
            uninstallServerx86ToolStripMenuItem.Enabled = SelectedEntry != null;
            uninstallServerx64ToolStripMenuItem.Enabled = SelectedEntry != null;
            installToolStripMenuItem.Enabled = SelectedEntry?.SharpShellServerInfo != null;
            uninstallToolStripMenuItem.Enabled = SelectedEntry != null;

            // Get selected managed server type
            var serverType = SelectedEntry?.SharpShellServerInfo?.ServerType;

            //  Test functions only available for specific servers.
            testServerToolStripMenuItem.Enabled =
                SelectedEntry != null &&
                (
                    serverType == ServerType.ShellContextMenu ||
                    serverType == ServerType.ShellIconHandler ||
                    serverType == ServerType.ShellInfoTipHandler ||
                    serverType == ServerType.ShellDropHandler ||
                    serverType == ServerType.ShellPreviewHandler ||
                    serverType == ServerType.ShellThumbnailHandler ||
                    serverType == ServerType.ShellIconOverlayHandler
                );

            toolStripButtonTestServer.Enabled = testServerToolStripMenuItem.Enabled;
        }
        
        private bool InstallAssembly(ManagedAssemblyInfo assembly, bool codeBase)
        {
            // Get all server types
            var servers = SharpShellServerInfo.FromAssembly(assembly).ToArray();

            var registrationScope = Environment.Is64BitOperatingSystem
                ? RegistrationScope.OS64Bit
                : RegistrationScope.OS32Bit;

            var success = true;

            foreach (var server in servers)
            {
                try
                {
                    ServerRegistrationManager.InstallServer(server, registrationScope, codeBase);
                    ServerRegistrationManager.RegisterAndApproveServer(server, registrationScope);
                }
                catch (Exception exception)
                {
                    Logging.Error($"Failed to uninstall and unregister a server. [{server.DisplayName}]", exception);
                    success = false;
                }

                AddOrUpdateEntry(new ShellExtensionEntry(server));
            }

            CheckIfRegisterOrUnregisterRequiresExplorerRestart(servers);

            return success;
        }
        
        private bool UninstallAssembly(ManagedAssemblyInfo assemblyInfo)
        {
            // Get all server types
            var servers = SharpShellServerInfo.FromAssembly(assemblyInfo).ToArray();

            var registrationScope = Environment.Is64BitOperatingSystem
                ? RegistrationScope.OS64Bit
                : RegistrationScope.OS32Bit;

            var success = true;

            foreach (var server in servers)
            {
                try
                {
                    ServerRegistrationManager.UnregisterAndUnApproveServer(server, registrationScope);
                    ServerRegistrationManager.UninstallServer(server, registrationScope);
                }
                catch (Exception exception)
                {
                    Logging.Error($"Failed to uninstall and unregister a server. [{server.DisplayName}]", exception);
                    success = false;
                }

                AddOrUpdateEntry(new ShellExtensionEntry(server));
            }

            CheckIfRegisterOrUnregisterRequiresExplorerRestart(servers);

            return success;
        }
        
        private void RemoveRecentlyUsed(string filePath)
        {
            if (Settings.Default.RecentlyUsedFiles != null && !string.IsNullOrEmpty(filePath))
            {
                Settings.Default.RecentlyUsedFiles.Remove(filePath);
                Settings.Default.Save();
            }
        }

        private void AddRecentlyUsed(string filePath)
        {
            if (Settings.Default.RecentlyUsedFiles == null)
            {
                Settings.Default.RecentlyUsedFiles = new StringCollection();
            }

            if (!string.IsNullOrEmpty(filePath) &&
                !Settings.Default.RecentlyUsedFiles.Contains(filePath))
            {
                Settings.Default.RecentlyUsedFiles.Insert(0, filePath);
                Settings.Default.Save();
            }
        }

        private void ClearRecentlyUsed()
        {
            if (Settings.Default.RecentlyUsedFiles != null)
            {
                Settings.Default.RecentlyUsedFiles.Clear();
                Settings.Default.Save();
            }
        }

        private bool InstallServer(ShellExtensionEntry entry, RegistrationScope registrationScope)
        {
            if (entry?.SharpShellServerInfo?.AssemblyInfo == null)
            {
                return false;
            }

            var codeBase = !string.IsNullOrEmpty(entry?.SharpShellServerInfo?.AssemblyInfo?.AssemblyPath);

            ServerRegistrationManager.InstallServer(entry.SharpShellServerInfo, registrationScope, codeBase);

            if (registrationScope == RegistrationScope.OS64Bit)
            {
                entry.UpdateInstallationInfo64();
            }
            else
            {
                entry.UpdateInstallationInfo32();
            }

            AddOrUpdateEntry(entry);

            return true;
        }

        private bool UninstallServer(ShellExtensionEntry entry, RegistrationScope registrationScope)
        {
            if (entry == null)
            {
                return false;
            }

            ServerRegistrationManager.UninstallServer(entry.ServerClassId, registrationScope);

            if (registrationScope == RegistrationScope.OS64Bit)
            {
                entry.UpdateInstallationInfo64();
            }
            else
            {
                entry.UpdateInstallationInfo32();
            }

            AddOrUpdateEntry(entry);

            return true;
        }

        private bool RegisterServer(ShellExtensionEntry entry, RegistrationScope registrationScope)
        {
            if (entry?.SharpShellServerInfo == null)
            {
                return false;
            }

            ServerRegistrationManager.RegisterAndApproveServer(entry.SharpShellServerInfo, registrationScope);

            if (registrationScope == RegistrationScope.OS64Bit)
            {
                entry.UpdateRegistrationInfo64();
            }
            else
            {
                entry.UpdateRegistrationInfo32();
            }

            AddOrUpdateEntry(entry);

            CheckIfRegisterOrUnregisterRequiresExplorerRestart(SelectedEntry?.SharpShellServerInfo);

            return true;
        }

        private bool UnregisterServer(ShellExtensionEntry entry, RegistrationScope registrationScope)
        {
            if (entry == null)
            {
                return false;
            }

            if (entry.SharpShellServerInfo == null)
            {
                ServerRegistrationManager.UnregisterAndUnApproveServer(entry.ServerClassId, registrationScope);
            }
            else
            {
                ServerRegistrationManager.UnregisterAndUnApproveServer(entry.SharpShellServerInfo, registrationScope);
            }

            if (registrationScope == RegistrationScope.OS64Bit)
            {
                entry.UpdateRegistrationInfo64();
            }
            else
            {
                entry.UpdateRegistrationInfo32();
            }

            AddOrUpdateEntry(entry);

            CheckIfRegisterOrUnregisterRequiresExplorerRestart(SelectedEntry?.SharpShellServerInfo);

            return true;
        }

        public void PrepareServerList()
        {
            //  Add the recently used servers. If any of them fail to load, we'll remove them from the list.
            var recentlyUsedFilesToRemove = new List<string>();

            if (Settings.Default.RecentlyUsedFiles != null)
            {
                foreach (var path in Settings.Default.RecentlyUsedFiles)
                {
                    if (!AddServersFromFile(path))
                    {
                        recentlyUsedFilesToRemove.Add(path);
                    }
                }
            }

            foreach (var fileToRemove in recentlyUsedFilesToRemove)
            {
                Settings.Default.RecentlyUsedFiles?.Remove(fileToRemove);
            }

            Settings.Default.Save();

            //  Check for any servers added via the command line.
            var arguments = Environment.GetCommandLineArgs();

            for (var i = 1; i < arguments.Length; i++)
            {
                var arg = arguments[i];

                if (File.Exists(arg))
                {
                    AddServersFromFile(arg);
                }
            }

            var _ = StartBlockingAction(() =>
            {
                foreach (var shellExtensionEntry in ShellExtensionEntry.GetRegisteredEntries())
                {
                    Invoke(new Action(() =>
                    {
                        AddOrUpdateEntry(shellExtensionEntry);
                    }));
                }
            });
        }

        private async Task StartBlockingAction(Action action)
        {
            splitContainer.Enabled = false;
            toolStrip.Enabled = false;
            menuStrip.Enabled = false;
            statusStrip.Enabled = false;
            panelPleaseWait.Visible = true;

            try
            {
                await Task.Factory.StartNew(action).ConfigureAwait(true);
            }
            finally
            {
                panelPleaseWait.Visible = false;

                splitContainer.Enabled = true;
                toolStrip.Enabled = true;
                menuStrip.Enabled = true;
                statusStrip.Enabled = true;
            }
        }

        public bool AddServersFromFile(string path)
        {
            if (Entries.Any(se => se.ServerPath == path))
            {
                return true;
            }

            try
            {
                //  Load any servers from the assembly.
                var servers = SharpShellServerInfo.FromExternalAssemblyFile(path);

                foreach (var server in servers)
                {
                    AddOrUpdateEntry(new ShellExtensionEntry(server));
                }
            }
            catch
            {
                MessageBox.Show($@"The file \'{Path.GetFileName(path)}\' is not a SharpShell Server.", @"Warning");

                return false;
            }

            //  We've successfully added the server - so add the path of the server to our recent files.
            AddRecentlyUsed(path);

            return true;
        }

        public void AddOrUpdateEntry(ShellExtensionEntry extensionEntry)
        {
            if (extensionEntry == null)
            {
                serverDetailsView.ExtensionEntry = null;

                return;
            }

            if (extensionEntry.InstallationInfo32 == null &&
                extensionEntry.InstallationInfo64 == null &&
                extensionEntry.RegistrationInfo32 == null &&
                extensionEntry.RegistrationInfo64 == null &&
                extensionEntry.SharpShellServerInfo == null)
            {
                RemoveExtensionEntryFromList(extensionEntry);
                serverDetailsView.ExtensionEntry = null;

                return;
            }

            var listViewItem = listViewServers.Items
                .Cast<ServerListViewItem>()
                .FirstOrDefault(item =>
                    item.ExtensionEntry?.ServerClassId == extensionEntry.ServerClassId
                );


            if (listViewItem == null)
            {
                listViewItem = new ServerListViewItem(extensionEntry);
                listViewServers.Items.Add(listViewItem);
            }
            else
            {
                listViewItem.ExtensionEntry = extensionEntry;
                listViewServers.Refresh();
            }

            if (!listViewServers.SelectedItems.Contains(listViewItem))
            {
                if (listViewServers.SelectedIndices.Count > 0)
                {
                    listViewServers.SelectedIndices.Clear();
                }

                var itemIndex = listViewServers.Items.IndexOf(listViewItem);

                if (itemIndex >= 0)
                {
                    listViewServers.SelectedIndices.Add(itemIndex);
                }
            }

            // Update the details view with the selected server entry.
            serverDetailsView.ExtensionEntry = extensionEntry;
        }

        public void RemoveExtensionEntryFromList(ShellExtensionEntry extensionEntry)
        {
            var listViewItem = listViewServers.Items
                .Cast<ServerListViewItem>()
                .FirstOrDefault(item =>
                    item.ExtensionEntry?.ServerClassId == extensionEntry.ServerClassId
                );

            if (listViewItem != null)
            {
                if (listViewServers.SelectedItems.OfType<ServerListViewItem>().Contains(listViewItem))
                {
                    listViewServers.SelectedIndices.Clear();
                }

                if (serverDetailsView.ExtensionEntry == extensionEntry)
                {
                    serverDetailsView.ExtensionEntry = null;
                }

                listViewServers.Items.Remove(listViewItem);
            }
        }

        #endregion
    }
}