using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebSearchDeskBand
{
    public partial class DeskBandUI : UserControl
    {
        public DeskBandUI()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Process.Start("http://google.com#q=" + textBoxSearch.Text);
        }
    }
}
