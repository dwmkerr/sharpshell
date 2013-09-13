using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace SharpShell
{
    public class ShellStream : Stream
    {

        public ShellStream(IStream shellStream)
        {
            this.shellStream = shellStream;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            shellStream.Commit(0);
        }

        public override long Length
        {
            get
            {
                ulong size;
                IStream_Size(shellStream, out size);
                return (long) size;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        [DllImport("Shlwapi.dll")]
        [PreserveSig]
        private static extern int IStream_Size(IStream pstm, out UInt64 pui);

        private readonly IStream shellStream;
    }
}
