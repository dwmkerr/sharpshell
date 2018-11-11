using System;
using System.Linq;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Provides metadata for a predefined shell object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class PredefinedShellObjectAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PredefinedShellObjectAttribute"/> class.
        /// </summary>
        /// <param name="className">Name of the class in the registry for this object.</param>
        public PredefinedShellObjectAttribute(string className)
        {
            ClassName = className;
        }

        /// <summary>
        /// Gets the class name for a type with a <see cref="PredefinedShellObjectAttribute"/> set, or null if none is set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string GetClassName(Enum value)
        {
            var enumType = value.GetType();
            var enumValueInfo = enumType.GetField(Enum.GetName(enumType, value));
            var predefinedShellObject = enumValueInfo.GetCustomAttributes(false).OfType<PredefinedShellObjectAttribute>().FirstOrDefault();
            return predefinedShellObject?.ClassName;
        }

        /// <summary>
        /// Gets the class name.
        /// </summary>
        public string ClassName { get; }
    }
}
