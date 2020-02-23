using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.Attributes
{
    /// <summary>
    /// The name attribute can be used to give a class display name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DisplayNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayNameAttribute"/> class.
        /// </summary>
        /// <param name="displayName">The display name.</param>
        public DisplayNameAttribute(string displayName)
        {
            //   Set the display name.
            DisplayName = displayName;
        }

        /// <summary>
        /// Gets the display name for a type, if defined.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The display name of the type, if defined.</returns>
        public static string GetDisplayName(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof (DisplayNameAttribute), true)
                .OfType<DisplayNameAttribute>().FirstOrDefault();
            return attribute != null ? attribute.DisplayName : null;
        }

        /// <summary>
        /// Gets the display name of the (if defined and not empty) for a type.
        /// If there is no display name, or it is empty, the type name is returned.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The display name of the (if defined and not empty) for a type, otherwise the type name.</returns>
        public static string GetDisplayNameOrTypeName(Type type)
        {
            //  Return the display name if it is set, otherwise the type name.
            var displayName = GetDisplayName(type);
            return string.IsNullOrEmpty(displayName) ? type.Name : displayName;
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        public string DisplayName { get; private set; }
    }
}
