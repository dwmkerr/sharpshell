using SharpShell.Attributes;
using SharpShell.SharpDeskBand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WebSearchDeskBand
{

    [ComVisible(true)]
    [DisplayName("Web Search")]
    public class WebSearchDeskBand : SharpDeskBand
    {
        protected override System.Windows.Forms.UserControl CreateDeskBand()
        {
            return new DeskBandUI();
        }
    }
}
