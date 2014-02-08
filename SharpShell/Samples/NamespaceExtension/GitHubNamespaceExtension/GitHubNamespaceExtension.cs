using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Interop;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace GitHubNamespaceExtension
{
    /// <summary>
    /// 
    /// </summary>
    [ComVisible(true)]
    [NamespaceExtensionJunctionPoint(NamespaceExtensionAvailability.Everyone, VirtualFolder.Desktop, "GitHub")]
    public class GitHubNamespaceExtension : SharpNamespaceExtension, IShellNamespaceFolderContextMenuProvider
    {
        private readonly IShellNamespaceItem[] sampleRepos = new IShellNamespaceItem[]
        {
            new GitHubRepo("SharpShell"),
            new GitHubRepo("SharpGL"),
            new GitHubRepo("Space Invaders"),
                new GitHubBranch {Name = "Branch1"},
                new GitHubBranch {Name = "Branch2"},
                new GitHubBranch {Name = "Branch3"}
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
                ExtensionAttributes = AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders,
                Tooltip = "GitHub Shell Namespace Extension (SharpShell)"
            };
        }

        protected override IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            return sampleRepos;
        }

        protected override ShellNamespaceFolderView GetView()
        {
            return new DefaultNamespaceFolderView(new[]
            {
                new ShellDetailColumn("Name", new PropertyKey(StandardPropertyKey.PKEY_ItemNameDisplay))
            },
                (item, column) =>
                {
                    return item.GetDisplayName(DisplayNameContext.Normal);
                });
        }

        public IContextMenu CreateContextMenu(IdList folderIdList, IdList[] folderItemIdLists)
        {
            return new GithubContextMenu(folderIdList, folderItemIdLists);
        }
    }
}