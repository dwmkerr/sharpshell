using System.Windows.Controls;
using Apex.MVVM;

namespace ServerInspector.Servers
{
    /// <summary>
    /// Interaction logic for ServersView.xaml
    /// </summary>
    [View(typeof(ServersViewModel))]
    public partial class ServersView : UserControl
    {
        public ServersView()
        {
            InitializeComponent();
        }
    }
}
