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
using ResourcesPropertySheet.Loader;
using ResourcesPropertySheet.Properties;
using ResourcesPropertySheet.ResourceEditors;
using SharpShell.Diagnostics;
using SharpShell.Interop;
using SharpShell.SharpPropertySheet;

namespace ResourcesPropertySheet
{
    public partial class ResourcesView : SharpPropertyPage
    {
        private IResourceEditor currentEditor;

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
                    foreach (var resource in resourceType.Resouces)
                    {
                        var node = nodes.Nodes.Add(resource.ResourceName.ToString());
                        node.Tag = resource;
                    }
                }
            }
            catch (Exception e)
            {
                Logging.Error("An error occured loading resources", e);
            }
        }

        private void treeViewResources_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //  Get the editor host.
            var editorHost = tableLayoutPanel1.GetControlFromPosition(1, 1);

            //  Release any existing editor.
            if (currentEditor != null)
            {
                editorHost.Controls.Remove((Control)currentEditor);
                currentEditor.Release();
            }

            //  Create the appropriate control.
            var resource = e.Node?.Tag as Win32Resource;
            if (resource == null) return;

            if (resource.ResourceType.IsKnownResourceType(ResType.RT_BITMAP))
            {
                var bitmapEditor = new BitmapEditor {Dock = DockStyle.Fill};
                currentEditor = bitmapEditor;
                var parent = tableLayoutPanel1.GetControlFromPosition(1, 1);
                parent.Controls.Add(bitmapEditor);
                bitmapEditor.Initialise(resource);
            }
            else if (resource.ResourceType.IsKnownResourceType(ResType.RT_ICON))
            {
                var iconEditor = new IconEditor { Dock = DockStyle.Fill };
                currentEditor = iconEditor;
                var parent = tableLayoutPanel1.GetControlFromPosition(1, 1);
                parent.Controls.Add(iconEditor);
                iconEditor.Initialise(resource);
            }
        }
    }
}
