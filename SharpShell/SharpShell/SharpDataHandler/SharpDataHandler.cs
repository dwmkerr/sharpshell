using System;
using System.Windows.Forms;
using SharpShell.Attributes;
using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

namespace SharpShell.SharpDataHandler
{
    /// <summary>
    /// The SharpDataHandler is the base class for SharpShell servers that provide
    /// custom Icon Handlers.
    /// </summary>
    [ServerType(ServerType.ShellDataHandler)]
    public abstract class SharpDataHandler : PersistFileServer, IDataObject
    {
        #region Implementation of IDataObject

        int IDataObject.DAdvise(ref System.Runtime.InteropServices.ComTypes.FORMATETC pFormatetc, System.Runtime.InteropServices.ComTypes.ADVF advf, System.Runtime.InteropServices.ComTypes.IAdviseSink adviseSink, out int connection)
        {
            //  DebugLog key events.
            Log("IDataObject.DAdvise called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        void IDataObject.DUnadvise(int connection)
        {
            //  DebugLog key events.
            Log("IDataObject.DUnadvise called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        int IDataObject.EnumDAdvise(out System.Runtime.InteropServices.ComTypes.IEnumSTATDATA enumAdvise)
        {
            //  DebugLog key events.
            Log("IDataObject.EnumDAdvise called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        System.Runtime.InteropServices.ComTypes.IEnumFORMATETC IDataObject.EnumFormatEtc(System.Runtime.InteropServices.ComTypes.DATADIR direction)
        {
            //  DebugLog key events.
            Log("IDataObject.EnumFormatEtc called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        int IDataObject.GetCanonicalFormatEtc(ref System.Runtime.InteropServices.ComTypes.FORMATETC formatIn, out System.Runtime.InteropServices.ComTypes.FORMATETC formatOut)
        {
            //  DebugLog key events.
            Log("IDataObject.GetCanonicalFormatEtc called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        void IDataObject.GetData(ref System.Runtime.InteropServices.ComTypes.FORMATETC format, out System.Runtime.InteropServices.ComTypes.STGMEDIUM medium)
        {
            //  DebugLog key events.
            Log("IDataObject.GetData called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        void IDataObject.GetDataHere(ref System.Runtime.InteropServices.ComTypes.FORMATETC format, ref System.Runtime.InteropServices.ComTypes.STGMEDIUM medium)
        {
            //  DebugLog key events.
            Log("IDataObject.GetDataHere called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        int IDataObject.QueryGetData(ref System.Runtime.InteropServices.ComTypes.FORMATETC format)
        {
            //  DebugLog key events.
            Log("IDataObject.QueryGetData called.");

            //  Not needed for Shell Data Handlers.
            throw new NotImplementedException();
        }

        void IDataObject.SetData(ref System.Runtime.InteropServices.ComTypes.FORMATETC formatIn, ref System.Runtime.InteropServices.ComTypes.STGMEDIUM medium, bool release)
        {
            //  DebugLog key events.
            Log("IDataObject.SetData called.");

            try
            {
                //  Let the derived class provide data.
                var itemData = GetData();

                //  Set the data.
                ((IDataObject)itemData).SetData(ref formatIn, ref medium, release);
            }
            catch (Exception exception)
            {
                LogError("An exception occured getting data for the item " + SelectedItemPath, exception);
            }
        }

        #endregion

        /// <summary>
        /// Gets the data for the selected item. The selected item's path is stored in the SelectedItemPath property.
        /// </summary>
        /// <returns>The data for the selected item, or null if there is none.</returns>
        protected abstract DataObject GetData();
    }
}