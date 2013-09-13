using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using Apex.WinForms.Shell;
using SharpShell.Interop;
using SharpShell.SharpThumbnailHandler;

namespace ServerManager.TestShell
{
    public partial class ShellThumbnailHost : UserControl
    {
        public ShellThumbnailHost()
        {
            InitializeComponent();
        }

        public void SetThumbnailHandler(SharpThumbnailHandler testThumbnailHandler)
        {
            sharpThumbnailHandler = testThumbnailHandler;
        }

        public void SetPreviewItem(ShellItem shellItem)
        {
            //  Create a stream for the item.
            using (var managedStream = new FileStream(shellItem.Path, FileMode.Open, FileAccess.Read))
            {
                var stream = new StreamWrapper(managedStream);
                ((IInitializeWithStream) sharpThumbnailHandler).Initialize(stream, 0);
                IntPtr bitmapHandle;
                WTS_ALPHATYPE alphaType;
                ((IThumbnailProvider) sharpThumbnailHandler).GetThumbnail((uint)pictureBox1.Width, out bitmapHandle, out alphaType);
                pictureBox1.Image = Image.FromHbitmap(bitmapHandle);
                /*
                var bitmap = (Bitmap) sharpThumbnailHandler.GetType().GetMethod("GetThumbnailImage", BindingFlags.Instance | BindingFlags.NonPublic)
                                                           .Invoke(sharpThumbnailHandler, new object[] { (uint)pictureBox1.Width });

                var hbmp = bitmap.GetHbitmap();
                var converted = Image.FromHbitmap(hbmp);
                pictureBox1.Image = converted;*/
            }
        }

        private SharpThumbnailHandler sharpThumbnailHandler;
    }
}
