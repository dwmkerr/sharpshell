using SharpShell.Attributes;
using SharpShell.SharpDeskBand;
using System;
using System.Runtime.InteropServices;

namespace WebSearchDeskBand
{

    [ComVisible(true)]
    [DisplayName("Web Search")]
    [Guid("095CF383-6240-4524-B16D-B478577758FC")]
    public class WebSearchDeskBand : SharpDeskBand
    {
        protected override System.Windows.Forms.UserControl CreateDeskBand()
        {
            return new DeskBandUI();
        }

        protected override BandOptions GetBandOptions()
        {
            return new BandOptions
                   {
                       HasVariableHeight = false,
                       IsSunken = false,
                       ShowTitle = true,
                       Title = "Web Search",
                       UseBackgroundColour = false,
                       AlwaysShowGripper = true
                   };
        }

        [ComRegisterFunction]
        public static void RegisterClass(Type type) => ComUtilities.RegisterDeskBandClass(type);

        [ComUnregisterFunction]
        public static void UnregisterClass(Type type) => ComUtilities.UnregisterClass(type);
    }
}
