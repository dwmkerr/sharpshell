using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    [ComImport, Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFolderView
    {
        void GetCurrentViewMode(out uint pViewMode);
        void SetCurrentViewMode(uint ViewMode);
        void GetFolder(ref Guid riid, ref IntPtr ppv);
        void Item(int iItemIndex, out IntPtr ppidl);
        void ItemCount(uint uFlags, out int pcItems);
        void Items(uint uFlags, ref Guid riid, ref IntPtr ppv);
        void GetSelectionMarkedItem(out int piItem);
        void GetFocusedItem(out int piItem);
        void GetItemPosition(IntPtr pidl, out POINT ppt);
        void GetSpacing(out POINT ppt);
        void GetDefaultSpacing(out POINT ppt);
        [PreserveSig]
        int GetAutoArrange();
        void SelectItem(int iItem, uint dwFlags);
        void SelectAndPositionItems(uint cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref POINT apt, uint dwFlags);
    }
}