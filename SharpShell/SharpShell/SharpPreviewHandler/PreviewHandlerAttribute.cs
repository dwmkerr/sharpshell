using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.SharpPreviewHandler
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PreviewHandlerAttribute : Attribute
    {
        public PreviewHandlerAttribute()
        {
            //  Set up the defaults.
            DisableLowILProcessIsolation = false; // as recommended on MSDN.
        }

        /// <summary>
        /// Gets the preview handler attribute for a type, if one is set.
        /// If no preview handler is set, null is returned.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The preview handler attribute, or null.</returns>
        public static PreviewHandlerAttribute GetPreviewHandlerAttribute(Type type)
        {
            var previewHandlers = type.GetCustomAttributes(typeof (PreviewHandlerAttribute), false).OfType<PreviewHandlerAttribute>();
            return previewHandlers.SingleOrDefault();
        }

        /// <summary>
        /// Disables low IL process isolation. Default is <c>false</c>.
        /// By default, preview handlers run in a low integrity level (IL) process 
        /// for security reasons. You can optionally disable running as a low IL 
        /// process by setting the following value in the registry. However, it 
        /// is not recommended to do so. Systems could eventually be configured to 
        /// reject any process that is not low IL.
        /// </summary>
        /// <value>
        /// <c>true</c> if low IL process isolation should be disabled; otherwise, <c>false</c>.
        /// </value>
        public bool DisableLowILProcessIsolation { get; set; }
    }
}
