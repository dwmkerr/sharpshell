using System.Reflection;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// A ShellDetailColumn is used to define a column in a <see cref="DefaultNamespaceFolderView" />.
    /// This class specifies how the column is shown, but also how the property of the shell item that is represented
    /// by the detail column is referenced, via it's <see cref="PropertyKey" />.
    /// Use standard property keys with the constructor that takes a <see cref="StandardPropertyKey" /> or create
    /// custom property keys by manually specifiying the guid for the <see cref="PropertyKey" />.
    /// </summary>
    public class ShellDetailColumn
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellDetailColumn"/> class.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="propertyKey">The property key.</param>
        public ShellDetailColumn(string name, PropertyKey propertyKey)
        {
            this.name = name;
            this.propertyKey = propertyKey;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Gets the property key for the column and detail.
        /// </summary>
        public PropertyKey PropertyKey { get { return propertyKey; } }

        /// <summary>
        /// The name of the column.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The property key for the column and detail.
        /// </summary>
        private readonly PropertyKey propertyKey;
    }
}