using System;
using System.Linq;
#pragma warning disable 1591

namespace SharpShell.SharpNamespaceExtension
{
    // todo document when api is locked down.
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PropertyKeyAttribute : Attribute
    {
        public PropertyKeyAttribute(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k, uint propertyId)
        {
            formatId = new  Guid(a, b, c, d, e, f, g, h, i, j, k);
            this.propertyId = propertyId;
        }


        public PropertyKeyAttribute(string formatId, uint propertyId)
        {
            this.formatId = new Guid(formatId);
            this.propertyId = propertyId;
        }

        internal static void GetShellPropertyKey(Enum @enum, out Guid formatId, out uint propertyId)
        {
            var attribute = @enum.GetType()
                                 .GetField(@enum.ToString())
                                 .GetCustomAttributes(typeof(PropertyKeyAttribute), true)
                                 .OfType<PropertyKeyAttribute>()
                                 .Single();
            formatId = attribute.formatId;
            propertyId = attribute.propertyId;
        }

        private readonly Guid formatId;
        private readonly uint propertyId;
        

    }
}