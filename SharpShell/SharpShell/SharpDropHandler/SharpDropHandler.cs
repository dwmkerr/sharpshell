using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.Extensions;
using SharpShell.Interop;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using IDropTarget = SharpShell.Interop.IDropTarget;

namespace SharpShell.SharpDropHandler
{
    /// <summary>
    /// The SharpDropHandler is the base class for SharpShell servers that provide
    /// custom Drop Handlers.
    /// </summary>
    [ServerType(ServerType.ShellDropHandler)]
    public abstract class SharpDropHandler : PersistFileServer, IDropTarget
    {
        #region Implementation of IDropTarget

        /// <summary>
        /// Indicates whether a drop can be accepted, and, if so, the effect of the drop.
        /// </summary>
        /// <param name="pDataObj">A pointer to the IDataObject interface on the data object. This data object contains the data being transferred in the drag-and-drop operation. If the drop occurs, this data object will be incorporated into the target.</param>
        /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Possible values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
        /// <param name="pt">A POINTL structure containing the current cursor coordinates in screen coordinates.</param>
        /// <param name="pdwEffect">On input, pointer to the value of the pdwEffect parameter of the DoDragDrop function. On return, must contain one of the DROPEFFECT flags, which indicates what the result of the drop operation would be.</param>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IDropTarget.DragEnter(IDataObject pDataObj, uint grfKeyState, POINT pt, ref uint pdwEffect)
        {
            //  DebugLog this key event.
            Log(string.Format("Drag Enter for item {0}", SelectedItemPath));

            //  Get the drag items.
            try
            {
                dragItems = pDataObj.GetFileList();
            }
            catch (Exception exception)
            {
                //  DebugLog the error.
                LogError("An exception occured when getting the file list from the data object.", exception);
                return WinError.E_UNEXPECTED;
            }

            //  Create drag event args, which store the provided parameters.
            var dragEventArgs = new DragEventArgs(null, (int) grfKeyState, pt.X, pt.Y, (DragDropEffects) pdwEffect, DragDropEffects.None);

            try
            {
                //  Call the main drag enter function.
                DragEnter(dragEventArgs);
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured during a drag enter event.", exception);

                //  Don't allow any drag effect.
                dragEventArgs.Effect = DragDropEffects.None;
            }

            //  Set the effect.
            pdwEffect = (uint)dragEventArgs.Effect;

            //  Return success.
            return WinError.S_OK;
        }

        /// <summary>
        /// Provides target feedback to the user and communicates the drop's effect to the DoDragDrop function so it can communicate the effect of the drop back to the source.
        /// </summary>
        /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Valid values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
        /// <param name="pt">A POINTL structure containing the current cursor coordinates in screen coordinates.</param>
        /// <param name="pdwEffect">On input, pointer to the value of the pdwEffect parameter of the DoDragDrop function. On return, must contain one of the DROPEFFECT flags, which indicates what the result of the drop operation would be.</param>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IDropTarget.DragOver(uint grfKeyState, POINT pt, ref uint pdwEffect)
        {
            //  Not called for shell extensions.
            return WinError.S_OK;
        }

        /// <summary>
        /// Removes target feedback and releases the data object.
        /// </summary>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IDropTarget.DragLeave()
        {
            //  We don't have cleanup tasks in this case.
            return WinError.S_OK;
        }

        /// <summary>
        /// Incorporates the source data into the target window, removes target feedback, and releases the data object.
        /// </summary>
        /// <param name="pDataObj">A pointer to the IDataObject interface on the data object being transferred in the drag-and-drop operation.</param>
        /// <param name="grfKeyState">The current state of the keyboard modifier keys on the keyboard. Possible values can be a combination of any of the flags MK_CONTROL, MK_SHIFT, MK_ALT, MK_BUTTON, MK_LBUTTON, MK_MBUTTON, and MK_RBUTTON.</param>
        /// <param name="pt">A POINTL structure containing the current cursor coordinates in screen coordinates.</param>
        /// <param name="pdwEffect">On input, pointer to the value of the pdwEffect parameter of the DoDragDrop function. On return, must contain one of the DROPEFFECT flags, which indicates what the result of the drop operation would be.</param>
        /// <returns>
        /// This method returns S_OK on success.
        /// </returns>
        int IDropTarget.Drop(IDataObject pDataObj, uint grfKeyState, POINT pt, ref uint pdwEffect)
        {
            //  DebugLog this key event.
            Log(string.Format("Drop for item {0}", SelectedItemPath));

            //  Create drag event args, which store the provided parameters.
            var dragEventArgs = new DragEventArgs(null, (int)grfKeyState, pt.X, pt.Y, (DragDropEffects)pdwEffect, DragDropEffects.None);

            try
            {
                //  Perform the drop.
                Drop(dragEventArgs);
            }
            catch (Exception exception)
            {
                //  DebugLog the exception.
                LogError("An exception occured during the drop event.", exception);
            }

            //  Return success.
            return WinError.S_OK;
        }

        #endregion

        /// <summary>
        /// Checks what operations are available for dragging onto the target with the drag files.
        /// </summary>
        /// <param name="dragEventArgs">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        protected abstract void DragEnter(DragEventArgs dragEventArgs);

        /// <summary>
        /// Performs the drop.
        /// </summary>
        /// <param name="dragEventArgs">The <see cref="System.Windows.Forms.DragEventArgs"/> instance containing the event data.</param>
        protected abstract void Drop(DragEventArgs dragEventArgs);

        /// <summary>
        /// The set of items being dragged.
        /// </summary>
        private List<string> dragItems = new List<string>();

        /// <summary>
        /// Gets the drag items.
        /// </summary>
        public IEnumerable<string> DragItems { get { return dragItems; } } 
    }
}