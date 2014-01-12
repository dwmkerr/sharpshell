using System;
using System.Linq;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The <see cref="NamespaceExtensionJunctionPointAttribute"/> is an attribute that
    /// must be applied to Sharp Namespace Extensions to specify the location of the extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NamespaceExtensionJunctionPointAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceExtensionJunctionPointAttribute"/> class.
        /// </summary>
        /// <param name="availability">The availability.</param>
        /// <param name="location">The location.</param>
        /// <param name="name">The name.</param>
        public NamespaceExtensionJunctionPointAttribute(NamespaceExtensionAvailability availability, VirtualFolder location, string name)
        {
            //  Store the provided values.
            Availablity = availability;
            Location = location;
            Name = name;
        }

        /// <summary>
        /// Gets the junction point for a type, if defined.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The junction point for a type, or null if one is not defined.</returns>
        public static NamespaceExtensionJunctionPointAttribute GetJunctionPoint(Type type)
        {
            return type.GetCustomAttributes(typeof(NamespaceExtensionJunctionPointAttribute), true)
                .OfType<NamespaceExtensionJunctionPointAttribute>().FirstOrDefault();
        }

        /// <summary>
        /// Gets the availablity of a junction point.
        /// </summary>
        /// <value>
        /// The availablity.
        /// </value>
        public NamespaceExtensionAvailability Availablity { get; private set; }

        /// <summary>
        /// Gets or sets the location of a junction point.
        /// </summary>
        /// <value>
        /// The location of a junction point.
        /// </value>
        public VirtualFolder Location { get; private set; }

        /// <summary>
        /// Gets the name of a junction point.
        /// </summary>
        /// <value>
        /// The name of a junction point.
        /// </value>
        public string Name { get; private set; }
    }
}