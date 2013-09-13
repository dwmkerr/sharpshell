using System;

namespace SharpShell.Interop
{
    /// <summary>
    /// Contains information about a notification message.
    /// </summary>
    public struct NMHDR
    {
        /// <summary>
        /// A window handle to the control sending the message.
        /// </summary>
        public IntPtr hwndFrom;

        /// <summary>
        /// An identifier of the control sending the message.
        /// </summary>
        public UIntPtr idFrom;

        /// <summary>
        /// A notification code. This member can be one of the common notification codes (see Notifications under General Control Reference), or it can be a control-specific notification code.
        /// </summary>
        public uint code;
    }

    /// <summary>
    /// The notify struct for Property Sheet notifications.
    /// </summary>
    public struct PSHNOTIFY
    {
        /// <summary>
        /// The header.
        /// </summary>
        public NMHDR hdr;
        
        /// <summary>
        /// The lparam
        /// </summary>
        public IntPtr lParam;
    };
}