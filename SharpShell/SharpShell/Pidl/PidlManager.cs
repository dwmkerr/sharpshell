using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
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
        /// <summary>
        /// Decodes the specified pidl.
        /// </summary>
        /// <param name="pidl">The pidl.</param>
        /// <returns>A set of Shell IDs.</returns>
        public static List<ShellId> Decode(IntPtr pidl)
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
                Marshal.Copy(pidl + bytesRead + 2, id, 0, idLength - 2);
                idList.Add(id);
                bytesRead += idLength;
            }

            return idList.Select(id => new ShellId(id)).ToList();
        }

        /// <summary>
        /// Gets the desktop idlist.
        /// </summary>
        /// <returns>The desktop idlist.</returns>
        public static IdList GetDesktop()
        {
            IntPtr pidl;
            Shell32.SHGetKnownFolderIDList(KnownFolders.FOLDERID_Desktop, KNOWN_FOLDER_FLAG.KF_NO_FLAGS, IntPtr.Zero,
                out pidl);
            var idlist = IdList.Create(Decode(pidl));
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
            if(pidl == IntPtr.Zero)
                throw new Exception("Cannot create an ID list from a null pidl.");

            //  Create the raw ID list.
            var ids = Decode(pidl);

            //  Return a new idlist from the pidl.
            return IdList.Create(ids);
        }

        /// <summary>
        /// Converts an array of PIDLs to an IdList array.
        /// </summary>
        /// <param name="apidl">The PIDL array.</param>
        /// <param name="count">The count.</param>
        /// <returns>An IdList array.</returns>
        public static IdList[] APidlToIdListArray(IntPtr apidl, int count)
        {
            var pidls = new IntPtr[count];
            Marshal.Copy(apidl, pidls, 0, count);
            return pidls.Select(PidlToIdlist).ToArray();
        }

        /// <summary>
        /// Converts an IdList to a PIDL.
        /// </summary>
        /// <param name="idList">The identifier list.</param>
        /// <returns>A PIDL</returns>
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
                rawBytes.AddRange(id.RawId);
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

        /// <summary>
        /// Combines two IdLists.
        /// </summary>
        /// <param name="folderIdList">The folder identifier list.</param>
        /// <param name="folderItemIdList">The folder item identifier list.</param>
        /// <returns>A new, combined IdList.</returns>
        public static IdList Combine(IdList folderIdList, IdList folderItemIdList)
        {
            var combined = new List<ShellId>(folderIdList.Ids);
            combined.AddRange(folderItemIdList.Ids);
            return IdList.Create(combined);
        }

        /// <summary>
        /// Deletes the underlying PIDL.
        /// </summary>
        /// <param name="pidl">The PIDL.</param>
        public static void DeletePidl(IntPtr pidl)
        {
            Marshal.FreeCoTaskMem(pidl);
        }

        /// <summary>
        /// Takes a set of PIDLs and creates a PIDL array.
        /// </summary>
        /// <param name="pidls">The PIDLs.</param>
        /// <returns>A PIDL array.</returns>
        public static IntPtr PidlsToAPidl(IntPtr[] pidls)
        {
            var buffer = Marshal.AllocCoTaskMem(pidls.Length*IntPtr.Size);
            Marshal.Copy(pidls, 0, buffer, pidls.Length);
            return buffer;
        }

        /// <summary>
        /// Gets the display name of the PIDL.
        /// </summary>
        /// <param name="pidl">The PIDL.</param>
        /// <returns>The display name of the PIDL.</returns>
        public static string GetPidlDisplayName(IntPtr pidl)
        {
            SHFILEINFO fileInfo = new SHFILEINFO();
            Shell32.SHGetFileInfo(pidl, 0, out fileInfo, (uint)Marshal.SizeOf(fileInfo), SHGFI.SHGFI_PIDL | SHGFI.SHGFI_DISPLAYNAME);
            return fileInfo.szDisplayName;
        }
    }

    /// <summary>
    /// Defines the type of an IdList, which can be relative or absolute.
    /// </summary>
    public enum IdListType
    {
        /// <summary>
        /// The IdList is absolute.
        /// </summary>
        Absolute,

        /// <summary>
        /// The IdList is relative.
        /// </summary>
        Relative
    }
}
