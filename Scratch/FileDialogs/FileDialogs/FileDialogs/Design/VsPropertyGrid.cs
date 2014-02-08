using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace FileDialogs.Design
{
    internal class VsPropertyGrid : PropertyGrid
    {
        public VsPropertyGrid(IServiceProvider serviceProvider)
        {
            if (serviceProvider != null)
            {
                IUIService service = serviceProvider.GetService(typeof(IUIService)) as IUIService;
                if (service != null)
                    ToolStripRenderer = (ToolStripProfessionalRenderer)service.Styles["VsToolWindowRenderer"];
            }
        }
    }
}
