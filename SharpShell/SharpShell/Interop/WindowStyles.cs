namespace SharpShell.Interop
{
    internal class WindowStyles
    {
        public const uint WS_OVERLAPPED       = 0x00000000;
        public const uint WS_POPUP        = 0x80000000;
        public const uint WS_CHILD        = 0x40000000;
        public const uint WS_MINIMIZE     = 0x20000000;
        public const uint WS_VISIBLE      = 0x10000000;
        public const uint WS_DISABLED     = 0x08000000;
        public const uint WS_CLIPSIBLINGS     = 0x04000000;
        public const uint WS_CLIPCHILDREN     = 0x02000000;
        public const uint WS_MAXIMIZE     = 0x01000000;
        public const uint WS_CAPTION      = 0x00C00000;     /* WS_BORDER | WS_DLGFRAME  */
        public const uint WS_BORDER       = 0x00800000;
        public const uint WS_DLGFRAME     = 0x00400000;
        public const uint WS_VSCROLL      = 0x00200000;
        public const uint WS_HSCROLL      = 0x00100000;
        public const uint WS_SYSMENU      = 0x00080000;
        public const uint WS_THICKFRAME       = 0x00040000;
        public const uint WS_GROUP        = 0x00020000;
        public const uint WS_TABSTOP      = 0x00010000;

        public const uint WS_MINIMIZEBOX      = 0x00020000;
        public const uint WS_MAXIMIZEBOX      = 0x00010000;

        public const uint WS_TILED        = WS_OVERLAPPED;
        public const uint WS_ICONIC       = WS_MINIMIZE;
        public const uint WS_SIZEBOX      = WS_THICKFRAME;
        public const uint WS_TILEDWINDOW      = WS_OVERLAPPEDWINDOW;

        // Common Window Styles

        public const uint WS_OVERLAPPEDWINDOW = 
            ( WS_OVERLAPPED  | 
              WS_CAPTION     | 
              WS_SYSMENU     | 
              WS_THICKFRAME  | 
              WS_MINIMIZEBOX | 
              WS_MAXIMIZEBOX );

        public const uint WS_POPUPWINDOW = 
            ( WS_POPUP   | 
              WS_BORDER  | 
              WS_SYSMENU );

        public const uint WS_CHILDWINDOW = WS_CHILD;

        //Extended Window Styles

        public const uint WS_EX_DLGMODALFRAME     = 0x00000001;
        public const uint WS_EX_NOPARENTNOTIFY    = 0x00000004;
        public const uint WS_EX_TOPMOST       = 0x00000008;
        public const uint WS_EX_ACCEPTFILES       = 0x00000010;
        public const uint WS_EX_TRANSPARENT       = 0x00000020;

//#if(WINVER >= 0x0400)
        public const uint WS_EX_MDICHILD      = 0x00000040;
        public const uint WS_EX_TOOLWINDOW    = 0x00000080;
        public const uint WS_EX_WINDOWEDGE    = 0x00000100;
        public const uint WS_EX_CLIENTEDGE    = 0x00000200;
        public const uint WS_EX_CONTEXTHELP       = 0x00000400;

        public const uint WS_EX_RIGHT         = 0x00001000;
        public const uint WS_EX_LEFT          = 0x00000000;
        public const uint WS_EX_RTLREADING    = 0x00002000;
        public const uint WS_EX_LTRREADING    = 0x00000000;
        public const uint WS_EX_LEFTSCROLLBAR     = 0x00004000;
        public const uint WS_EX_RIGHTSCROLLBAR    = 0x00000000;

        public const uint WS_EX_CONTROLPARENT     = 0x00010000;
        public const uint WS_EX_STATICEDGE    = 0x00020000;
        public const uint WS_EX_APPWINDOW     = 0x00040000;

        public const uint WS_EX_OVERLAPPEDWINDOW  = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const uint WS_EX_PALETTEWINDOW     = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
//#endif /* WINVER >= 0x0400 */

//#if(_WIN32_WINNT >= 0x0500)
        public const uint WS_EX_LAYERED       = 0x00080000;
//#endif /* _WIN32_WINNT >= 0x0500 */

//#if(WINVER >= 0x0500)
        public const uint WS_EX_NOINHERITLAYOUT   = 0x00100000; // Disable inheritence of mirroring by children
        public const uint WS_EX_LAYOUTRTL     = 0x00400000; // Right to left mirroring
//#endif /* WINVER >= 0x0500 */

//#if(_WIN32_WINNT >= 0x0500)
        public const uint WS_EX_COMPOSITED    = 0x02000000;
        public const uint WS_EX_NOACTIVATE    = 0x08000000;
//#endif /* _WIN32_WINNT >= 0x0500 */
    }
}