using System;
using System.Collections.Generic;
using SharpShell.Extensions;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Attribute to describe handler sub-key config.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class HandlerSubKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerSubKeyAttribute" /> class.
        /// </summary>
        /// <param name="handlerSubKey">The handler sub-key name.</param>
        /// <param name="allowMultipleEntries">if set to <c>true</c> [allow multiple entries].</param>
        public HandlerSubKeyAttribute(string handlerSubKey, bool allowMultipleEntries)
        {
            AllowMultipleEntries = allowMultipleEntries;
            HandlerSubKey = handlerSubKey;
        }

        public static HandlerSubKeyAttribute GetHandlerSubKeyAttribute<T>(T value) where T : Enum
        {
            return value.GetAttribute<HandlerSubKeyAttribute>();
        }

        /// <summary>
        /// Gets a value indicating whether multiple entries are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if multiple entries are allowed; otherwise, <c>false</c>.
        /// </value>
        public bool AllowMultipleEntries { get; }

        /// <summary>
        /// Gets the handler sub-key.
        /// </summary>
        /// <value>
        /// The handler sub-key.
        /// </value>
        public string HandlerSubKey { get; }
    }
}
