using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Text;
using SharpShell.Interop;

// Notes:
//  http://msdn.microsoft.com/en-us/library/windows/desktop/cc144093.aspx

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

        public static IdList GetDesktop()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Desktop, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idlist = IdList.Create(IdListType.Absolute, Decode(pidl));
            Shell32.ILFree(pidl);
            return idlist;
        }

        /// <summary>
        /// Converts a Win32 PIDL to a <see cref="PidlManager"/> <see cref="IdList"/>.
        /// The PIDL is not freed by the PIDL manager, if it has been allocated by the
        /// shell it is the caller's responsibility to manage it.
        /// </summary>
        /// <param name="pidl">The pidl.</param>
        /// <returns>An <see cref="IdList"/> that corresponds to the PIDL.</returns>
        public static IdList PidlToIdlist(IntPtr pidl)
        {
            //  Create the raw ID list.
            var ids = Decode(pidl);

            //  Determine whether it's relative or absolute.
            var type = IdListType.Absolute; // todo we might be able to simple always specify it in the arugments.-

            //  Return a new idlist from the pidl.
            return IdList.Create(type, ids);
        }

        public static IntPtr IdListToPidl(IdList idList)
        {
            //  Turn the ID list into a set of raw bytes.
            var rawBytes = new List<byte>();

            //  Each item starts with it's length, then the data. The length includes
            //  two bytes, as it counts the length as a short.
            foreach (var id in idList.Ids)
            {
                //  Add the size and data.
                short length = (short)(id.Length + 2);
                rawBytes.AddRange(BitConverter.GetBytes(length));
                rawBytes.AddRange(id);
            }

            //  Write the null termination.
            rawBytes.Add(0);
            rawBytes.Add(0);

            //  Allocate COM memory for the pidl.
            var ptr = Marshal.AllocCoTaskMem(rawBytes.Count);

            //  Copy the raw bytes.
            for (var i = 0; i < rawBytes.Count; i++)
            {
                Marshal.WriteByte(ptr, i, rawBytes[i]);
            }

            //  We've allocated the pidl, copied it and are ready to rock.
            return ptr;
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

        internal List<byte[]> Ids { get { return ids; }}

        public string ToParsingString()
        {
            //  TODO document and validate.
            var sb = new StringBuilder(ids.Sum(id => id.Length*2 + 4));
            foreach (var id in ids)
            {
                sb.AppendFormat("{0:x4}", (short)id.Length);
                foreach(var idi in id)
                    sb.AppendFormat("{0:x2}", idi);
            }

            return sb.ToString();
        }

        public static IdList FromParsingString(string str)
        {
            //  Create the id storage.
            var ids = new List<byte[]>();

            //  Repeatedly read a short length then the data.
            int index = 0;
            while (index < str.Length)
            {
                var length = Convert.ToInt16(str.Substring(index, 4), 16);
                var id = new byte[length];
                index += 4;
                for (var i = 0; i < length; i++, index += 2)
                    id[i] = Convert.ToByte(str.Substring(index, 2), 16);
                ids.Add(id);
            }

            //  Return the list.
            return new IdList(IdListType.Relative, ids);
        }
    }

    public enum IdListType
    {
        Absolute,
        Relative
    }
}
