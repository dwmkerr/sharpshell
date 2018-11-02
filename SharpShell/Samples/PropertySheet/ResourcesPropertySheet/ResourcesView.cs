using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResourcesPropertySheet.Properties;
using SharpShell.Diagnostics;
using SharpShell.Interop;
using SharpShell.SharpPropertySheet;

namespace ResourcesPropertySheet
{
    public partial class ResourcesView : SharpPropertyPage
    {
        public ResourcesView()
        {
            InitializeComponent();

//            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
//            this.BackColor = Color.Transparent;

            //  Set the page title.
            PageTitle = "Resources";
            PageIcon = Resources.ResourceIcon;
        }

        /// <summary>
        /// Called when the page is initialised.
        /// </summary>
        /// <param name="parent">The parent property sheet.</param>
        protected override void OnPropertyPageInitialised(SharpPropertySheet parent)
        {
            var path = parent.SelectedItemPaths.FirstOrDefault();

            if (string.IsNullOrEmpty(path))
            {
                Logging.Log("Skipping initialisation of property page as there are no paths.");
                return;
            }

            try
            {
                var resources = ResourceLoader.LoadResources(path);
                foreach (var resourceType in resources)
                {
                    var nodes = treeViewResources.Nodes.Add(resourceType.ResourceType.ToString());
                    foreach(var name in resourceType.ResourceNames) nodes.Nodes.Add(name.ToString());
                }
            }
            catch (Exception e)
            {
                Logging.Error("An error occured loading resources", e);
            }
        }
    }
}
