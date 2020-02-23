using System;
using System.Linq;
using System.Collections.Generic;
using SharpShell.ServerRegistration;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Attribute to associate a SharpShell server with a file extension.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [Serializable]
    public class COMServerAssociationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="COMServerAssociationAttribute"/> class.
        /// </summary>
        /// <param name="associationType">Type of the association.</param>
        /// <param name="associations">The associations.</param>
        public COMServerAssociationAttribute(AssociationType associationType, params string[] associations)
        {
            //  Set the association type.
            AssociationType = associationType;

            //  Set the associations.
            Associations = associations;
        }

        /// <summary>
        /// Gets the file extension associations for a specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The set of file extension associations.</returns>
        public static IEnumerable<string> GetAssociations(Type type)
        {
            return GetAssociationAttributes(type).SelectMany(attribute => attribute.Associations ?? new string[0]);
        }

        /// <summary>
        /// Gets the type of the association.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<AssociationType> GetAssociationTypes(Type type)
        {
            return GetAssociationAttributes(type)?
                .Select(attribute => attribute?.AssociationType ?? AssociationType.None)
                .Where(associationType => associationType != AssociationType.None);
        }

        /// <summary>
        /// Gets all COMServerAssociationAttribute attributes of this class
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IEnumerable<COMServerAssociationAttribute> GetAssociationAttributes(Type type)
        {
            var attributes = ServerSandBox.GetAttributesSafe(type, nameof(COMServerAssociationAttribute), true);

            if (attributes == null)
            {
                yield break;
            }

            foreach (var attribute in attributes)
            {
                var associationType = ServerSandBox.GetByValPropertySafe<AssociationType>(
                    attribute,
                    nameof(AssociationType)
                );

                var associations = ServerSandBox.GetByRefPropertySafe<IEnumerable<string>>(
                    attribute,
                    nameof(Associations)
                );

                if (associationType != null && associations != null)
                {
                    yield return new COMServerAssociationAttribute(associationType.Value, associations.ToArray());
                }
            }
        }

        public IEnumerable<string> GetAssociationClassNames(RegistrationScope registrationScope)
        {
            return ServerRegistrationManager.GetAssociationClassNames(AssociationType, Associations, registrationScope);
        }

        /// <summary>
        /// Gets the type of the association.
        /// </summary>
        /// <value>
        /// The type of the association.
        /// </value>
        public AssociationType AssociationType { get; }

        /// <summary>
        /// Gets the associations.
        /// </summary>
        public IEnumerable<string> Associations { get; }
    }
}
