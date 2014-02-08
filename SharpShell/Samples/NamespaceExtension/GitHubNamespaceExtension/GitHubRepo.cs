using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SharpShell.Interop;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace GitHubNamespaceExtension
{
    public class GitHubRepo : IShellNamespaceFolder, IShellNamespaceFolderContextMenuProvider
    {
        internal GitHubRepo(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns>The unique identifier, which in this case is based on the repo name.</returns>
        public ShellId GetShellId()
        {
            //  Use the name as the source of the ID.
            return ShellId.FromString(Name);
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="displayNameContext">The display name context.</param>
        /// <returns></returns>
        public string GetDisplayName(DisplayNameContext displayNameContext)
        {
            //  In all cases, we just use the repo name
            return Name;
        }

        public AttributeFlags GetAttributes()
        {
            return AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders;
        }

        public Icon GetIcon()
        {
            return null;
        }

        public string Name { get; private set; }

        public IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            return new[]
            {
                new GitHubBranch {Name = "Branch1"},
                new GitHubBranch {Name = "Branch2"},
                new GitHubBranch {Name = "Branch3"}
            };
        }

        public ShellNamespaceFolderView GetView()
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