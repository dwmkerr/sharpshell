using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Extensions;

namespace SharpShell.ServerRegistration
{
    /// <summary>
    /// Helper class to determine the registration info of specific class.
    /// </summary>
    public class ClassRegistration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClassRegistration"/> class.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        public ClassRegistration(string className)
        {
            ClassName = className;
            lazySpecialRegistryClass = new Lazy<SpecialRegistryClass>(DetermineSpecialRegistryClass);
        }

        /// <summary>
        /// Determines the special registry class.
        /// </summary>
        /// <returns>The special registry class, if any.</returns>
        private SpecialRegistryClass DetermineSpecialRegistryClass()
        {
            //  Create a dictionary of special class key names to special classes.
            Dictionary<string, SpecialRegistryClass> dic = new Dictionary<string, SpecialRegistryClass>();
            foreach (SpecialRegistryClass enumValue in Enum.GetValues(typeof(SpecialRegistryClass)))
            {
                var att = enumValue.GetAttribute<SpecialClassKeyAttribute>();
                if (att != null)
                    dic[att.SpecialClassKey] = enumValue;
            }

            var specialClass = SpecialRegistryClass.None;
            dic.TryGetValue(ClassName, out specialClass);
            return specialClass;
        }

        private readonly Lazy<SpecialRegistryClass> lazySpecialRegistryClass;

        /// <summary>
        /// Gets or sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; set; }
    }
}
