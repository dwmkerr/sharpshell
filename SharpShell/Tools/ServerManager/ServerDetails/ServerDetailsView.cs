using System.Windows.Forms;
using SharpShell.Attributes;
using SharpShell.ServerRegistration;

namespace ServerManager.ServerDetails
{
    public partial class ServerDetailsView : UserControl
    {
        public ServerDetailsView()
        {
            InitializeComponent();
        }

        public void Initialise(ServerEntry serverEntry)
        {
            if (serverEntry != null)
            {
                textBoxServerName.Text = serverEntry.ServerName;
                textBoxServerType.Text = serverEntry.ServerType.ToString();
                textBoxServerCLSID.Text = serverEntry.ClassId.ToString();
                textBoxServerSecurity.Text = serverEntry.GetSecurityStatus();
                textBoxAssemblyPath.Text = serverEntry.ServerPath;

                //  Get the specified associations.
                var associationType = COMServerAssociationAttribute.GetAssociationType(serverEntry.Server.GetType());
                var associations = COMServerAssociationAttribute.GetAssociations(serverEntry.Server.GetType());
                textBoxAssociations.Text = associationType.ToString() + " " + string.Join(", ", associations);


                //  Now use the server registration manager to get the registration info
                //  for the different operating system architectures.
                var info32 = ServerRegistrationManager.GetServerRegistrationInfo(serverEntry.Server.ServerClsid, RegistrationType.OS32Bit);
                var info64 = ServerRegistrationManager.GetServerRegistrationInfo(serverEntry.Server.ServerClsid, RegistrationType.OS64Bit);

                //  By default, our installation info is going to be empty.
                textBox32BitServer.Text = "Not Installed";
                textBox64BitServer.Text = "Not Installed";

                //  Do we have 32 bit registration info?
                if (info32 != null)
                {
                    //  Do we have a codebase?
                    if (!string.IsNullOrEmpty(info32.CodeBase))
                        textBox32BitServer.Text = info32.CodeBase;
                    else if (!string.IsNullOrEmpty(info32.Assembly))
                        textBox32BitServer.Text = info32.Assembly + " (GAC)";
                }
                
                //  Do we have 32 bit registration info?
                if (info64 != null)
                {
                    //  Do we have a codebase?
                    if (!string.IsNullOrEmpty(info64.CodeBase))
                        textBox64BitServer.Text = info64.CodeBase;
                    else if (!string.IsNullOrEmpty(info64.Assembly))
                        textBox64BitServer.Text = info64.Assembly + " (GAC)";
                }
            }
        }
    }
}
