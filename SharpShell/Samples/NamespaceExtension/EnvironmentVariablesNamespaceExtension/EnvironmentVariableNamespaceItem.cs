using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace EnvironmentVariablesNamespaceExtension
{
    class EnvironmentVariableNamespaceItem : IShellNamespaceItem
    {
        private readonly string _name;

        public EnvironmentVariableNamespaceItem(string name)
        {
            _name = name;
        }

        /// <inheritdoc />
        public ShellId GetShellId()
        {
            //  The environment variable name will be unique in the folder, so works perfectly as a PIDL.
            return ShellId.FromString(_name);
        }

        /// <inheritdoc />
        public string GetDisplayName(DisplayNameContext displayNameContext)
        {
            //  Our display name is always just the key name.
            return _name;
        }

        /// <inheritdoc />
        public AttributeFlags GetAttributes()
        {
            //  We can copy, delete or rename environment variables.
            return AttributeFlags.CanByCopied | AttributeFlags.CanBeDeleted | AttributeFlags.CanBeRenamed;
        }

        public Icon GetIcon()
        {
            //  Return the environment variable icon.
            return Properties.Resources.EnvironmentVariable;
        }

        public string GetValue()
        {
            return Environment.GetEnvironmentVariable(_name);
        }
    }
}
