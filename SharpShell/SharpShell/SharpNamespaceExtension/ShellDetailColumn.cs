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
            Name = name;
            PropertyKey = propertyKey;
            AverageItemLength = 50;
        }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the property key for the column and detail.
        /// </summary>
        public PropertyKey PropertyKey { get; private set; }

        /// <summary>
        /// Gets or sets the column alignment. The default column alignment is left.
        /// </summary>
        /// <value>
        /// The column alignment.
        /// </value>
        public ColumnAlignment ColumnAlignment { get; set; }

        /// <summary>
        /// Gets or sets the average length of the text for items in the column, used to 
        /// provide an initial size. The default value is 50 characters.
        /// </summary>
        /// <value>
        /// The average length of the item.
        /// </value>
        public uint AverageItemLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column has an image.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this column has an image; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// TODO: replace this with an icon member and set it automatically if the icon is not null.
        /// </remarks>
        public bool HasImage { get; set; }
    }
}