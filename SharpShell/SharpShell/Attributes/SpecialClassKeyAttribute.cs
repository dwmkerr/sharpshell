using System;
using System.Linq;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Allows the special class key to be defined.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class SpecialClassKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialClassKeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public SpecialClassKeyAttribute(string key)
        {
            SpecialClassKey = key;
        }

        /// <summary>
        /// Gets the attribute for a enum type field with a <see cref="SpecialClassKeyAttribute"/> set, or null if none is set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static SpecialClassKeyAttribute GetPredefinedShellObjectAttribute(Enum value)
        {
            var enumType = value.GetType();
            var enumValueInfo = enumType.GetField(Enum.GetName(enumType, value));

            var specialClassKeyAttribute = enumValueInfo
                .GetCustomAttributes(false)
                .OfType<SpecialClassKeyAttribute>()
                .FirstOrDefault();

            return specialClassKeyAttribute;
        }

        /// <summary>
        /// Gets the special class key.
        /// </summary>
        /// <value>
        /// The special class key.
        /// </value>
        public string SpecialClassKey { get; }
    }
}
