using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Attributes;
using SharpShell.Extensions;

namespace SharpShell.ServerRegistration
{
    public class ClassRegistration
    {
        public ClassRegistration(string className)
        {
            ClassName = className;
            lazySpecialRegistryClass = new Lazy<SpecialRegistryClass>(DetermineSpecialRegistryClass);
        }

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
        
        public string ClassName { get; set; }
    }
}
