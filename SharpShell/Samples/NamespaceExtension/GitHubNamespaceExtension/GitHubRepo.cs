using System;
using System.Text;
using SharpShell.SharpNamespaceExtension;

namespace GitHubNamespaceExtension
{
    public class GitHubRepo : ShellNamespaceFolder
    {
        internal GitHubRepo(string name)
        {
            Name = name;
        }
        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns>The unique identifier, which in this case is based on the repo name.</returns>
        public override byte[] GetUniqueId()
        {
            //  Use the name as the source of the ID.
            return Encoding.UTF8.GetBytes(Name);
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <param name="displayNameContext">The display name context.</param>
        /// <returns></returns>
        public override string GetDisplayName(DisplayNameContext displayNameContext)
        {
            //  In all cases, we just use the repo name
            return Name;
        }

        public override AttributeFlags GetAttributes()
        {
            return AttributeFlags.IsFolder | AttributeFlags.MayContainSubFolders;
        }

        public string Name { get; private set; }
    }
}