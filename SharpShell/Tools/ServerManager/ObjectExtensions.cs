using System.ComponentModel;
using System.Linq;

namespace ServerManager
{
    public static class ObjectExtensions
    {
        public static string GetDescription(this object @this)
        {
            return @this.GetType()
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .OfType<DescriptionAttribute>()
                .FirstOrDefault()?.Description;
        }
    }
}
