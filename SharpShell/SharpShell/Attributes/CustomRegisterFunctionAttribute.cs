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
        /// Gets the name of CustomRegisterFunction if it exists for a type.
        /// </summary>
        /// <param name="type">The type.</param>
        public static string GetMethodName(Type type)
        {
            //  Does the type have the attribute?
            return type
                .GetMethods(
                    BindingFlags.Static |
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.FlattenHierarchy
                ).FirstOrDefault(m =>
                    ServerSandBox.GetAttributesSafe(m, nameof(CustomRegisterFunctionAttribute), false).Any()
                )?.Name;
        }
    }
}
