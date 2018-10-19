using System;
using System.Linq;
using System.Text;

namespace SharpShell.Pidl
{
    /// <summary>
    /// A ShellId is a representation of a shell item that makes up an id list.
    /// Simply put, a ShellId names a Shell Item uniquely in the context of it's
    /// parent Shell Folder.
    /// </summary>
    public class ShellId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellId"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        internal ShellId(byte[] id)
        {
            this.id = id;
        }

        /// <summary>
        /// Creates a new Shell ID from a string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>A new Shell ID from the given string.</returns>
        public static ShellId FromString(string str)
        {
            return new ShellId(Encoding.UTF8.GetBytes(str));
        }

        /// <summary>
        /// Creates a Shell ID frm raw data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>A new Shell ID from the given data.</returns>
        /// <exception cref="System.NullReferenceException">'data' cannot be null.</exception>
        /// <exception cref="System.InvalidOperationException">'data' must contain data.</exception>
        public static ShellId FromData(byte[] data)
        {
            if(data == null)
                throw new NullReferenceException("'data' cannot be null.");
            if(data.Length == 0)
                throw new InvalidOperationException("'data' must contain data.");

            return new ShellId(data);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            //  Write the bytes of the ID and the string if we can get one.

            var bytesString = BitConverter.ToString(id.ToArray());
            string ascii, utf8;
            try
            {
                ascii = Encoding.ASCII.GetString(id);
            }
            catch
            {
                ascii = "[Undecodable]";
            }
            try
            {
                utf8 = Encoding.UTF8.GetString(id.ToArray());
            }
            catch
            {
                utf8 = "[Undecodable]";
            }

            return string.Format("{0} ASCII {1} UTF8 {2}", bytesString, ascii, utf8);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            var rhs = obj as ShellId;
            if (rhs == null)
                return false;
            if (Length != rhs.Length)
                return false;
            for(int i=0;i<Length;i++)
                if (id[i] != rhs.id[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        /// <summary>
        /// Gets the raw identifier.
        /// </summary>
        public byte[] RawId { get { return id; }}

        /// <summary>
        /// Gets the length of the identifier.
        /// </summary>
        public int Length {get { return id.Length; }}

        /// <summary>
        /// The identifier.
        /// </summary>
        private readonly byte[] id;

        /// <summary>
        /// Gets the ShellId as a UFT8 string.
        /// </summary>
        /// <returns>The ShellId as a UFT8 string.</returns>
        public string AsUTF8()
        {
            var utf8 = string.Empty;
            try
            {
                utf8 = Encoding.UTF8.GetString(id.ToArray());
            }
            catch
            {
                utf8 = "[Undecodable]";
            }
            return utf8;
        }

        /// <summary>
        /// Gets the IdList as an ASCII string.
        /// </summary>
        /// <returns>The IdList as an ASCII string.</returns>
        public string AsASCII()
        {
            var utf8 = string.Empty;
            try
            {
                utf8 = Encoding.ASCII.GetString(id.ToArray());
            }
            catch
            {
                utf8 = "[Undecodable]";
            }
            return utf8;
        }
    }
}