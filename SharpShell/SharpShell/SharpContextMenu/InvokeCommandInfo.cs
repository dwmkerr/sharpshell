using System;

namespace SharpShell.SharpContextMenu
{
    /// <summary>
    /// The InvokeCommandInfo class stores data about an invoked command. It is typically
    /// retrieved in a <see cref="SharpContextMenu"/> derived class for advanced menu#
    /// functionality.
    /// </summary>
    public class InvokeCommandInfo
    {
        /// <summary>
        /// Gets the window handle.
        /// </summary>
        /// <value>
        /// The window handle.
        /// </value>
        public IntPtr WindowHandle { get; internal set; }

        /// <summary>
        /// Gets the show command.
        /// </summary>
        /// <value>
        /// The show command.
        /// </value>
        public int ShowCommand { get; internal set; }
    }
}