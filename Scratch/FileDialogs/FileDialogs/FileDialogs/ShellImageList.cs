using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FileDialogs
{
    internal enum ShellImageListSize
    {
        Small,
        Large
    }

    internal static class ShellImageList
    {
        #region Member Fields

        private static NativeMethods.IShellFolder m_desktopFolder;
        private static IntPtr m_smallImageListHandle;
        private static IntPtr m_largeImageListHandle;
        private static Hashtable m_imageTable;

        #endregion

        #region Construction

        static ShellImageList()
        {
            IntPtr desktopFolderPtr;
            NativeMethods.Shell32.SHGetDesktopFolder(out desktopFolderPtr);
            m_desktopFolder = (NativeMethods.IShellFolder)Marshal.GetObjectForIUnknown(desktopFolderPtr);

            m_imageTable = new Hashtable();

            uint flag = NativeMethods.SHGFI_USEFILEATTRIBUTES | NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_SMALLICON;
            NativeMethods.SHFILEINFO shfiSmall = new NativeMethods.SHFILEINFO();
            m_smallImageListHandle = NativeMethods.Shell32.SHGetFileInfo(".txt", NativeMethods.FILE_ATTRIBUTE_NORMAL, ref shfiSmall, Marshal.SizeOf(shfiSmall), flag);

            flag = NativeMethods.SHGFI_USEFILEATTRIBUTES | NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_LARGEICON;
            NativeMethods.SHFILEINFO shfiLarge = new NativeMethods.SHFILEINFO();
            m_largeImageListHandle = NativeMethods.Shell32.SHGetFileInfo(".txt", NativeMethods.FILE_ATTRIBUTE_NORMAL, ref shfiLarge, Marshal.SizeOf(shfiLarge), flag);
        }

        #endregion

        #region Methods

        internal static int GetIconIndex(IntPtr pidl, bool selectedIcon, bool openIcon)
        {
            NativeMethods.SHFILEINFO info = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(pidl, 0, ref info, NativeMethods.cbFileInfo, NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_SYSICONINDEX);
            int index = info.iIcon;

            bool hasOverlay = false; // true if it's an overlay
            int rVal; // The returned Index

            IntPtr[] pidls = new IntPtr[] { pidl };
            NativeMethods.SHGFAO attribs = NativeMethods.SHGFAO.SFGAO_LINK |
                                           NativeMethods.SHGFAO.SFGAO_SHARE |
                                           NativeMethods.SHGFAO.SFGAO_HIDDEN |
                                           NativeMethods.SHGFAO.SFGAO_FILESYSTEM |
                                           NativeMethods.SHGFAO.SFGAO_FOLDER;
            m_desktopFolder.GetAttributesOf(1, pidls, ref attribs);

            uint dwflag = NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ICON;
            int dwAttr = 0;
            // Build Key into HashTable for this Item
            int key = index * 256;
            if ((attribs & NativeMethods.SHGFAO.SFGAO_LINK) != 0)
            {
                key = key | 1;
                dwflag = dwflag | NativeMethods.SHGFI_LINKOVERLAY;
                hasOverlay = true;
            }
            if ((attribs & NativeMethods.SHGFAO.SFGAO_SHARE) != 0)
            {
                key = key | 2;
                dwflag = dwflag | NativeMethods.SHGFI_ADDOVERLAYS;
                hasOverlay = true;
            }
            if (selectedIcon)
            {
                key = key | 4;
                dwflag = dwflag | NativeMethods.SHGFI_SELECTED;
                hasOverlay = true;
            }
            if (openIcon)
            {
                key = key | 8;
                dwflag = dwflag | NativeMethods.SHGFI_OPENICON;
                hasOverlay = true; // Not really an overlay, but handled the same
            }

            if (m_imageTable.ContainsKey(key))
                rVal = (int)m_imageTable[key];
            else if (!hasOverlay && (attribs & NativeMethods.SHGFAO.SFGAO_HIDDEN) == 0) // For non-overlay icons, we already have
            {
                rVal = (int)Math.Floor((double)key / 256); // the right index -- put in table
                m_imageTable[key] = rVal;
            }
            else // Don't have iconindex for an overlay, get it.
            {
                //item.m_isDisk = (item.m_path.Length == 3 && item.m_path.EndsWith(":\\"));
                if ((attribs & NativeMethods.SHGFAO.SFGAO_FILESYSTEM) != 0 &&
                    (attribs & NativeMethods.SHGFAO.SFGAO_FOLDER) == 0)
                {
                    dwflag = dwflag | NativeMethods.SHGFI_USEFILEATTRIBUTES;
                    dwAttr = dwAttr | NativeMethods.FILE_ATTRIBUTE_NORMAL;
                }

                NativeMethods.SHFILEINFO shfiSmall = new NativeMethods.SHFILEINFO();
                NativeMethods.Shell32.SHGetFileInfo(pidl, dwAttr, ref shfiSmall, Marshal.SizeOf(shfiSmall), dwflag | NativeMethods.SHGFI_SMALLICON);

                NativeMethods.SHFILEINFO shfiLarge = new NativeMethods.SHFILEINFO();
                NativeMethods.Shell32.SHGetFileInfo(pidl, dwAttr, ref shfiLarge, Marshal.SizeOf(shfiLarge), dwflag | NativeMethods.SHGFI_LARGEICON);

                lock (m_imageTable)
                {
                    rVal = NativeMethods.ComCtl32.ImageList_ReplaceIcon(m_smallImageListHandle, -1, shfiSmall.hIcon);
                    NativeMethods.ComCtl32.ImageList_ReplaceIcon(m_largeImageListHandle, -1, shfiLarge.hIcon);
                }

                NativeMethods.User32.DestroyIcon(shfiSmall.hIcon);
                NativeMethods.User32.DestroyIcon(shfiLarge.hIcon);
                m_imageTable[key] = rVal;
            }

            return rVal;
        }

        public static Image GetSmallImage(SpecialFolder specialFolder)
        {
            uint dwflag = NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ICON;
            int dwAttr = 0;

            IntPtr pidl;
            NativeMethods.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, (int)specialFolder, out pidl);

            NativeMethods.SHFILEINFO shfi = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(pidl, dwAttr, ref shfi, Marshal.SizeOf(shfi), dwflag);

            IntPtr iconPtr = NativeMethods.ComCtl32.ImageList_GetIcon(m_smallImageListHandle, shfi.iIcon, NativeMethods.ILD_NORMAL);

            return Icon.FromHandle(iconPtr).ToBitmap();
        }

        public static Image GetImage(SpecialFolder specialFolder)
        {
            uint dwflag = NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ICON;
            int dwAttr = 0;

            IntPtr pidl;
            NativeMethods.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, (int)specialFolder, out pidl);

            NativeMethods.SHFILEINFO shfi = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(pidl, dwAttr, ref shfi, Marshal.SizeOf(shfi), dwflag);

            IntPtr iconPtr = NativeMethods.ComCtl32.ImageList_GetIcon(m_largeImageListHandle, shfi.iIcon, NativeMethods.ILD_NORMAL);

            return Icon.FromHandle(iconPtr).ToBitmap();
        }

        public static Image GetSmallImage(IntPtr pidl)
        {
            uint dwflag = NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ICON;
            int dwAttr = 0;

            NativeMethods.SHFILEINFO shfi = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(pidl, dwAttr, ref shfi, Marshal.SizeOf(shfi), dwflag);

            IntPtr iconPtr = NativeMethods.ComCtl32.ImageList_GetIcon(m_smallImageListHandle, shfi.iIcon, NativeMethods.ILD_NORMAL);

            return Icon.FromHandle(iconPtr).ToBitmap();
        }

        public static Image GetImage(IntPtr pidl)
        {
            uint dwflag = NativeMethods.SHGFI_SYSICONINDEX | NativeMethods.SHGFI_PIDL | NativeMethods.SHGFI_ICON;
            int dwAttr = 0;

            NativeMethods.SHFILEINFO shfi = new NativeMethods.SHFILEINFO();
            NativeMethods.Shell32.SHGetFileInfo(pidl, dwAttr, ref shfi, Marshal.SizeOf(shfi), dwflag);

            IntPtr iconPtr = NativeMethods.ComCtl32.ImageList_GetIcon(m_largeImageListHandle, shfi.iIcon, NativeMethods.ILD_NORMAL);

            return Icon.FromHandle(iconPtr).ToBitmap();
        }

        public static Icon GetIcon(int index, ShellImageListSize size)
        {
            IntPtr iconPtr = IntPtr.Zero;

            switch (size)
            {
                case ShellImageListSize.Small:
                    iconPtr = NativeMethods.ComCtl32.ImageList_GetIcon(m_smallImageListHandle, index, NativeMethods.ILD_NORMAL);
                    break;

                case ShellImageListSize.Large:
                    iconPtr = NativeMethods.ComCtl32.ImageList_GetIcon(m_largeImageListHandle, index, NativeMethods.ILD_NORMAL);
                    break;
            }

            if (iconPtr != IntPtr.Zero)
            {
                Icon icon = Icon.FromHandle(iconPtr);
                Icon retVal = (Icon)icon.Clone();
                NativeMethods.User32.DestroyIcon(iconPtr);
                return retVal;
            }
            else
                return null;
        }

        #endregion

        #region Properties

        public static IntPtr SmallImageList
        {
            get { return m_smallImageListHandle; }
        }

        public static IntPtr LargeImageList
        {
            get { return m_largeImageListHandle; }
        }

        #endregion
    }
}
