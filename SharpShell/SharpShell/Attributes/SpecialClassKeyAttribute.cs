using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.Attributes
{
    /// <summary>
    /// Allows the special class key to be defined.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SpecialClassKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialClassKeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public SpecialClassKeyAttribute(string key)
        {
            SpecialClassKey = key;
        }

        /// <summary>
        /// Gets the special class key.
        /// </summary>
        /// <value>
        /// The special class key.
        /// </value>
        public string SpecialClassKey { get; private set; }
    }
}
