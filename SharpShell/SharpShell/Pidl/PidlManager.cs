using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;

namespace SharpShell.Pidl
{
    /// <summary>
    /// The PidlManager is a class that offers a set of functions for 
    /// working with PIDLs.
    /// </summary>
    /// <remarks>
    /// For more information on PIDLs, please see:
    ///     http://msdn.microsoft.com/en-us/library/windows/desktop/cc144090.aspx
    /// </remarks>
    public static class PidlManager
    {
        public static List<byte[]> Decode(IntPtr pidl)
        {
            //  Pidl is a pointer to an idlist, an idlist is a set of shitemid
            //  structures that have length indicator of two bytes, then the id data.
            //  The whole thing ends with two null bytes.

            //  Storage for the decoded pidl.
            var idList = new List<byte[]>();

            //  Start reading memory, shitemid at at time.
            int bytesRead = 0;
            ushort idLength = 0;
            while((idLength = (ushort)Marshal.ReadInt16(pidl, bytesRead)) != 0)
            {
                //  Read the data.
                var id = new byte[idLength - 2];
                Marshal.Copy(pidl + bytesRead, id, 0, idLength - 2);
                idList.Add(id);
                bytesRead += idLength;
            }

            return idList;
        }
    }

    public sealed class IdList
    {
        private IdList(IdListType type, List<byte[]> ids)
        {
            this.type = type;
            this.ids = ids;
        }

        internal static IdList Create(IdListType type, List<byte[]> ids)
        {
            return new IdList(type, ids);
        }

        private readonly List<byte[]> ids;
        private readonly IdListType type;

        public IdListType Type { get { return type; } }
    }

    public enum IdListType
    {
        Absolute,
        Relative
    }
}
