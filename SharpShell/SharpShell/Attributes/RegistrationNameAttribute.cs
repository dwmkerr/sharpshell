using System;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Specify the registry key name under which a SharpShell server should be registered.
    /// By default (without this attribute) a server is registered under the classname.
    /// Since each server needs its own registry key name, this attribute does not inherit.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RegistrationNameAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationNameAttribute"/> class.
        /// </summary>
        /// <param name="registrationName">The registry key name under which the SharpShell server should be registered. Cannot be null or whitespace.</param>
        public RegistrationNameAttribute(string registrationName)
        {
            if (string.IsNullOrWhiteSpace(registrationName))
                throw new ArgumentException("registrationName");
            RegistrationName = registrationName;
        }

        /// <summary>
        /// Gets the registry key name under which to register the SharpShell server.
        /// </summary>
        public string RegistrationName { get; private set; }

        /// <summary>
        /// Gets the registration name for a type if defined.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The registration name of the type if defined, or <value>null</value> otherwise.</returns>
        public static string GetRegistrationName(Type type)
        {
            foreach (RegistrationNameAttribute attribute in type.GetCustomAttributes(typeof(RegistrationNameAttribute), false))
                return attribute.RegistrationName;
            return null;
        }

        /// <summary>
        /// Gets the registration name for a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The registration name of the type if defined, or <value>type.Name</value> otherwise.</returns>
        public static string GetRegistrationNameOrTypeName(Type type)
        {
            return GetRegistrationName(type) ?? type.Name;
        }
    }
}
