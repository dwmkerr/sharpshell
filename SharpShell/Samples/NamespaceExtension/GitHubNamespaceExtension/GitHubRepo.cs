using System;
using System.Collections.Generic;
using System.Text;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace GitHubNamespaceExtension
{
    public class GitHubRepo : IShellNamespaceFolder
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

        public string Name { get; private set; }
        public IEnumerable<IShellNamespaceItem> GetChildren(ShellNamespaceEnumerationFlags flags)
        {
            throw new NotImplementedException();
        }

        public ShellNamespaceFolderView GetView()
        {
            throw new NotImplementedException();
        }
    }
}