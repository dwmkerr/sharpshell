using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using SharpShell.ServerRegistration;

namespace ServerManager
{
    internal class ShellExtensionEntry : INotifyPropertyChanged, IEquatable<ShellExtensionEntry>
    {
        private ServerInstallationInfo _installationInfo32;
        private ServerInstallationInfo _installationInfo64;
        private bool _isNative;
        private ShellExtensionRegistrationInfo _registrationInfo32;
        private ShellExtensionRegistrationInfo _registrationInfo64;
        private SharpShellServerInfo _sharpShellServerInfo;

        public ShellExtensionEntry(Guid serverClassId)
        {
            ServerClassId = serverClassId;
        }

        public ShellExtensionEntry(SharpShellServerInfo serverInfo) : this(serverInfo.ClassId)
        {
            SharpShellServerInfo = serverInfo;
            UpdateInstallationInfo32();
            UpdateInstallationInfo64();
            UpdateRegistrationInfo32();
            UpdateRegistrationInfo64();
        }

        public ServerInstallationInfo InstallationInfo32
        {
            get => _installationInfo32;
            private set
            {
                _installationInfo32 = value;
                OnPropertyChanged(nameof(InstallationInfo32));
                OnPropertyChanged(nameof(ServerPath));
                OnPropertyChanged(nameof(IsNative));
                OnPropertyChanged(nameof(ServerDisplayName));
            }
        }

        public ServerInstallationInfo InstallationInfo64
        {
            get => _installationInfo64;
            private set
            {
                _installationInfo64 = value;
                OnPropertyChanged(nameof(InstallationInfo64));
                OnPropertyChanged(nameof(ServerPath));
                OnPropertyChanged(nameof(IsNative));
                OnPropertyChanged(nameof(ServerDisplayName));
            }
        }

        public bool IsNative
        {
            get
            {
                if (_isNative)
                {
                    return true;
                }

                if (InstallationInfo32 != null)
                {
                    return InstallationInfo32.ServerInstallationType != ServerInstallationType.ManagedInProcess32;
                }

                if (InstallationInfo64 != null)
                {
                    return InstallationInfo64.ServerInstallationType != ServerInstallationType.ManagedInProcess32;
                }

                return false;
            }
            set
            {
                _isNative = value;
                OnPropertyChanged(nameof(IsNative));
            }
        }

        public ShellExtensionRegistrationInfo RegistrationInfo32
        {
            get => _registrationInfo32;
            private set
            {
                _registrationInfo32 = value;
                OnPropertyChanged(nameof(RegistrationInfo32));
                OnPropertyChanged(nameof(ShellAssociatedClassNames));
                OnPropertyChanged(nameof(ServerDisplayName));
            }
        }

        public ShellExtensionRegistrationInfo RegistrationInfo64
        {
            get => _registrationInfo64;
            private set
            {
                _registrationInfo64 = value;
                OnPropertyChanged(nameof(RegistrationInfo64));
                OnPropertyChanged(nameof(ShellAssociatedClassNames));
                OnPropertyChanged(nameof(ServerDisplayName));
            }
        }

        public Guid ServerClassId { get; }

        public string ServerDisplayName
        {
            get
            {
                return SharpShellServerInfo?.DisplayName ??
                       SharpShellServerInfo?.RegistrationName ??
                       SharpShellServerInfo?.ClassName ??
                       RegistrationInfo32?.Associations
                           .FirstOrDefault(info => !string.IsNullOrWhiteSpace(info.RegistrationName))
                           ?.RegistrationName ??
                       RegistrationInfo64?.Associations
                           .FirstOrDefault(info => !string.IsNullOrWhiteSpace(info.RegistrationName))
                           ?.RegistrationName ??
                       InstallationInfo32?.ManagedClassName ??
                       InstallationInfo64?.ManagedClassName ??
                       ServerClassId.ToString("B");
            }
        }

        public string ServerPath
        {
            get => InstallationInfo32?.ServerPath ?? InstallationInfo64?.ServerPath;
        }

        public SharpShellServerInfo SharpShellServerInfo
        {
            get => _sharpShellServerInfo;
            private set
            {
                _sharpShellServerInfo = value;
                OnPropertyChanged(nameof(SharpShellServerInfo));
                OnPropertyChanged(nameof(ServerDisplayName));
            }
        }

        public string[] ShellAssociatedClassNames
        {
            get
            {
                if (RegistrationInfo32 != null || RegistrationInfo64 != null)
                {
                    return (RegistrationInfo32?.Associations ?? new ShellExtensionRegisteredAssociationInfo[0])
                        .Concat(RegistrationInfo64?.Associations ?? new ShellExtensionRegisteredAssociationInfo[0])
                        .Select(info => info.AssociationClassName).Distinct().ToArray();
                }

                return (SharpShellServerInfo?.AssociationClassNamesX32 ?? new string[0])
                    .Concat(SharpShellServerInfo?.AssociationClassNamesX64 ?? new string[0])
                    .Distinct().ToArray();
            }
        }

        public ShellExtensionType[] ShellExtensionTypes
        {
            get
            {
                var extensionTypes = new List<ShellExtensionType>();

                if (SharpShellServerInfo?.ShellExtensionType != null)
                {
                    extensionTypes.Add(SharpShellServerInfo.ShellExtensionType);
                }

                if (RegistrationInfo32 != null)
                {
                    extensionTypes.AddRange(RegistrationInfo32.Associations.Select(info => info.ShellExtensionType));
                }

                if (RegistrationInfo64 != null)
                {
                    extensionTypes.AddRange(RegistrationInfo64.Associations.Select(info => info.ShellExtensionType));
                }

                if (extensionTypes.Count == 0)
                {
                    return new[] {ShellExtensionType.None};
                }

                return extensionTypes.Distinct().Where(type => type != ShellExtensionType.None).ToArray();
            }
        }

        /// <inheritdoc />
        public bool Equals(ShellExtensionEntry other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return ServerClassId.Equals(other.ServerClassId);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static IEnumerable<ShellExtensionEntry> GetRegisteredEntries()
        {
            var extensions32 = ServerRegistrationManager.EnumerateRegisteredExtensions(RegistrationScope.OS32Bit)
                .Select(info =>
                    new Tuple<RegistrationScope, ShellExtensionRegistrationInfo>(RegistrationScope.OS32Bit, info)
                );

            var extensions64 = ServerRegistrationManager.EnumerateRegisteredExtensions(RegistrationScope.OS64Bit)
                .Select(info =>
                    new Tuple<RegistrationScope, ShellExtensionRegistrationInfo>(RegistrationScope.OS64Bit, info)
                );

            var extensions = extensions32.Concat(extensions64).GroupBy(info => info.Item2.ServerClassId);

            foreach (var extensionGroup in extensions)
            {
                var extension = new ShellExtensionEntry(extensionGroup.Key)
                {
                    RegistrationInfo64 =
                        extensionGroup.FirstOrDefault(t => t.Item1 == RegistrationScope.OS64Bit)?.Item2,
                    RegistrationInfo32 = extensionGroup.FirstOrDefault(t => t.Item1 == RegistrationScope.OS32Bit)?.Item2
                };

                extension.UpdateManagedServerInfo();

                yield return extension;
            }
        }

        public static bool operator ==(ShellExtensionEntry left, ShellExtensionEntry right)
        {
            return Equals(left, right) || left?.Equals(right) == true;
        }

        public static bool operator !=(ShellExtensionEntry left, ShellExtensionEntry right)
        {
            return !(left == right);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as ShellExtensionEntry);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ServerClassId.GetHashCode();
        }

        public void UpdateInstallationInfo32()
        {
            var installation = ServerRegistrationManager.GetExtensionInstallationInfo(
                ServerClassId,
                RegistrationScope.OS32Bit
            );

            if (installation?.ServerInstallationType != ServerInstallationType.PartiallyInstalled)
            {
                InstallationInfo32 = installation;
            }
            else
            {
                InstallationInfo32 = null;
            }
        }

        public void UpdateInstallationInfo64()
        {
            var installation = ServerRegistrationManager.GetExtensionInstallationInfo(
                ServerClassId,
                RegistrationScope.OS64Bit
            );

            if (installation?.ServerInstallationType != ServerInstallationType.PartiallyInstalled)
            {
                InstallationInfo64 = installation;
            }
            else
            {
                InstallationInfo64 = null;
            }
        }

        public void UpdateManagedServerInfo()
        {
            if (SharpShellServerInfo != null || IsNative)
            {
                return;
            }

            UpdateInstallationInfo32();
            UpdateInstallationInfo64();

            try
            {
                if (InstallationInfo64?.ServerInstallationType == ServerInstallationType.ManagedInProcess32)
                {
                    SharpShellServerInfo = InstallationInfo64.GetSharpShellServerInformation();
                }
                else if (InstallationInfo32?.ServerInstallationType == ServerInstallationType.ManagedInProcess32)
                {
                    SharpShellServerInfo = InstallationInfo32.GetSharpShellServerInformation();
                }
            }
            catch
            {
                IsNative = true;
            }
        }

        public void UpdateRegistrationInfo32()
        {
            if (SharpShellServerInfo != null)
            {
                var registration = ServerRegistrationManager.GetExtensionRegistrationInfo(
                    SharpShellServerInfo,
                    RegistrationScope.OS32Bit
                );

                if (registration?.Associations.Any() == true)
                {
                    RegistrationInfo32 = registration;
                }
                else
                {
                    RegistrationInfo32 = null;
                }
            }
            else
            {
                var registration = ServerRegistrationManager.GetExtensionRegistrationInfo(
                    ServerClassId,
                    RegistrationScope.OS32Bit
                );

                if (registration?.Associations.Any() == true)
                {
                    RegistrationInfo32 = registration;
                }
                else
                {
                    RegistrationInfo32 = null;
                }
            }
        }

        public void UpdateRegistrationInfo64()
        {
            if (SharpShellServerInfo != null)
            {
                var registration = ServerRegistrationManager.GetExtensionRegistrationInfo(
                    SharpShellServerInfo,
                    RegistrationScope.OS64Bit
                );
                
                if (registration?.Associations.Any() == true)
                {
                    RegistrationInfo64 = registration;
                }
                else
                {
                    RegistrationInfo64 = null;
                }
            }
            else
            {
                var registration = ServerRegistrationManager.GetExtensionRegistrationInfo(
                    ServerClassId,
                    RegistrationScope.OS64Bit
                );

                if (registration?.Associations.Any() == true)
                {
                    RegistrationInfo64 = registration;
                }
                else
                {
                    RegistrationInfo64 = null;
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch
            {
                // ignored
            }
        }
    }
}