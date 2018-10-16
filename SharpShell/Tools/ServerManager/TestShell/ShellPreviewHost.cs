using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Forms;
using SharpShell.Interop;

namespace ServerManager.TestShell
{
    public partial class ShellPreviewHost : UserControl
    {
        public ShellPreviewHost()
        {
            InitializeComponent();
        }

        public void SetPreviewHandler(Guid previewHandlerGuid)
        {
            //  Try and create an instance of the preview handler.
            var previewHandlerType = Type.GetTypeFromCLSID(previewHandlerGuid);
            previewHandler = Activator.CreateInstance(previewHandlerType);
        }

        public void SetPreviewItem(string path)
        {

            if (previewHandler is IInitializeWithFile)
            {
                ((IInitializeWithFile)previewHandler).Initialize(path, 0);
            }
            else if (previewHandler is IInitializeWithStream)
            {
                var managedStream = File.OpenRead(path);
                var stream = new StreamWrapper(managedStream);
                ((IInitializeWithStream)previewHandler).Initialize(stream, 0);
            }

            if (previewHandler is IPreviewHandler)
            {
                RECT rect = new RECT() {bottom = ClientRectangle.Bottom, left = ClientRectangle.Left, right = ClientRectangle.Right, top = ClientRectangle.Top};
                ((IPreviewHandler)previewHandler).SetWindow(Handle, rect);
                ((IPreviewHandler)previewHandler).DoPreview();
            }
        }

        private object previewHandler;

        private void ShellPreviewHost_Resize(object sender, EventArgs e)
        {
            //  Do we habe a preview handler?
            if (previewHandler != null && previewHandler is IPreviewHandler)
            {
                var bounds = new RECT
                {
                    bottom = ClientRectangle.Bottom,
                    left = ClientRectangle.Left,
                    right = ClientRectangle.Right,
                    top = ClientRectangle.Top
                };
                ((IPreviewHandler) previewHandler).SetRect(ref bounds);
            }
        }
    }

    /// <summary>
    /// Provides a bare-bones implementation of System.Runtime.InteropServices.IStream that wraps an System.IO.Stream.
    /// </summary>
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    internal class StreamWrapper : IStream
    {

        private System.IO.Stream mInner;

        /// <summary>
        /// Initialises a new instance of the StreamWrapper class, using the specified System.IO.Stream.
        /// </summary>
        /// <param name="inner"></param>
        public StreamWrapper(System.IO.Stream inner)
        {
            mInner = inner;
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="ppstm"></param>
        public void Clone(out IStream ppstm)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="grfCommitFlags"></param>
        public void Commit(int grfCommitFlags)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="pstm"></param>
        /// <param name="cb"></param>
        /// <param name="pcbRead"></param>
        /// <param name="pcbWritten"></param>
        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="libOffset"></param>
        /// <param name="cb"></param>
        /// <param name="dwLockType"></param>
        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Reads a sequence of bytes from the underlying System.IO.Stream.
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="cb"></param>
        /// <param name="pcbRead"></param>
        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            long bytesRead = mInner.Read(pv, 0, cb);
            if (pcbRead != IntPtr.Zero) Marshal.WriteInt64(pcbRead, bytesRead);
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        public void Revert()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Advances the stream to the specified position.
        /// </summary>
        /// <param name="dlibMove"></param>
        /// <param name="dwOrigin"></param>
        /// <param name="plibNewPosition"></param>
        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            long pos = mInner.Seek(dlibMove, (System.IO.SeekOrigin)dwOrigin);
            if (plibNewPosition != IntPtr.Zero) Marshal.WriteInt64(plibNewPosition, pos);
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="libNewSize"></param>
        public void SetSize(long libNewSize)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns details about the stream, including its length, type and name.
        /// </summary>
        /// <param name="pstatstg"></param>
        /// <param name="grfStatFlag"></param>
        public void Stat(out System.Runtime.InteropServices.ComTypes.STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new System.Runtime.InteropServices.ComTypes.STATSTG();
            pstatstg.cbSize = mInner.Length;
            pstatstg.type = 2; // stream type
            pstatstg.pwcsName = (mInner is FileStream) ? ((FileStream)mInner).Name : String.Empty;
        }

        /// <summary>
        /// This operation is not supported.
        /// </summary>
        /// <param name="libOffset"></param>
        /// <param name="cb"></param>
        /// <param name="dwLockType"></param>
        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Writes a sequence of bytes to the underlying System.IO.Stream.
        /// </summary>
        /// <param name="pv"></param>
        /// <param name="cb"></param>
        /// <param name="pcbWritten"></param>
        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            mInner.Write(pv, 0, cb);
            if (pcbWritten != IntPtr.Zero) Marshal.WriteInt64(pcbWritten, (Int64)cb);
        }
    }
}
