using System;
using System.Reflection;
using System.Windows.Forms;
using SharpShell.Diagnostics;

namespace SharpShell.Extensions
{
    /// <summary>
    /// Extensions for cotrols.
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Calls the window proc.
        /// </summary>
        /// <param name="me">Me.</param>
        /// <param name="hWnd">The window handle.</param>
        /// <param name="uMessage">The message.</param>
        /// <param name="wParam">The w param.</param>
        /// <param name="lParam">The l param.</param>
        /// <returns>True if the message was handled.</returns>
        public static bool WindowProc(this Control me, IntPtr hWnd, uint uMessage, IntPtr wParam, IntPtr lParam)
        {
            //  Get the wndproc function.
            try
            {

                var methodInfo = me.GetType().GetMethod("WndProc", BindingFlags.Instance | BindingFlags.NonPublic);
                return (bool)methodInfo.Invoke(me, new object[] { new Message { HWnd = hWnd, WParam = wParam, LParam = lParam, Msg = (int)uMessage } });
            }
            catch (Exception exception)
            {
                Logging.Error("Failed to pass to wnproc", exception);
                return false;
            }
        }

        /// <summary>
        /// Finds the control inside this control which has focus, if any.
        /// </summary>
        /// <param name="this">The the parent control.</param>
        /// <returns>The child control with focus, or null.</returns>
        public static Control FindFocusedControl(this Control @this)
        {
            var container = @this as ContainerControl;
            while (container != null)
            {
                @this = container.ActiveControl;
                container = @this as ContainerControl;
            }
            return @this;
        }
    }
}
