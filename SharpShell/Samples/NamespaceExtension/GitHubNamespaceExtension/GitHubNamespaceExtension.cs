using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace GitHubNamespaceExtension
{
    /// <summary>
    /// 
    /// </summary>
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.Desktop, "GitHub")]
    public class GitHubNamespaceExtension : SharpNamespaceExtension
    {
        private readonly GitHubRepo[] sampleRepos = new GitHubRepo[]
        {
            new GitHubRepo("SharpShell"),
            new GitHubRepo("SharpGL"),
            new GitHubRepo("Space Invaders")
        };

        /// <summary>
        /// Gets the registration settings. This function is called only during the initial
        /// registration of a shell namespace extension to provide core configuration.
        /// </summary>
        /// <returns>
        /// Registration settings for the server.
        /// </returns>
        public override NamespaceExtensionRegistrationSettings GetRegistrationSettings()
        {
            return new NamespaceExtensionRegistrationSettings
            {
                ExtensionAttributes = AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders
            };
        }

        protected override IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            return sampleRepos;
        }

        protected override ShellNamespaceFolderView GetView()
        {
            return new DefaultNamespaceFolderView(new [] {new ShellDetailColumn("Name"), });
        }
    }
}