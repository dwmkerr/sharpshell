using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Contains information about a button in a toolbar.
    /// TODO: This struct has different sizes on Windows x86 and x64, validate that it works in both modes.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TBBUTTON 
    {
        /// <summary>
        /// Zero-based index of the button image. Set this member to I_IMAGECALLBACK, and the toolbar will send the TBN_GETDISPINFO notification code to retrieve the image index when it is needed.
        /// Version 5.81. Set this member to I_IMAGENONE to indicate that the button does not have an image. The button layout will not include any space for a bitmap, only text.
        /// If the button is a separator, that is, if fsStyle is set to BTNS_SEP, iBitmap determines the width of the separator, in pixels. For information on selecting button images from image lists, see TB_SETIMAGELIST message.
        /// </summary>
        public int iBitmap;

        /// <summary>
        /// Command identifier associated with the button. This identifier is used in a WM_COMMAND message when the button is chosen.
        /// </summary>
        public int idCommand;

        [StructLayout(LayoutKind.Explicit)]
        private struct TBBUTTON_U {
            [FieldOffset(0)] public byte fsState;
            [FieldOffset(1)] public byte fsStyle;
            [FieldOffset(0)] private IntPtr bReserved;
        }
        private TBBUTTON_U union;

        /// <summary>
        /// Button state flags. This member can be a combination of the values listed in Toolbar Button States.
        /// </summary>
        public byte fsState { get { return union.fsState; } set { union.fsState = value; } }

        /// <summary>
        /// Button style. This member can be a combination of the button style values listed in Toolbar Control and Button Styles.
        /// </summary>
        public byte fsStyle { get { return union.fsStyle; } set { union.fsStyle = value; } }

        /// <summary>
        /// Application-defined value.
        /// </summary>
        public UIntPtr dwData;

        /// <summary>
        /// 
        /// </summary>
        public IntPtr iString;
    }

    // ReSharper restore InconsistentNaming
}