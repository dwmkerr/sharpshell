using System;

namespace ServerManager.ShellDebugger
{
    public class ShellTreeEventArgs : EventArgs
    {
        public ShellTreeEventArgs(ShellItem shellItem)
        {
            ShellItem = shellItem;
        }

        public ShellItem ShellItem { get; private set; }
    }
}