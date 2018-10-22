using System;
using SharpShell.Interop;
#pragma warning disable 1591

namespace SharpShell.SharpNamespaceExtension
{
    // TODO work in progress. document when API is fixed.
    public class PropertyKey
    {
        private readonly Guid _formatId;
        private readonly uint _propertyId;

        public PropertyKey(StandardPropertyKey standardPropertyKey)
        {
            PropertyKeyAttribute.GetShellPropertyKey(standardPropertyKey, out _formatId, out _propertyId);
        }

        public PropertyKey(Guid formatId, uint propertyId)
        {
            this._formatId = formatId;
            this._propertyId = propertyId;
        }

        public bool Equals(StandardPropertyKey standardPropertyKey)
        {
            Guid formatId;
            uint propertyId;
            PropertyKeyAttribute.GetShellPropertyKey(standardPropertyKey, out formatId, out propertyId);
            return _formatId == formatId && _propertyId == propertyId;
        }

        /// <summary>
        /// Gets the format identifier.
        /// </summary>
        /// <value>
        /// The format identifier.
        /// </value>
        public Guid FormatId => _formatId;

        /// <summary>
        /// Gets the property identifier.
        /// </summary>
        /// <value>
        /// The property identifier.
        /// </value>
        public uint PropertyId => _propertyId;

        public PropertyKey(uint standardPropertyKey, int i, int i1, int i2, int i3, int i4, int i5, int i6, int i7, int i8, int i9, int i10)
        {
            throw new NotImplementedException();
        }

        protected internal Interop.PROPERTYKEY CreateShellPropertyKey()
        {
            return new PROPERTYKEY
            {
                fmtid = _formatId,
                pid = _propertyId
            };
        }
    }
}