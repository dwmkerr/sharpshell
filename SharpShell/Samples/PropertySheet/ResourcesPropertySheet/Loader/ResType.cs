using System.ComponentModel.DataAnnotations;
// ReSharper disable IdentifierTypo, InconsistentNaming, UnusedMember.Global

namespace ResourcesPropertySheet.Loader
{
    /// <summary>
    /// Win32 resource types.
    /// </summary>
    internal enum ResType : uint
    {
        [Display(Name = "Cursor")]
        RT_CURSOR = 1,

        [Display(Name = "Bitmap")]
        RT_BITMAP = 2,

        [Display(Name = "Icon")]
        RT_ICON = 3,

        [Display(Name = "Menu")]
        RT_MENU = 4,

        [Display(Name = "Dialog")]
        RT_DIALOG = 5,

        [Display(Name = "String Table")]
        RT_STRING = 6,

        [Display(Name = "Font Directory")]
        RT_FONTDIR = 7,

        [Display(Name = "Font")]
        RT_FONT = 8,

        [Display(Name = "Accelerator")]
        RT_ACCELERATOR = 9,

        [Display(Name = "Application Defined Resource")]
        RT_RCDATA = 10,

        [Display(Name = "Message Table")]
        RT_MESSAGETABLE = 11,

        [Display(Name = "Group Cursor")]
        RT_GROUP_CURSOR = 12,

        [Display(Name = "Group Icon")]
        RT_GROUP_ICON = 14,

        [Display(Name = "Version")]
        RT_VERSION = 16,

        [Display(Name = "Dialog Include")]
        RT_DLGINCLUDE = 17,

        [Display(Name = "Plug and Play")]
        RT_PLUGPLAY = 19,

        [Display(Name = "VXD")]
        RT_VXD = 20,

        [Display(Name = "Animated Cursor")]
        RT_ANICURSOR = 21,

        [Display(Name = "Animated Icon")]
        RT_ANIICON = 22,

        [Display(Name = "HTML")]
        RT_HTML = 23,

        [Display(Name = "RT_MANIFEST")]
        RT_MANIFEST = 24
    }
}