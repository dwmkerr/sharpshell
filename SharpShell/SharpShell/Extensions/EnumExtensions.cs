using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SharpShell.Extensions
{
    /// <summary>
    /// Extension methods for enumerations.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Gets the first value of an attribute of the given type, or null.
        /// </summary>
        /// <typeparam name="T">The attribute type.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>The first value of the given type, or null.</returns>
        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            var memberInfo = enumValue.GetType().GetMember(enumValue.ToString())
                                            .FirstOrDefault();

            if (memberInfo == null) return null;

            var attribute = (T)memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return attribute;
        }
    }
}
