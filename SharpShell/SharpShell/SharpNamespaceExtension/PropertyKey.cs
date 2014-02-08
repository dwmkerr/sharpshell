using System;
using SharpShell.Interop;

namespace SharpShell.SharpNamespaceExtension
{
    public class PropertyKey
    {
        public PropertyKey(StandardPropertyKey standardPropertyKey)
        {
            PropertyKeyAttribute.GetShellPropertyKey(standardPropertyKey, out formatId, out propertyId);
        }

        public PropertyKey(Guid formatId, uint propertyId)
        {
            this.formatId = formatId;
            this.propertyId = propertyId;
        }

        private readonly Guid formatId;
        private readonly uint propertyId;

        public PropertyKey(uint standardPropertyKey, int i, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10)
        {
            throw new NotImplementedException();
        }

        protected internal Interop.PROPERTYKEY CreateShellPropertyKey()
        {
            return new PROPERTYKEY
            {
                fmtid = formatId,
                pid = propertyId
            };
        }
    }
}