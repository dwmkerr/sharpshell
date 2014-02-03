using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpShell.Interop;

namespace SharpShell.SharpContextMenu
{
    /// <summary>
    /// The Native Context Menu Wrapper builds a native context menu from a WinForms context
    /// menu. It also allows command indexes and verbs back to the original menu items.
    /// </summary>
    internal class NativeContextMenuWrapper
    {
        /// <summary>
        /// Resets the native context menu.
        /// </summary>
        public void ResetNativeContextMenu()
        {
            //  Reset any existing data.
            indexedCommands.Clear();
            verbsToCommands.Clear();
        }

        /// <summary>
        /// Builds a native context menu, on to the provided HMENU.
        /// </summary>
        /// <param name="hMenu">The handle to the menu.</param>
        /// <param name="firstItemId">The first item id.</param>
        /// <param name="toolStripItems">The tool strip menu items.</param>
        /// <returns>The index of the last item created.</returns>
        public uint BuildNativeContextMenu(IntPtr hMenu, uint firstItemId, ToolStripItemCollection toolStripItems)
        {
            //  Create an ID counter and position counter.
            var idCounter = firstItemId;
            uint positionCounter = 0;

            //  Go through every tool strip item.
            foreach (ToolStripItem item in toolStripItems)
            {
                //  If there's not a name, set one. 
                //  This will be used as the verb.
                if (string.IsNullOrEmpty(item.Name))
                    item.Name = Guid.NewGuid().ToString();

                //  Map the command by verb.
                verbsToCommands[item.Name] = item;

                //  Create the native menu item info.
                var menuItemInfo = CreateNativeMenuItem(item, idCounter);

                //  Insert the native menu item.
                if (User32.InsertMenuItem(hMenu, positionCounter, true, ref menuItemInfo) == false)
                {
                    //  We failed to build the item, so don't try and do anything more with it.
                    continue;
                }

                //  We successfully created the menu item, so increment the counters.
                indexedCommands.Add(item);
                idCounter++;
                positionCounter++;

                //  Have we just built a menu item? If so, does it have child items?
                var toolStripMenuItem = item as ToolStripMenuItem;
                if (toolStripMenuItem != null && toolStripMenuItem.HasDropDownItems)
                {
                    //  Create each drop down item.
                    idCounter = BuildNativeContextMenu(menuItemInfo.hSubMenu, idCounter, toolStripMenuItem.DropDownItems);
                }
            }

            //  Return the counter.
            return idCounter;
        }

        /// <summary>
        /// Creates the native menu item.
        /// </summary>
        /// <param name="toolStripItem">The tool strip item.</param>
        /// <param name="id">The id.</param>
        /// <returns>The native menu ite,.</returns>
        private static MENUITEMINFO CreateNativeMenuItem(ToolStripItem toolStripItem, uint id)
        {
            //  Create a menu item info, set its size.
            var menuItemInfo = new MENUITEMINFO();
            menuItemInfo.cbSize = (uint)Marshal.SizeOf(menuItemInfo);
            menuItemInfo.wID = id;
            
            //  Depending on the type of the item, we'll call the appropriate building function.
            if(toolStripItem is ToolStripMenuItem)
                BuildMenuItemInfo(ref menuItemInfo, (ToolStripMenuItem)toolStripItem);
            else if(toolStripItem is ToolStripSeparator)
                BuildSeparatorMenuItemInfo(ref menuItemInfo);

            //  Return the menu item info.
            return menuItemInfo;

        }

        /// <summary>
        /// Builds the menu item info.
        /// </summary>
        /// <param name="menuItemInfo">The menu item info.</param>
        /// <param name="menuItem">The menu item.</param>
        private static void BuildMenuItemInfo(ref MENUITEMINFO menuItemInfo, ToolStripMenuItem menuItem)
        {
            //  Set the mask - we're interested in essentially everything.
            menuItemInfo.fMask = (uint)(MIIM.MIIM_BITMAP | MIIM.MIIM_STRING | MIIM.MIIM_FTYPE |
                                         MIIM.MIIM_ID | MIIM.MIIM_STATE);

            //  If the menu item has children, we'll also create the submenu.
            if (menuItem.HasDropDownItems)
            {
                menuItemInfo.fMask += (uint) MIIM.MIIM_SUBMENU;
                menuItemInfo.hSubMenu = User32.CreatePopupMenu();
            }
            
            //  The type is the string.
            menuItemInfo.fType = (uint)MFT.MFT_STRING;

            //  The type data is the text of the menu item.
            menuItemInfo.dwTypeData = menuItem.Text;

            //  The state is enabled.
            menuItemInfo.fState = menuItem.Enabled ? (uint)MFS.MFS_ENABLED : (uint)(MFS.MFS_DISABLED | MFS.MFS_GRAYED);

            //  If the menu item is checked, add the check state.
            if (menuItem.Checked)
            {
                menuItemInfo.fState += (uint) MFS.MFS_CHECKED;
            }

            //  Is there an icon?
            menuItemInfo.hbmpItem = menuItem.Image != null ? CreatePARGB32(menuItem.Image) : IntPtr.Zero;
        }

        private static IntPtr CreatePARGB32(Image source)
        {

            //  Get the image a PARGB32 bit array.
            var clone = new Bitmap(source.Width, source.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            using (Graphics gr = Graphics.FromImage(clone))
            {
                gr.DrawImage(clone, new Rectangle(0, 0, clone.Width, clone.Height));
            }

            var bits = clone.LockBits(new Rectangle(0, 0, source.Width, source.Height), ImageLockMode.ReadOnly,
                                      PixelFormat.Format32bppPArgb);

            BITMAPINFO bmi = new BITMAPINFO();
            bmi.bmiHeader = new BITMAPINFOHEADER();
            bmi.bmiHeader.biSize = (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
            bmi.bmiHeader.biWidth = source.Width;
            bmi.bmiHeader.biHeight = source.Height;
            bmi.bmiHeader.biPlanes = 1;
            bmi.bmiHeader.biBitCount = 32;
            bmi.bmiHeader.biCompression = 0x0;

            IntPtr pBits;
            IntPtr hBitmap = Gdi32.CreateDIBSection(IntPtr.Zero, ref bmi, 0, out pBits, IntPtr.Zero, 0);

            CopyMemory(pBits, bits.Scan0, (uint)(source.Width * source.Height * 4));
            return hBitmap;
        }

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        /// <summary>
        /// Builds the menu item info.
        /// </summary>
        /// <param name="menuItemInfo">The menu item info.</param>
        private static void BuildSeparatorMenuItemInfo(ref MENUITEMINFO menuItemInfo)
        {
            //  Set the mask to the type only, and the type to the separator type.
            menuItemInfo.fMask = (uint)MIIM.MIIM_TYPE;
            menuItemInfo.fType = (uint)MFT.MFT_SEPARATOR;
        }

        /// <summary>
        /// Tries to invoke the command.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>True if the command is invoked.</returns>
        public bool TryInvokeCommand(int index)
        {
            //  Get the item. If we don't have it, return false.
            if (index >= indexedCommands.Count)
                return false;

            //  Fire the click event.
            indexedCommands[index].PerformClick();

            //  The command was invoked, return true,
            return true;
        }

        /// <summary>
        /// Tries to invoke the command.
        /// </summary>
        /// <param name="verb">The verb.</param>
        /// <returns>True if the command is invoked.</returns>
        public bool TryInvokeCommand(string verb)
        {
            //  Get the item. If we don't have it, return false.
            ToolStripItem item;
            if (verbsToCommands.TryGetValue(verb, out item) == false)
                return false;

            //  Fire the click event.
            item.PerformClick();

            //  The command was invoked, return true,
            return true;
        }
        
        /// <summary>
        /// Map of indexes to commands.
        /// </summary>
        private readonly List<ToolStripItem> indexedCommands = new List<ToolStripItem>();

        /// <summary>
        /// Map of verbs to commands.
        /// </summary>
        private readonly Dictionary<string, ToolStripItem> verbsToCommands = new Dictionary<string, ToolStripItem>();
    }

   /* internal static class TransparentBitmapCreator
    {
        public static IntPtr CreateTransparentBitmap(Image image)
        {
            // http://msdn.microsoft.com/en-us/library/bb757020.aspx
        }

        void Create32BitHBITMAP(Size size, out IntPtr bits, ref IntPtr hbitmap)
        {
            var header = new BITMAPINFOHEADER();
            header.biSize = (uint)Marshal.SizeOf(header);
            header.biPlanes = 1;
            header.biCompression = 0;//BI_RGB
            header.biWidth = size.Width;
            header.biHeight = size.Height;
            header.biBitCount = 32;

            var hdc = User32.GetDC(IntPtr.Zero);

            hbitmap = Gdi32.CreateDIBSection(hdc, header, 0 //DIB_RGB_COLORS, out bits, IntPtr.Zero, 0);
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct ARGB
        {
            public UInt32 A;
            public UInt32 R;
            public UInt32 G;
            public UInt32 B;
        }

        static void ConvertToPARGB32(IntPtr hdc, ref RGBQUAD[] pargb, IntPtr hbmp, Size size, int cxRow)
        {

            var header = new BITMAPINFOHEADER();
            header.biSize = (uint)Marshal.SizeOf(header);
            header.biPlanes = 1;
            header.biCompression = 0;//BI_RGB
            header.biWidth = size.Width;
            header.biHeight = size.Height;
            header.biBitCount = 32;
            var bmi = new BITMAPINFO {bmiHeader = header};

            var mem = Marshal.AllocHGlobal(header.biWidth*4*header.biHeight);
            var bits = new byte[header.biWidth*4*header.biHeight];
            Gdi32.GetDIBits(hdc, hbmp, 0, (uint)header.biHeight, bits, ref bmi, 0 //DIB_RGB_COLORS);

            var index = 0;
            var pIndex = 0;
            var cxDelta = cxRow - bmi.bmiHeader.biWidth;
            for (var y = bmi.bmiHeader.biHeight; y != 0; --y)
            {
                for (var x = bmi.bmiHeader.biWidth; x != 0; --x)
                {
                    if (bits[index] != 0)
                    {
                        // transparent pixel
                        pargb[pIndex] = 0;
                    }
                    else
                    {
                        // opaque pixel
                        *pargb++ |= 0xFF000000;
                    }
                }

                pargb += cxDelta;
            }
        }*/
/*

HRESULT ConvertToPARGB32(HDC hdc, __inout ARGB *pargb, HBITMAP hbmp, SIZE& sizImage, int cxRow)
{
    BITMAPINFO bmi;
    InitBitmapInfo(&bmi, sizeof(bmi), sizImage.cx, sizImage.cy, 32);

    HRESULT hr = E_OUTOFMEMORY;
    HANDLE hHeap = GetProcessHeap();
    void *pvBits = HeapAlloc(hHeap, 0, bmi.bmiHeader.biWidth * 4 * bmi.bmiHeader.biHeight);
    if (pvBits)
    {
        hr = E_UNEXPECTED;
        if (GetDIBits(hdc, hbmp, 0, bmi.bmiHeader.biHeight, pvBits, &bmi, DIB_RGB_COLORS) == bmi.bmiHeader.biHeight)
        {
            ULONG cxDelta = cxRow - bmi.bmiHeader.biWidth;
            ARGB *pargbMask = static_cast<ARGB *>(pvBits);

            for (ULONG y = bmi.bmiHeader.biHeight; y; --y)
            {
                for (ULONG x = bmi.bmiHeader.biWidth; x; --x)
                {
                    if (*pargbMask++)
                    {
                        // transparent pixel
                        *pargb++ = 0;
                    }
                    else
                    {
                        // opaque pixel
                        *pargb++ |= 0xFF000000;
                    }
                }

                pargb += cxDelta;
            }

            hr = S_OK;
        }

        HeapFree(hHeap, 0, pvBits);
    }

    return hr;
}

bool HasAlpha(__in ARGB *pargb, SIZE& sizImage, int cxRow)
{
    ULONG cxDelta = cxRow - sizImage.cx;
    for (ULONG y = sizImage.cy; y; --y)
    {
        for (ULONG x = sizImage.cx; x; --x)
        {
            if (*pargb++ & 0xFF000000)
            {
                return true;
            }
        }

        pargb += cxDelta;
    }

    return false;
}

HRESULT ConvertBufferToPARGB32(HPAINTBUFFER hPaintBuffer, HDC hdc, HICON hicon, SIZE& sizIcon)
{
    RGBQUAD *prgbQuad;
    int cxRow;
    HRESULT hr = GetBufferedPaintBits(hPaintBuffer, &prgbQuad, &cxRow);
    if (SUCCEEDED(hr))
    {
        ARGB *pargb = reinterpret_cast<ARGB *>(prgbQuad);
        if (!HasAlpha(pargb, sizIcon, cxRow))
        {
            ICONINFO info;
            if (GetIconInfo(hicon, &info))
            {
                if (info.hbmMask)
                {
                    hr = ConvertToPARGB32(hdc, pargb, info.hbmMask, sizIcon, cxRow);
                }

                DeleteObject(info.hbmColor);
                DeleteObject(info.hbmMask);
            }
        }
    }

    return hr;
}

HRESULT AddIconToMenuItem(HMENU hmenu, int iMenuItem, BOOL fByPosition, HICON hicon, BOOL fAutoDestroy, __out_opt HBITMAP *phbmp)
{
    HRESULT hr = E_OUTOFMEMORY;
    HBITMAP hbmp = NULL;

    SIZE sizIcon;
    sizIcon.cx = GetSystemMetrics(SM_CXSMICON);
    sizIcon.cy = GetSystemMetrics(SM_CYSMICON);

    RECT rcIcon;
    SetRect(&rcIcon, 0, 0, sizIcon.cx, sizIcon.cy);

    HDC hdcDest = CreateCompatibleDC(NULL);
    if (hdcDest)
    {
        hr = Create32BitHBITMAP(hdcDest, &sizIcon, NULL, &hbmp);
        if (SUCCEEDED(hr))
        {
            hr = E_FAIL;

            HBITMAP hbmpOld = (HBITMAP)SelectObject(hdcDest, hbmp);
            if (hbmpOld)
            {
                BLENDFUNCTION bfAlpha = { AC_SRC_OVER, 0, 255, AC_SRC_ALPHA };
                BP_PAINTPARAMS paintParams = {0};
                paintParams.cbSize = sizeof(paintParams);
                paintParams.dwFlags = BPPF_ERASE;
                paintParams.pBlendFunction = &bfAlpha;

                HDC hdcBuffer;
                HPAINTBUFFER hPaintBuffer = BeginBufferedPaint(hdcDest, &rcIcon, BPBF_DIB, &paintParams, &hdcBuffer);
                if (hPaintBuffer)
                {
                    if (DrawIconEx(hdcBuffer, 0, 0, hicon, sizIcon.cx, sizIcon.cy, 0, NULL, DI_NORMAL))
                    {
                        // If icon did not have an alpha channel, we need to convert buffer to PARGB.
                        hr = ConvertBufferToPARGB32(hPaintBuffer, hdcDest, hicon, sizIcon);
                    }

                    // This will write the buffer contents to the
// destination bitmap.
                    EndBufferedPaint(hPaintBuffer, TRUE);
                }

                SelectObject(hdcDest, hbmpOld);
            }
        }

        DeleteDC(hdcDest);
    }

    if (SUCCEEDED(hr))
    {
        hr = AddBitmapToMenuItem(hmenu, iMenuItem, fByPosition, hbmp);
    }

    if (FAILED(hr))
    {
        DeleteObject(hbmp);
        hbmp = NULL;
    }

    if (fAutoDestroy)
    {
        DestroyIcon(hicon);
    }

    if (phbmp)
    {
        *phbmp = hbmp;
    }

    return hr;
}*/
   /* }*/
}
