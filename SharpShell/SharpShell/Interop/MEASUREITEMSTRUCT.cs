using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Informs the system of the dimensions of an owner-drawn control or menu item. This allows the system to process user interaction with the control correctly.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEASUREITEMSTRUCT
    {
        /// <summary>
        /// The control type. This member can be one of the values shown in the following table.
        /// </summary>
        public uint CtlType;

        /// <summary>
        /// The identifier of the combo box or list box. This member is not used for a menu.
        /// </summary>
        public uint CtlID;

        /// <summary>
        /// The identifier for a menu item or the position of a list box or combo box item. This value is specified for a list box only if it has the LBS_OWNERDRAWVARIABLE style; this value is specified for a combo box only if it has the CBS_OWNERDRAWVARIABLE style.
        /// </summary>
        public uint itemID;

        /// <summary>
        /// The width, in pixels, of a menu item. Before returning from the message, the owner of the owner-drawn menu item must fill this member.
        /// </summary>
        public uint itemWidth;

        /// <summary>
        /// The height, in pixels, of an individual item in a list box or a menu. Before returning from the message, the owner of the owner-drawn combo box, list box, or menu item must fill out this member.
        /// </summary>
        public uint itemHeight;

        /// <summary>
        /// The application-defined value associated with the menu item. For a control, this member specifies the value last assigned to the list box or combo box by the LB_SETITEMDATA or CB_SETITEMDATA message. If the list box or combo box has the LB_HASSTRINGS or CB_HASSTRINGS style, this value is initially zero. Otherwise, this value is initially the value passed to the list box or combo box in the lParam parameter of one of the following messages:
        /// </summary>
        public IntPtr itemData;
    }
}