using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpShell.Interop;

namespace SharpShell.SharpContextMenu
{
    internal class MenuMetrics : IDisposable
    {
        /// <summary>
        /// The theme handle.
        /// </summary>
        private IntPtr hTheme;

        /// <summary>
        /// Window handle used to generate theme.
        /// </summary>
        private IntPtr hwndTheme;

        /// <summary>
        /// Popup check margins
        /// </summary>
        private MARGINS marPopupCheck;

        /// <summary>
        /// Popup check background 
        /// </summary>
        private MARGINS marPopupCheckBackground;

        // margins
        /// <summary>
        /// Popup item margins
        /// </summary>
        private MARGINS marPopupItem;

        /// <summary>
        /// Popup text margins
        /// </summary>
        private MARGINS marPopupText;

        /// <summary>
        /// Popup accelerator margins
        /// </summary>
        private MARGINS marPopupAccelerator;

        /// <summary>
        /// Popup check size metric
        /// </summary>
        private SIZE sizePopupCheck;

        /// <summary>
        /// Popup separator size metric
        /// </summary>
        private SIZE sizePopupSeparator;

        /// <summary>
        /// Popup border space between item text and accelerator
        /// </summary>
        private int iPopupBorderSize;

        /// <summary>
        /// Popup border space between item text and gutter
        /// </summary>
        private int iPopupBgBorderSize;

        /// <summary>
        /// Additional amount of vertical space to add to checkbox
        /// </summary>
        private int cyMarCheckBackground;

        private const string VSCLASS_MENU = "MENU";
        private const int MENU_MENUITEM_TMSCHEMA = 1;
        private const int MENU_MENUDROPDOWN_TMSCHEMA = 2;
        private const int MENU_MENUBARITEM_TMSCHEMA = 3;
        private const int MENU_MENUBARDROPDOWN_TMSCHEMA = 4;
        private const int MENU_CHEVRON_TMSCHEMA = 5;
        private const int MENU_SEPARATOR_TMSCHEMA = 6;
        private const int MENU_BARBACKGROUND = 7;
        private const int MENU_BARITEM = 8;
        private const int MENU_POPUPBACKGROUND = 9;
        private const int MENU_POPUPBORDERS = 10;
        private const int MENU_POPUPCHECK = 11;
        private const int MENU_POPUPCHECKBACKGROUND = 12;
        private const int MENU_POPUPGUTTER = 13;
        private const int MENU_POPUPITEM = 14;
        private const int MENU_POPUPSEPARATOR = 15;
        private const int MENU_POPUPSUBMENU = 16;
        private const int MENU_SYSTEMCLOSE = 17;
        private const int MENU_SYSTEMMAXIMIZE = 18;
        private const int MENU_SYSTEMMINIMIZE = 19;
        private const int MENU_SYSTEMRESTORE = 20;
        private const int TS_MIN = 0;
        private const int TS_TRUE = 1;
        private const int TS_DRAW = 2;
        private const int TMT_BORDERSIZE = 2403;
        private const int TMT_CONTENTMARGINS = 3602;
        private const uint ODS_SELECTED = 0x0001;
        private const uint ODS_GRAYED = 0x0002;
        private const uint ODS_DISABLED = 0x0004;
        private const uint ODS_CHECKED = 0x0008;
        private const uint ODS_FOCUS = 0x0010;
        private const uint ODS_DEFAULT = 0x0020;
        private const uint ODS_COMBOBOXEDIT = 0x1000;
        private const uint ODS_HOTLIGHT = 0x0040;
        private const uint ODS_INACTIVE = 0x0080;
        private const uint ODS_NOACCEL = 0x0100;
        private const uint ODS_NOFOCUSRECT = 0x0200;
        private const uint MFT_RADIOCHECK = 0x00000200;
        private const uint MFT_SEPARATOR = 0x00000800;
        private const uint MFT_RIGHTORDER = 0x00002000;
        private const uint MFT_RIGHTJUSTIFY = 0x00004000;
        private const uint DT_TOP = 0x00000000;
        private const uint DT_LEFT = 0x00000000;
        private const uint DT_CENTER = 0x00000001;
        private const uint DT_RIGHT = 0x00000002;
        private const uint DT_VCENTER = 0x00000004;
        private const uint DT_BOTTOM = 0x00000008;
        private const uint DT_WORDBREAK = 0x00000010;
        private const uint DT_SINGLELINE = 0x00000020;
        private const uint DT_EXPANDTABS = 0x00000040;
        private const uint DT_TABSTOP = 0x00000080;
        private const uint DT_NOCLIP = 0x00000100;
        private const uint DT_EXTERNALLEADING = 0x00000200;
        private const uint DT_CALCRECT = 0x00000400;
        private const uint DT_NOPREFIX = 0x00000800;
        private const uint DT_INTERNAL = 0x00001000;

        private const int POPUP_CHECK = 0;
        private const int POPUP_TEXT = 1;
        private const int POPUP_SEPARATOR = 2;
        private const int POPUP_MAX = 3;

        
    internal struct DRAWITEMMETRICS
    {
        public RECT rcSelection;           // Selection rectangle
        public RECT rcGutter;              // Gutter rectangle
        public RECT rcCheckBackground;     // Check background rectangle
        public RECT[] rgrc;        // POPUPMAX items Menu subitem rectangles
    }

        private void Initialize()
        {
            hTheme = Uxtheme.OpenThemeData(hwndTheme, VSCLASS_MENU);
            Uxtheme.GetThemePartSize(hTheme, IntPtr.Zero, MENU_POPUPCHECK, 0, IntPtr.Zero,
                                     TS_TRUE, out sizePopupCheck);
            Uxtheme.GetThemePartSize(hTheme, IntPtr.Zero, MENU_POPUPSEPARATOR, 0, IntPtr.Zero,
                                     TS_TRUE, out sizePopupSeparator);

            Uxtheme.GetThemeInt(hTheme, MENU_POPUPITEM, 0, TMT_BORDERSIZE,
                                out iPopupBorderSize);
            Uxtheme.GetThemeInt(hTheme, MENU_POPUPBACKGROUND, 0, TMT_BORDERSIZE,
                                out iPopupBgBorderSize);

            Uxtheme.GetThemeMargins(hTheme, IntPtr.Zero, MENU_POPUPCHECK, 0,
                                    TMT_CONTENTMARGINS, IntPtr.Zero, out marPopupCheck);
            Uxtheme.GetThemeMargins(hTheme, IntPtr.Zero, MENU_POPUPCHECKBACKGROUND, 0,
                                    TMT_CONTENTMARGINS, IntPtr.Zero,
                                    out marPopupCheckBackground);
            Uxtheme.GetThemeMargins(hTheme, IntPtr.Zero, MENU_POPUPITEM, 0,
                                    TMT_CONTENTMARGINS, IntPtr.Zero, out marPopupItem);

            marPopupAccelerator = marPopupItem;
            marPopupAccelerator.cxLeftWidth =
                marPopupAccelerator.cxRightWidth = 0;

            // Popup text margins
            MARGINS margins = marPopupItem;
            margins.cxRightWidth = iPopupBorderSize;
            margins.cxLeftWidth = iPopupBgBorderSize;
            marPopupText = margins;

            cyMarCheckBackground = marPopupCheckBackground.cyTopHeight +
                                   marPopupCheckBackground.cyBottomHeight;
        }

        public void EnsureInitialised(IntPtr hwnd)
        {
            hTheme = IntPtr.Zero;
            this.hwndTheme = hwnd;
            Initialize();
        }

        public void Dispose()
        {
            //  TODO implement pattern properly.

            //  If we have a theme handle, release it.
            if (hTheme != IntPtr.Zero)
            {
                Uxtheme.CloseThemeData(hTheme);
                hTheme = IntPtr.Zero;
            }
        }

        private POPUPITEMSTATES ToItemStateId(UInt32 uItemState)
        {
            bool fDisabled = ((uItemState & (ODS_INACTIVE |
                                             ODS_DISABLED)) != 0);
            bool fHot = ((uItemState & (ODS_HOTLIGHT |
                                        ODS_SELECTED)) != 0);
            POPUPITEMSTATES iState;

            if (fDisabled)
            {
                iState = (fHot ? POPUPITEMSTATES.MPI_DISABLEDHOT : POPUPITEMSTATES.MPI_DISABLED);
            }
            else if (fHot)
            {
                iState = POPUPITEMSTATES.MPI_HOT;
            }
            else
            {
                iState = POPUPITEMSTATES.MPI_NORMAL;
            }

            return iState;
        }

        private POPUPCHECKBACKGROUNDSTATES ToCheckBackgroundStateId(int iStateId)
        {
            POPUPCHECKBACKGROUNDSTATES iStateIdCheckBackground;

            // Determine the check background state.
            if (iStateId == (int) POPUPITEMSTATES.MPI_DISABLED || iStateId == (int) POPUPITEMSTATES.MPI_DISABLEDHOT)
            {
                iStateIdCheckBackground = POPUPCHECKBACKGROUNDSTATES.MCB_DISABLED;
            }
            else
            {
                iStateIdCheckBackground = POPUPCHECKBACKGROUNDSTATES.MCB_NORMAL;
            }

            return iStateIdCheckBackground;
        }


        private POPUPCHECKSTATES ToCheckStateId(UInt32 fType, int iStateId)
        {
            POPUPCHECKSTATES iStateIdCheck;

            if ((fType & MFT_RADIOCHECK) != 0)
            {
                if (iStateId == (int) POPUPITEMSTATES.MPI_DISABLED || iStateId == (int) POPUPITEMSTATES.MPI_DISABLEDHOT)
                {
                    iStateIdCheck = POPUPCHECKSTATES.MC_BULLETDISABLED;
                }
                else
                {
                    iStateIdCheck = POPUPCHECKSTATES.MC_BULLETNORMAL;
                }
            }
            else
            {
                if (iStateId == (int) POPUPITEMSTATES.MPI_DISABLED || iStateId == (int) POPUPITEMSTATES.MPI_DISABLEDHOT)
                {
                    iStateIdCheck = POPUPCHECKSTATES.MC_CHECKMARKDISABLED;
                }
                else
                {
                    iStateIdCheck = POPUPCHECKSTATES.MC_CHECKMARKNORMAL;
                }
            }

            return iStateIdCheck;
        }

        private void ToMeasureSize(SIZE psizeDraw, MARGINS pmargins, out SIZE psizeMeasure)
        {
            psizeMeasure.cx = psizeDraw.cx + pmargins.cxLeftWidth +
                              pmargins.cxRightWidth;
            psizeMeasure.cy = psizeDraw.cy + pmargins.cyTopHeight +
                              pmargins.cyBottomHeight;
        }

        private void ToDrawRect(RECT prcMeasure, MARGINS pmargins,
                                out RECT prcDraw)
        {

            // Convert the measure rect to a drawing rect.
            prcDraw = new RECT(prcMeasure.left + pmargins.cxLeftWidth,
                               prcMeasure.top + pmargins.cyTopHeight,
                               prcMeasure.right - pmargins.cxRightWidth,
                               prcMeasure.bottom - pmargins.cyBottomHeight);
        }


        private void MeasureMenuItem(string szItemText, int cch, UInt32 fType, SIZE[] rgPopupSize, MENUITEMINFO pmii,
                                     ref MEASUREITEMSTRUCT pmis, out int itemWidth, out int itemHeight)
        {
            int cxTotal = 0;
            int cyMax = 0;
            SIZE sizeDraw;

            // Size the check rectangle.
            sizeDraw = sizePopupCheck;
            sizeDraw.cy += marPopupCheckBackground.cyTopHeight +
                           marPopupCheckBackground.cyBottomHeight;

            ToMeasureSize(sizeDraw, marPopupCheck, out rgPopupSize[(int) POPUP_CHECK]);
            cxTotal += rgPopupSize[(int) POPUP_CHECK].cx;

            if ((fType & MFT_SEPARATOR) != 0)
            {
                // Size the separator, using the minimum width.
                sizeDraw = sizePopupCheck;
                sizeDraw.cy = sizePopupSeparator.cy;

                ToMeasureSize(sizeDraw, marPopupItem, out rgPopupSize[(int) POPUP_SEPARATOR]);
            }
            else
            {
                // Add check background horizontal padding.
                cxTotal += marPopupCheckBackground.cxLeftWidth +
                           marPopupCheckBackground.cxRightWidth;

                if (cch > 0)
                {
                    IntPtr hdc = User32.GetDC(hwndTheme);
                    if (hdc != IntPtr.Zero)
                    {
                        // Size the text subitem rectangle.
                        RECT rcText = new RECT(0, 0, 0, 0);
                        Uxtheme.GetThemeTextExtent(hTheme,
                                                   hdc,
                                                   (int) MENU_POPUPITEM,
                                                   0,
                                                   szItemText,
                                                   cch,
                                                   DT_LEFT | DT_SINGLELINE,
                                                   IntPtr.Zero,
                                                   out rcText);
                        sizeDraw.cx = rcText.right;
                        sizeDraw.cy = rcText.bottom;

                        ToMeasureSize(sizeDraw, marPopupText, out rgPopupSize[(int) POPUP_TEXT]);
                        cxTotal += rgPopupSize[(int) POPUP_TEXT].cx;

                        User32.ReleaseDC(hwndTheme, hdc);
                    }
                }

                // Account for selection margin padding.
                cxTotal += marPopupItem.cxLeftWidth +
                           marPopupItem.cxRightWidth;
            }

            // Calculate maximum menu item height.
            if ((fType & MFT_SEPARATOR) > 0)
            {
                cyMax = rgPopupSize[(int) POPUP_SEPARATOR].cy;
            }
            else
            {
                for (uint i = 0; i < (uint) POPUP_MAX; i++)
                {
                    cyMax = Math.Max(cyMax, rgPopupSize[i].cy);
                }
            }

            // Return the composite sizes.
            itemWidth = cxTotal;
            itemHeight = cyMax;
        }

        void LayoutMenuItem(int fType, SIZE[] rgPopupSize, DRAWITEMSTRUCT pdis, out DRAWITEMMETRICS pdim, RECT[] rgrc)
        {
            RECT prcItem = pdis.rcItem;
            int cyItem = pdis.rcItem.Height();
            SIZE[] prgsize = rgPopupSize;
            bool fIsSeparator = (fType & MFT_SEPARATOR) != 0;
            int x = prcItem.left + marPopupItem.cxLeftWidth; // Left gutter margin
            int y = prcItem.top;
            RECT rcMeasure = new RECT();
            pdim = new DRAWITEMMETRICS();

            for (uint i = 0; i < POPUP_MAX; i++)
            {
                if (prgsize[i].cx != 0)
                {
                    int cx;
                    int cy;

                    switch (i)
                    {
                        case POPUP_CHECK:
                            // Add left padding for the check background rectangle.
                            x += marPopupCheckBackground.cxLeftWidth;

                            // Right-align the check/bitmap in the column.            
                            cx = prgsize[i].cx;
                            cy = prgsize[i].cy - cyMarCheckBackground;
                            break;

                        default:
                            cx = prgsize[i].cx;
                            cy = prgsize[i].cy;
                            break;
                    }

                    // Position and vertically center the subitem.
                    rcMeasure = new RECT(x, y, x + cx, y + cy);
                    if (i == POPUP_CHECK)
                    {
                        ToDrawRect(rcMeasure, marPopupCheck, out rgrc[i]);
                    }
                    else
                    {
                        ToDrawRect(rcMeasure, marPopupText, out rgrc[i]);
                    }
                    rgrc[i].Offset(0, (cyItem - cy)/2);

                    // Add right padding for the check background rectangle.
                    if (i == POPUP_CHECK)
                    {
                        x += marPopupCheckBackground.cxRightWidth;
                    }

                    // Move to the next subitem.
                    x += cx;
                }
            }

            // Calculate the check background draw size.
            SIZE sizeDraw;
            sizeDraw.cx = prgsize[POPUP_CHECK].cx;
            sizeDraw.cy = prgsize[POPUP_CHECK].cy - cyMarCheckBackground;

            // Calculate the check background measure size.
            SIZE sizeMeasure;
            ToMeasureSize(sizeDraw, marPopupCheckBackground, out sizeMeasure);

            // Lay out the check background rectangle.
            x = prcItem.left + marPopupItem.cxLeftWidth;

            rcMeasure.Set(x, y, x + sizeMeasure.cx, y + sizeMeasure.cy);
            ToDrawRect(rcMeasure, marPopupCheckBackground,
                       out pdim.rcCheckBackground);
            pdim.rcCheckBackground.Offset(0, (cyItem - sizeMeasure.cy)/
                                             2);

            // Lay out gutter rectangle.
            x = prcItem.left;

            sizeDraw.cx = prgsize[POPUP_CHECK].cx;
            ToMeasureSize(sizeDraw, marPopupCheckBackground, out sizeMeasure);

            pdim.rcGutter.Set(x, y, x + marPopupItem.cxLeftWidth +
                                    sizeMeasure.cx, y + cyItem);

            if (fIsSeparator)
            {
                // Lay out the separator rectangle.
                x = pdim.rcGutter.right + marPopupItem.cxLeftWidth;

                rcMeasure.Set(x, y, prcItem.right - marPopupItem.cxRightWidth, y + prgsize[POPUP_SEPARATOR].cy);
                ToDrawRect(rcMeasure, marPopupItem, out rgrc[POPUP_SEPARATOR]);
                rgrc[POPUP_SEPARATOR].Offset(0, (cyItem -
                                                 prgsize[POPUP_SEPARATOR].cy)/2);
            }
            else
            {
                // Lay out selection rectangle.
                x = prcItem.left + marPopupItem.cxLeftWidth;

                pdim.rcSelection.Set(x, y, prcItem.right -
                                           marPopupItem.cxRightWidth, y + cyItem);
            }
        }
    }

}