namespace SharpShell.Configuration
{
    /// <summary>
    /// Represents SharpShell system configuration.
    /// Load and save system configuration via the <see cref="SystemConfigurationProvider"/>.
    /// </summary>
    public class SystemConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfiguration"/> class.
        /// Only ever created by <see cref="SystemConfigurationProvider"/>.
        /// </summary>
        internal SystemConfiguration()
        {
        }

        /// <summary>
        /// Gets a value indicating whether configuration is present.
        /// This is a purely informational value that indicates whether
        /// there is SharpShell configuration in the registry.
        /// </summary>
        /// <value>
        /// <c>true</c> if configuration is present; otherwise, <c>false</c>.
        /// </value>
        public bool IsConfigurationPresent { get; internal set; }

        /// <summary>
        /// Gets or sets the logging mode.
        /// </summary>
        /// <value>
        /// The logging mode.
        /// </value>
        public LoggingMode LoggingMode { get; set; }

        /// <summary>
        /// Gets or sets the log path.
        /// Only used if <see cref="LoggingMode"/> is set to File.
        /// </summary>
        /// <value>
        /// The log path.
        /// </value>
        public string LogPath { get; set; }
    }
}