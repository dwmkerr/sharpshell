using System.Windows.Forms;

namespace SharpShell.SharpDeskBand
{
    /// <summary>
    /// Represents the band options.
    /// </summary>
    public class BandOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show a band title.
        /// </summary>
        /// <value>
        ///   <c>true</c> if a title should be shown; otherwise, <c>false</c>.
        /// </value>
        public bool ShowTitle { get; set; }

        /// <summary>
        /// Gets or sets the title. Only shown if <see cref="ShowTitle"/> is <c>true</c>
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the band has variable height.
        /// </summary>
        /// <value>
        /// <c>true</c> if the band has variable height; otherwise, <c>false</c>.
        /// </value>
        public bool HasVariableHeight { get; set; }

        /// <summary>
        /// Gets or sets the vertical sizing increment in pixels. Only used if <see cref="HasVariableHeight"/>
        /// is <c>true</c>.
        /// </summary>
        public uint VerticalSizingIncrement { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the background colour of the Band UI.
        /// If true, <see cref="Control.BackColor"/> is used as the background colour.
        /// </summary>
        public bool UseBackgroundColour { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the band has a sunken border.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the band has a sunken border; otherwise, <c>false</c>.
        /// </value>
        public bool IsSunken { get; set; }
    }
}