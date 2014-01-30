using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentVariablesNamespaceExtension
{
    public partial class EnvironmentVariablesView : UserControl
    {
        public EnvironmentVariablesView()
        {
            InitializeComponent();

            Load += EnvironmentVariablesView_Load;
        }

        void EnvironmentVariablesView_Load(object sender, EventArgs e)
        {
            //  Get the system environment variables.
            var systemEnvironmentVariables =
                System.Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Machine);

            //  Add each to the list.
            foreach (string variableKey in systemEnvironmentVariables.Keys)
            {
                listViewEnvironmentVariables.Items.Add(
                    new ListViewItem(
                        new[]
                        {
                            variableKey,
                            systemEnvironmentVariables[variableKey].ToString()
                        }));
            }
        }

        
    }
}
