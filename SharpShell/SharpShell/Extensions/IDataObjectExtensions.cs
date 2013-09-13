using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using SharpShell.Interop;

namespace SharpShell.Extensions
{
    /// <summary>
    /// Extensions for the IDataObject interface.
    /// </summary>
    public static class IDataObjectExtensions
    {
        /// <summary>
        /// Gets the file list.
        /// </summary>
        /// <param name="this">The IDataObject instance.</param>
        /// <returns>The file list in the data object.</returns>
        public static List<string> GetFileList(this IDataObject @this)
        {
            //  Create the format object.
            var formatEtc = new FORMATETC();

            //  Set up the format object, based on CF_HDROP.
            formatEtc.cfFormat = (short)CLIPFORMAT.CF_HDROP;
            formatEtc.ptd = IntPtr.Zero;
            formatEtc.dwAspect = DVASPECT.DVASPECT_CONTENT;
            formatEtc.lindex = -1;
            formatEtc.tymed = TYMED.TYMED_HGLOBAL;

            //  Get the data.
            // ReSharper disable RedundantAssignment
            var storageMedium = new STGMEDIUM();
            // ReSharper restore RedundantAssignment
            @this.GetData(ref formatEtc, out storageMedium);

            var fileList = new List<string>();

            //  Things can get risky now.
            try
            {
                //  Get the handle to the HDROP.
                var hDrop = storageMedium.unionmember;
                if (hDrop == IntPtr.Zero)
                    throw new ArgumentException("Failed to get the handle to the drop data.");

                //  Get the count of the files in the operation.
                var fileCount = Shell32.DragQueryFile(hDrop, UInt32.MaxValue, null, 0);

                //  Go through each file.
                for (uint i = 0; i < fileCount; i++)
                {
                    //  Storage for the file name.
                    var fileNameBuilder = new StringBuilder(MaxPath);

                    //  Get the file name.
                    var result = Shell32.DragQueryFile(hDrop, i, fileNameBuilder, (uint)fileNameBuilder.Capacity);

                    //  If we have a valid result, add the file name to the list of files.
                    if (result != 0)
                        fileList.Add(fileNameBuilder.ToString());
                }

            }
            finally
            {
                Ole32.ReleaseStgMedium(ref storageMedium);
            }

            return fileList;
        }

        private const int MaxPath = 260;
    }
}
