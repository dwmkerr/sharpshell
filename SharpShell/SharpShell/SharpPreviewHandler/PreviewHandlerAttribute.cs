using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpShell.SharpPreviewHandler
{
    /// <summary>
    /// Defines metadata for Sharp Preview Handlers.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PreviewHandlerAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewHandlerAttribute"/> class.
        /// </summary>
        public PreviewHandlerAttribute()
        {
            //  Set up the defaults.
            DisableLowILProcessIsolation = false;                       //  as recommended on MSDN.
            SurrogateHostType = SurrogateHostType.DedicatedPrevhost;    //  safest option.
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

        /// <summary>
        /// Gets or sets the surrogate host type. Changing this from
        /// the default of SurrogateHostType.DedicatedPrevhost is not recommended.
        /// </summary>
        /// <value>
        /// The surrogate host type.
        /// </value>
        public SurrogateHostType SurrogateHostType { get; set; }
    }

    /// <summary>
    /// Defines what sort of Surrogate Host will be used to host a preview handler.
    /// </summary>
    public enum SurrogateHostType
    {
        /// <summary>
        /// A dedicated Prevhost.exe process will be used for all instances of this preview
        /// handler. This is the safest option as conflicts with .NET Framework versions
        /// cannot happen.
        /// </summary>
        DedicatedPrevhost = 0,

        /// <summary>
        /// The standard Prevhost.exe surrogate will be used. Not recommended.
        /// </summary>
        Prevhost = 1,

        /// <summary>
        /// A dedicated Prevhost.exe surrogate suitable for hosting 32 bit preview
        /// handlers on 64 systems will be used. Not recommended.
        /// </summary>
        Prevhost32On64 = 2
    }
}
