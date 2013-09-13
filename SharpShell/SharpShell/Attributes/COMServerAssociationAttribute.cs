using System;
using System.Linq;
using System.Collections.Generic;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Attribute to associate a SharpShell server with a file extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class COMServerAssociationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="COMServerAssociationAttribute"/> class.
        /// </summary>
        /// <param name="associationType">Type of the association.</param>
        /// <param name="associations">The associations.</param>
        public COMServerAssociationAttribute(AssociationType associationType, params string[] associations)
        {
            //  Set the assocation type.
            this.associationType = associationType;

            //  Set the associations.
            this.associations = associations;
        }

        /// <summary>
        /// Gets the file extension associations for a specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The set of file extension assocations.</returns>
        public static IEnumerable<string> GetAssociations(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(COMServerAssociationAttribute), true)
                .OfType<COMServerAssociationAttribute>().FirstOrDefault();
            return attribute != null ? attribute.Associations : new string[] {};
        }

        /// <summary>
        /// Gets the type of the association.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static AssociationType GetAssociationType(Type type)
        {
            var attribute = type.GetCustomAttributes(typeof(COMServerAssociationAttribute), true)
                .OfType<COMServerAssociationAttribute>().FirstOrDefault();
            return attribute != null ? attribute.AssociationType : AssociationType.None;
        }

        /// <summary>
        /// The association type.
        /// </summary>
        private readonly AssociationType associationType;

        /// <summary>
        /// The extensions.
        /// </summary>
        private readonly string[] associations;

        /// <summary>
        /// Gets the type of the association.
        /// </summary>
        /// <value>
        /// The type of the association.
        /// </value>
        public AssociationType AssociationType { get { return associationType; } }

        /// <summary>
        /// Gets the associations.
        /// </summary>
        public IEnumerable<string> Associations { get { return associations; } }
    }
}
