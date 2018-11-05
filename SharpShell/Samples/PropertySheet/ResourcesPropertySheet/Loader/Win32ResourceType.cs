using System;
using System.ComponentModel.DataAnnotations;
using SharpShell.Extensions;

namespace ResourcesPropertySheet.Loader
{
    class Win32ResourceType : Win32ResourceName
    {

        public Win32ResourceType(IntPtr resource) : base(resource)
        {
        }

        public override string ToString()
        {
            //  For int resources, we return the known name if possible.
            if (IsInt)
            {
                //  If the resource is a known type...
                if (Enum.IsDefined(typeof(ResType), IntValue))
                {
                    //  ...then return its display name or just the value name.
                    var resType = (ResType)IntValue;
                    var displayName = resType.GetAttribute<DisplayAttribute>();
                    return displayName != null ? displayName.Name : resType.ToString();
                }

                //  If it's not a known type, we just return the int value.
                return $"{IntValue}";
            }

            //  For string resources, if the name starts with a Hash, we use that. Otherwise we quote it. This
            //  is *reasonably* consistent with the visual studio resource viewer.
            return StringValue.StartsWith(@"#") ? StringValue : $"\"{StringValue}\"";
        }

        public bool IsKnownResourceType(ResType resType)
        {
            if (!IsInt) return false;
            return IntValue == (uint)resType;
        }
    }
}