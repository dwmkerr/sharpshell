using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResourcesPropertySheet.Loader;

namespace ResourcesPropertySheet.ResourceEditors
{
    internal partial class BitmapEditor : UserControl, IResourceEditor
    {
        public BitmapEditor()
        {
            InitializeComponent();
        }

        public void Initialise(Win32Resource resource)
        {
            //  Create a byte stream from the data.
            this.pictureBoxPreview.Image = resource.BitmapData;
        }

        public void Release()
        {
            this.pictureBoxPreview.Image = null;
        }
    }
}
