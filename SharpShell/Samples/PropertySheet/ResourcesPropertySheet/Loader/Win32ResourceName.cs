using System;
using System.Runtime.InteropServices;
using SharpShell.Helpers;

namespace ResourcesPropertySheet.Loader
{
    internal class Win32ResourceName
    {
        private readonly bool _isInt;
        private readonly string _stringValue;
        private readonly uint? _intValue;

        public Win32ResourceName(IntPtr resource)
        {
            Resource = resource;
            _isInt = Win32Helper.IS_INTRESOURCE(resource);
            if (_isInt) _intValue = (uint) resource;
            else _stringValue = Marshal.PtrToStringUni(Resource);
        }

        public bool IsInt => _isInt;

        public IntPtr Resource { get; }

        public uint IntValue
        {
            get
            {
                if (!_isInt) throw new NotSupportedException($"Resource with value {Resource} cannot be safely converted into an ID");
                return _intValue.GetValueOrDefault();
            }
        }

        public string StringValue
        {
            get
            {
                if (_isInt) throw new NotSupportedException($"Resource with value {Resource} cannot be safely converted into a string");
                return _stringValue;
            }
        }

        public override string ToString()
        {
            //  Resources are written as an int value, or a quoted string.
            return IsInt ? $"{IntValue}" : $"\"{StringValue}\"";
        }
    }
}