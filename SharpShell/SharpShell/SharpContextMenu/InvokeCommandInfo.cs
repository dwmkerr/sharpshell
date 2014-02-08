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
        public IntPtr WindowHandle { get; internal set; }
        public int ShowCommand { get; internal set; }
    }
}