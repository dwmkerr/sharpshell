using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace ServerManager.Views
{
    internal partial class ServerDetailsView : UserControl
    {

        public ServerDetailsView()
        {
            InitializeComponent();
        }
        private ShellExtensionEntry _extensionEntry;

        public ShellExtensionEntry ExtensionEntry
        {
            get => _extensionEntry;
            set
            {
                if (!ReferenceEquals(_extensionEntry, value))
                {
                    if (_extensionEntry != null)
                    {
                        _extensionEntry.PropertyChanged -= ExtensionEntryOnPropertyChanged;
                    }

                    _extensionEntry = value;

                    if (_extensionEntry != null)
                    {
                        _extensionEntry.PropertyChanged += ExtensionEntryOnPropertyChanged;
                    }
                }

                UpdateUI();
            }
        }

        private void ExtensionEntryOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Setting default values for text boxes
            textBoxServerName.Text = @"[N/A]";
            textBoxServerClassId.Text = @"[N/A]";
            textBoxServerPath.Text = @"[N/A]";
            textBoxExntensionTypes.Text = @"[N/A]";
            textBoxAssociations.Text = @"[N/A]";

            textBoxManagedName.Text = @"[N/A]";
            textBoxManagedVersion.Text = @"[N/A]";
            textBoxManagedRuntime.Text = @"[N/A]";
            textBoxManagedClassName.Text = @"[N/A]";
            textBoxManagedSecurity.Text = @"[N/A]";

            textBoxSharpShellServerType.Text = @"[N/A]";
            textBoxSharpShellVersion.Text = @"[N/A]";

            textBox32BitServerInstallation.Text = @"[Not Installed]";
            textBox64BitServerInstallation.Text = @"[Not Installed]";

            textBox32BitServerRegistration.Text = @"[Not Registered]";
            textBox64BitServerRegistration.Text = @"[Not Registered]";

            if (ExtensionEntry != null)
            {
                // General information
                textBoxServerName.Text = ExtensionEntry.ServerDisplayName;
                textBoxServerClassId.Text = ExtensionEntry.ServerClassId.ToString("B");
                textBoxServerPath.Text = ExtensionEntry.ServerPath;
                textBoxAssociations.Text = string.Join(", ", ExtensionEntry.ShellAssociatedClassNames);
                textBoxExntensionTypes.Text = string.Join(", ", ExtensionEntry.ShellExtensionTypes.Select(type => type.ToString()));

                // Managed information
                if (ExtensionEntry.SharpShellServerInfo != null)
                {
                    textBoxManagedName.Text = ExtensionEntry.SharpShellServerInfo?.AssemblyInfo.FullName;
                    textBoxManagedVersion.Text = ExtensionEntry.SharpShellServerInfo.AssemblyInfo.Version;
                    textBoxManagedRuntime.Text = ExtensionEntry.SharpShellServerInfo.AssemblyInfo.RuntimeVersion;
                    textBoxManagedClassName.Text = ExtensionEntry.SharpShellServerInfo.ClassFullName;
                    textBoxManagedSecurity.Text = ExtensionEntry.SharpShellServerInfo.AssemblyInfo.IsSigned
                        ? "Signed"
                        : "Unsigned";

                    textBoxSharpShellServerType.Text = ExtensionEntry.SharpShellServerInfo.ServerType.ToString();
                    if (ExtensionEntry.SharpShellServerInfo.SharpShellAssemblyInfo != null)
                    {
                        textBoxSharpShellVersion.Text =
                            ExtensionEntry.SharpShellServerInfo.SharpShellAssemblyInfo.Version;
                    }
                }
                
                //  Do we have 32 bit registration info?
                if (ExtensionEntry.InstallationInfo32 != null)
                {
                    //  Do we have a codebase?
                    if (!string.IsNullOrEmpty(ExtensionEntry.InstallationInfo32?.ManagedAssembly?.AssemblyPath))
                    {
                        textBox32BitServerInstallation.Text = ExtensionEntry.InstallationInfo32.ManagedAssembly.AssemblyPath;
                    }
                    else if (!string.IsNullOrEmpty(ExtensionEntry.InstallationInfo32?.ManagedAssembly?.FullName))
                    {
                        textBox32BitServerInstallation.Text = ExtensionEntry.InstallationInfo32.ManagedAssembly.FullName + @" (GAC)";
                    }
                    else if (!string.IsNullOrEmpty(ExtensionEntry.InstallationInfo32?.ServerPath))
                    {
                        textBox32BitServerInstallation.Text = ExtensionEntry.InstallationInfo32.ServerPath + @" (Native)";
                    }
                }

                //  Do we have 64 bit registration info?
                if (ExtensionEntry.InstallationInfo64 != null)
                {
                    //  Do we have a codebase?
                    if (!string.IsNullOrEmpty(ExtensionEntry.InstallationInfo64?.ManagedAssembly?.AssemblyPath))
                    {
                        textBox64BitServerInstallation.Text = ExtensionEntry.InstallationInfo64.ManagedAssembly?.AssemblyPath;
                    }
                    else if (!string.IsNullOrEmpty(ExtensionEntry.InstallationInfo64?.ManagedAssembly?.FullName))
                    {
                        textBox64BitServerInstallation.Text = ExtensionEntry.InstallationInfo64.ManagedAssembly?.FullName + @" (GAC)";
                    }
                    else if (!string.IsNullOrEmpty(ExtensionEntry.InstallationInfo64?.ServerPath))
                    {
                        textBox64BitServerInstallation.Text = ExtensionEntry.InstallationInfo64.ServerPath + @" (Native)";
                    }
                }

                //  Set the registration info.
                if (ExtensionEntry.RegistrationInfo32?.Associations.Any() == true)
                {
                    textBox32BitServerRegistration.Text = ExtensionEntry.RegistrationInfo32.IsApproved
                        ? @"Registered and Approved"
                        : @"Registered";
                }

                //  Set the registration info.
                if (ExtensionEntry.RegistrationInfo64?.Associations.Any() == true)
                {
                    textBox64BitServerRegistration.Text = ExtensionEntry.RegistrationInfo64.IsApproved
                        ? @"Registered and Approved"
                        : @"Registered";
                }
            }
        }
    }
}
