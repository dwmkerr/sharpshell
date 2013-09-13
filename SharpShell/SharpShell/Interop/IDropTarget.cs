using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace SharpShell.Interop
{
    /// <summary>
    /// The IDropTarget interface is one of the interfaces you implement to provide drag-and-drop operations in your application. It contains methods used in any application that can be a target for data during a drag-and-drop operation. 
    /// </summary>
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00000122-0000-0000-C000-000000000046")]
    public interface IDropTarget
    {
        /// <summary>
        /// Indicates whether a drop can be accepted, and, if so, the effect of the drop.
        /// </summary>
        /// <param name="pDataObj">A pointer to the IDataObject interface on the data object. This data object contains the data being transferred in the drag-and-drop operation. If the drop occurs, this data object will be incorporated into the target.</param>
        /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Possible values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
        /// <param name="pt">A POINTL structure containing the current cursor coordinates in screen coordinates.</param>
        /// <param name="pdwEffect">On input, pointer to the value of the pdwEffect parameter of the DoDragDrop function. On return, must contain one of the DROPEFFECT flags, which indicates what the result of the drop operation would be.</param>
        /// <returns>This method returns S_OK on success.</returns>
        [PreserveSig]
        int DragEnter(IDataObject pDataObj, uint grfKeyState, POINT pt, ref uint pdwEffect);

        /// <summary>
        /// Provides target feedback to the user and communicates the drop's effect to the DoDragDrop function so it can communicate the effect of the drop back to the source.
        /// </summary>
        /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Valid values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
        /// <param name="pt">A POINTL structure containing the current cursor coordinates in screen coordinates.</param>
        /// <param name="pdwEffect">On input, pointer to the value of the pdwEffect parameter of the DoDragDrop function. On return, must contain one of the DROPEFFECT flags, which indicates what the result of the drop operation would be.</param>
        /// <returns>This method returns S_OK on success. </returns>
        [PreserveSig]
        int DragOver(uint grfKeyState, POINT pt, ref uint pdwEffect);

        /// <summary>
        /// Removes target feedback and releases the data object.
        /// </summary>
        /// <returns>This method returns S_OK on success.</returns>
        [PreserveSig]
        int DragLeave();

        /// <summary>
        /// Incorporates the source data into the target window, removes target feedback, and releases the data object.
        /// </summary>
        /// <param name="pDataObj">A pointer to the IDataObject interface on the data object being transferred in the drag-and-drop operation.</param>
        /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Possible values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
        /// <param name="pt">A POINTL structure containing the current cursor coordinates in screen coordinates.</param>
        /// <param name="pdwEffect">On input, pointer to the value of the pdwEffect parameter of the DoDragDrop function. On return, must contain one of the DROPEFFECT flags, which indicates what the result of the drop operation would be.</param>
        /// <returns>This method returns S_OK on success.</returns>
        [PreserveSig]
        int Drop(IDataObject pDataObj, uint grfKeyState, POINT pt, ref uint pdwEffect);
    };
}