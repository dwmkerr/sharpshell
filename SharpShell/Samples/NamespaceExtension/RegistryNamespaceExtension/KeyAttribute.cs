using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SharpShell.Pidl;
using SharpShell.SharpNamespaceExtension;

namespace RegistryNamespaceExtension
{
    public class KeyAttribute : IShellNamespaceItem
    {
        public KeyAttribute(string name, object value)
        {
            this.name = name;
            this.value = value;
        }

        private readonly string name;
        private readonly object value;

        public ShellId GetShellId()
        {
            return ShellId.FromString(name);
        }

        public string GetDisplayName(DisplayNameContext displayNameContext)
        {
            return name;
        }

        public AttributeFlags GetAttributes()
        {
            return AttributeFlags.IsReadOnly;
        }

        public Icon GetIcon()
        {
            return null;
        }
    }
}
