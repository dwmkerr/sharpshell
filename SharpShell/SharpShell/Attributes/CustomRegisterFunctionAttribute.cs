using System;
using System.Linq;
using System.Reflection;
using SharpShell.ServerRegistration;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Identifies a function as being a static custom registration function.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CustomRegisterFunctionAttribute : Attribute
    {
        /// <summary>
        /// Executes the CustomRegisterFunction if it exists for a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="registrationType">Type of the registration.</param>
        public static void ExecuteIfExists(Type type, RegistrationType registrationType)
        {
            //  Does the type have the attribute?
            var methodWithAttribute = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                .FirstOrDefault(m => m.GetCustomAttributes(typeof(CustomRegisterFunctionAttribute), false).Any());

            //  Do we have a method? If so, invoke it.
            if (methodWithAttribute != null)
                methodWithAttribute.Invoke(null, new object[] {type, registrationType});
        }
    }
}
