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
        /// <summary>
        /// Gets the attributes for the namespace extension. These attributes can be used
        /// to identify that a shell extension is a folder, contains folders, is part of the
        /// file system and so on and so on.
        /// </summary>
        /// <returns>
        /// The attributes for the shell item
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override AttributeFlags GetAttributes()
        {
            //  The GitHub namespace extension is a folder, contains folders and is faked as part of the file system.
            return AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders;
        }

        public override IEnumerable<IShellNamespaceItem> EnumerateChildren(uint index, uint count, EnumerateChildrenFlags flags)
        {
            //  Return the children we've been asked for.
            if (index + count <= sampleRepos.Length)
                return sampleRepos.Skip((int) index).Take((int) count).ToList();

            //  We've got no items to return.
            return new List<IShellNamespaceItem>();
        }

        public override IShellNamespaceItem GetChildItem(IdList idList)
        {
            /*foreach(var item in sampleRepos)
                if (item.GetUniqueId() == idList)
                    return item; */
            return null;
        }

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

        public override Control CreateView()
        {
            throw new NotImplementedException();
        }
    }
}