using System;
using System.Linq;
using SharpShell.ServerRegistration;

namespace SharpShell.Attributes
{
    /// <summary>
    ///     Specify the registry key name under which a SharpShell server should be registered.
    ///     By default (without this attribute) a server is registered under the classname.
    ///     Since each server needs its own registry key name, this attribute does not inherit.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Serializable]
    public class RegistrationNameAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegistrationNameAttribute" /> class.
        /// </summary>
        /// <param name="registrationName">
        ///     The registry key name under which the SharpShell server should be registered. Cannot be
        ///     null or whitespace.
        /// </param>
        public RegistrationNameAttribute(string registrationName)
        {
            if (string.IsNullOrWhiteSpace(registrationName))
            {
                throw new ArgumentException(nameof(registrationName));
            }

            RegistrationName = registrationName;
        }

        /// <summary>
        ///     Gets the registry key name under which to register the SharpShell server.
        /// </summary>
        public string RegistrationName { get; }

        /// <summary>
        ///     Gets the registration name attribute for a type if defined, otherwise returns null.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The registration name of the type if defined, or
        ///     <value>null</value>
        ///     otherwise.
        /// </returns>
        public static RegistrationNameAttribute GetRegistrationNameAttribute(Type type)
        {
            var attribute = ServerSandBox
                .GetAttributesSafe(type, nameof(RegistrationNameAttribute), false)
                .FirstOrDefault();

            if (attribute == null)
            {
                return null;
            }

            var registrationName = ServerSandBox.GetStringPropertySafe(attribute, nameof(RegistrationName));

            if (registrationName != null)
            {
                return new RegistrationNameAttribute(registrationName);
            }

            return null;
        }
    }
}