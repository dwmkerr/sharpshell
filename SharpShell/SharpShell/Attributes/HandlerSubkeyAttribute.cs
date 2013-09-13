using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Attribute to describe handler subkey config.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class HandlerSubkeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerSubkeyAttribute" /> class.
        /// </summary>
        /// <param name="allowMultipleEntries">if set to <c>true</c> [allow multiple entries].</param>
        /// <param name="handlerSubkey">The handler subkey.</param>
        public HandlerSubkeyAttribute(bool allowMultipleEntries, string handlerSubkey)
        {
            AllowMultipleEntries = allowMultipleEntries;
            HandlerSubkey = handlerSubkey;
        }


        /// <summary>
        /// Gets a value indicating whether multiple entries are allowed.
        /// </summary>
        /// <value>
        /// <c>true</c> if multiple entries are allowed; otherwise, <c>false</c>.
        /// </value>
        public bool AllowMultipleEntries { get; private set; }

        /// <summary>
        /// Gets the handler subkey.
        /// </summary>
        /// <value>
        /// The handler subkey.
        /// </value>
        public string HandlerSubkey { get; private set; }
    }
}
