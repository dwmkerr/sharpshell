using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharpShell;
using SharpShell.ServerRegistration;

namespace ServerManager.Views
{
    internal class ServerListViewItem : ListViewItem
    {
        private ShellExtensionEntry _extensionEntry;

        public ServerListViewItem(ShellExtensionEntry extensionEntry)
        {
            ExtensionEntry = extensionEntry;
        }

        public ServerListViewItem()
        {
        }

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
            SubItems.Clear();

            if (ExtensionEntry != null)
            {
                Text = ExtensionEntry.ServerDisplayName;
                SubItems.Add(string.Join(", ", ExtensionEntry.ShellExtensionTypes.Select(type => type.ToString())));
                SubItems.Add(ExtensionEntry.ServerClassId.ToString("B"));

                if (ExtensionEntry.ShellExtensionTypes.Contains(ShellExtensionType.ShellContextMenu))
                {
                    ImageIndex = 0;
                }
                else if (ExtensionEntry.ShellExtensionTypes.Contains(ShellExtensionType.ShellIconHandler))
                {
                    ImageIndex = 1;
                }
                else if (ExtensionEntry.ShellExtensionTypes.Contains(ShellExtensionType.ShellPropertySheet))
                {
                    ImageIndex = 2;
                }
                else if (ExtensionEntry.ShellExtensionTypes.Contains(ShellExtensionType.ShellInfoTipHandler))
                {
                    ImageIndex = 3;
                }
                else if (ExtensionEntry.SharpShellServerInfo?.ServerType == ServerType.ShellIconOverlayHandler)
                {
                    ImageIndex = 4;
                }
                else
                {
                    ImageIndex = 0;
                }

                if (ExtensionEntry.SharpShellServerInfo != null)
                {
                    BackColor = Color.FromArgb(221, 242, 255);
                }
                else
                {
                    BackColor = Color.White;
                }

                if (ExtensionEntry.InstallationInfo32 == null &&
                    ExtensionEntry.InstallationInfo64 == null &&
                    ExtensionEntry.RegistrationInfo32 == null &&
                    ExtensionEntry.RegistrationInfo64 == null)
                {
                    ForeColor = Color.FromArgb(217, 0, 0);
                }
                else if (ExtensionEntry.InstallationInfo32 == null && ExtensionEntry.InstallationInfo64 == null ||
                         ExtensionEntry.RegistrationInfo32 == null && ExtensionEntry.RegistrationInfo64 == null)
                {
                    ForeColor = Color.FromArgb(116, 91, 0);
                }
                else
                {
                    ForeColor = Color.Black;
                }
            }
            else
            {
                Text = @"[N/A]";
                SubItems.Add(@"[N/A]");
                SubItems.Add(@"[N/A]");
            }
        }
    }
}