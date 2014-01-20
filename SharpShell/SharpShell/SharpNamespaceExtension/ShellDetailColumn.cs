using System;

namespace SharpShell.SharpNamespaceExtension
{
    public class ShellDetailColumn
    {
        public ShellDetailColumn(string name)
        {
            this.name = name;
            this.uniqueId = Guid.NewGuid();
        }

        public string Name { get { return name; } }
        public Guid UniqueId { get {return uniqueId;}}

        private readonly string name;
        private readonly Guid uniqueId;
    }
}