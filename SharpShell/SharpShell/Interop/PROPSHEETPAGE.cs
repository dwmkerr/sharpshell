using System;
using System.Runtime.InteropServices;

namespace SharpShell.Interop
{
    /// <summary>
    /// Defines a page in a property sheet.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PROPSHEETPAGE
    {
        /// <summary>
        /// Size, in bytes, of this structure.
        /// </summary>
        public uint dwSize;

        /// <summary>
        /// Flags that indicate which options to use when creating the property sheet page. This member can be a combination of the following values.
        /// </summary>
        public PSP dwFlags;

        /// <summary>
        /// Handle to the instance from which to load an icon or string resource. If the pszIcon, pszTitle, pszHeaderTitle, or pszHeaderSubTitle member identifies a resource to load, hInstance must be specified.
        /// </summary>
        public IntPtr hInstance;

        /// <summary>
        /// Dialog box template to use to create the page. This member can specify either the resource identifier of the template or the address of a string that specifies the name of the template. If the PSP_DLGINDIRECT flag in the dwFlags member is set, pszTemplate is ignored. This member is declared as a union with pResource.
        /// </summary>
        public IntPtr pTemplate;

        /// <summary>
        /// Handle to the icon to use as the icon in the tab of the page. If the dwFlags member does not include PSP_USEHICON, this member is ignored. This member is declared as a union with pszIcon.
        /// </summary>
        public IntPtr hIcon;

        /// <summary>
        /// Title of the property sheet dialog box. This title overrides the title specified in the dialog box template. This member can specify either the identifier of a string resource or the address of a string that specifies the title. To use this member, you must set the PSP_USETITLE flag in the dwFlags member.
        /// </summary>
        public string pszTitle;

        /// <summary>
        /// Pointer to the dialog box procedure for the page. Because the pages are created as modeless dialog boxes, the dialog box procedure must not call the EndDialog function.
        /// </summary>
        public DialogProc pfnDlgProc;

        /// <summary>
        /// When the page is created, a copy of the page's PROPSHEETPAGE structure is passed to the dialog box procedure with a WM_INITDIALOG message. The lParam member is provided to allow you to pass application-specific information to the dialog box procedure. It has no effect on the page itself. For more information, see Property Sheet Creation.
        /// </summary>
        public IntPtr lParam;

        /// <summary>
        /// Pointer to an application-defined callback function that is called when the page is created and when it is about to be destroyed. For more information about the callback function, see PropSheetPageProc. To use this member, you must set the PSP_USECALLBACK flag in the dwFlags member.
        /// </summary>
        public PropSheetCallback pfnCallback;

        /// <summary>
        /// Pointer to the reference count value. To use this member, you must set the PSP_USEREFPARENT flag in the dwFlags member.
        /// </summary>
        public int pcRefParent;

        /// <summary>
        /// Version 5.80 or later. Title of the header area. To use this member under the Wizard97-style wizard, you must also do the following:
        ///      Set the PSP_USEHEADERTITLE flag in the dwFlags member.
        ///      Set the PSH_WIZARD97 flag in the dwFlags member of the page's PROPSHEETHEADER structure.
        ///      Make sure that the PSP_HIDEHEADER flag in the dwFlags member is not set.
        /// </summary>
        public string pszHeaderTitle;

        /// <summary>
        /// Version 5.80. Subtitle of the header area. To use this member, you must do the following:
        ///     Set the PSP_USEHEADERSUBTITLE flag in the dwFlags member.
        ///     Set the PSH_WIZARD97 flag in the dwFlags member of the page's PROPSHEETHEADER structure.
        ///     Make sure that the PSP_HIDEHEADER flag in the dwFlags member is not set.
        /// Note  This member is ignored when using the Aero-style wizard (PSH_AEROWIZARD).
        /// </summary>
        public string pszHeaderSubTitle;
    }
}
