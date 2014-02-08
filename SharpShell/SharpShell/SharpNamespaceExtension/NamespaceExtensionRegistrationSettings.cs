using System.Drawing;

namespace SharpShell.SharpNamespaceExtension
{
    /// <summary>
    /// The Namespace Extension Registration settings are requested by 
    /// SharpShell to when registering a <see cref="SharpNamespaceExtension "/>.
    /// All configuration is used only during registration time.
    /// </summary>
    public class NamespaceExtensionRegistrationSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceExtensionRegistrationSettings"/> class.
        /// </summary>
        public NamespaceExtensionRegistrationSettings()
        {
            HideFolderVerbs = false;
        }

        /// <summary>
        /// If set to true, indicates that verbs associated with folders in the
        /// system should not appear in the shortcut menu for this extension.
        /// The default value is <c>false</c>.
        /// </summary>
        public bool HideFolderVerbs { get; set; }

        /// <summary>
        /// Flags that indicate how an extension behaves with the system.
        /// By default, none are set.
        /// </summary>
        public AttributeFlags ExtensionAttributes { get; set; }

        /// <summary>
        /// Gets or sets the tooltip.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the assembly icon for the 
        /// namespace icon.
        /// </summary>
        public bool UseAssemblyIcon { get; set; }
    }
}