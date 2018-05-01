using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

namespace SharpShell.Helpers
{
    /// <summary>
    /// A ComStream is a wrapper around the COM IStream interface,
    /// providing direct .NET style access to a COM IStream.
    /// </summary>
    public class ComStream : Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComStream" /> class.
        /// </summary>
        /// <param name="comStream">The source COM stream.</param>
        public ComStream(IStream comStream)
        {
            //  Set the stream.
            this.comStream = comStream;
            bufferPointer = Marshal.AllocCoTaskMem(8);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ComStream"/> class.
        /// </summary>
        ~ComStream()
        {
            Marshal.FreeCoTaskMem(bufferPointer);
        }
        /// <summary>
        /// Releases all resources used by the Com Stream.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            //  Release the underlying stream.
            if (comStream != null)
            {
                Marshal.ReleaseComObject(comStream);
            }
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes 
        /// any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            //  Commit the underlying COM stream.
            comStream.Commit(0);
        }
        
        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset" /> and (<paramref name="offset" /> + <paramref name="count" /> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            //  We don't handle offsets.
            if (offset != 0) 
                throw new NotImplementedException();

            //  Read into the buffer and advance the position.
            comStream.Read(buffer, count, bufferPointer);
            position += count;

            return Marshal.ReadInt32(bufferPointer);
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin" /> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin" /> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            //  Seek the stream.
            comStream.Seek(offset, (int)origin, bufferPointer);

            //  Adjust the position.
            switch (origin)
            {
                case SeekOrigin.Begin: position = offset; break;
                case SeekOrigin.Current: position += offset; break;
                case SeekOrigin.End: position = Length - offset; break;
                default: throw new ArgumentOutOfRangeException("origin");
            }

            return Marshal.ReadInt64(bufferPointer);
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            comStream.SetSize(value);
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count" /> bytes from <paramref name="buffer" /> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer" /> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            //  We don't handle non zero offsets.
            if (offset != 0) 
                throw new NotImplementedException();

            //  Write to the stream and advance the position.
            comStream.Write(buffer, count, IntPtr.Zero);
            position = count;
        }

        /// <summary>
        /// The COM stream instance.
        /// </summary>
        private readonly IStream comStream;

        /// <summary>
        /// The buffer pointer.
        /// </summary>
        private readonly IntPtr bufferPointer;

        /// <summary>
        /// The position of the pointer in the stream.
        /// </summary>
        private long position;

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        public override long Length
        {
            get
            {
                //  Get the statistics of the COM stream, return the size.
                STATSTG stat;
                comStream.Stat(out stat, 1);
                return stat.cbSize;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="System.NotImplementedException">
        /// </exception>
        public override long Position
        {
            get { return position; }
            set { Seek(value, SeekOrigin.Begin); }
        }
    }
}
