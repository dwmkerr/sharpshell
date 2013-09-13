using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Apex.MVVM;
using Microsoft.Win32;
using SharpShell.ServerRegistration;

namespace ServerInspector.Servers
{
    /// <summary>
    /// The ServersViewModel ViewModel class.
    /// </summary>
    [ViewModel]
    public class ServersViewModel : ViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServersViewModel"/> class.
        /// </summary>
        public ServersViewModel()
        {
            ServersView = (ListCollectionView)CollectionViewSource.GetDefaultView(Servers);
            ServersView.Filter += Filter;
            //  Create the ReadServers Asynchronous Command.
            ReadServersCommand = new AsynchronousCommand(DoReadServersCommand);
            
            ReadServersCommand.DoExecute();
        }

        private bool Filter(object o)
        {
            var server = o as ServerViewModel;
            if (server == null || server.Model == null)
                return false;

            //  First, handle the type filters.
            if (!ShowPartiallyRegistered && server.Model.ServerRegistationType == ServerRegistationType.PartiallyRegistered)
                return false;
            if (!ShowNativeInProc32 && server.Model.ServerRegistationType == ServerRegistationType.NativeInProc32)
                return false;
            if (!ShowManagedInProc32 && server.Model.ServerRegistationType == ServerRegistationType.ManagedInProc32)
                return false;

            //  Now handle the search text filter.
            if (string.IsNullOrEmpty(SearchText))
                return true;

            if (server.Model.ServerCLSID.ToString().IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) != -1)
                return true;
            if(!string.IsNullOrEmpty(server.Model.ServerPath))
                if (server.Model.ServerPath.IndexOf(SearchText, StringComparison.OrdinalIgnoreCase) != -1)
                    return true;
            return false;
        }


        /// <summary>
        /// The Servers observable collection.
        /// </summary>
        private readonly ObservableCollection<ServerViewModel> ServersProperty =
          new ObservableCollection<ServerViewModel>();

        /// <summary>
        /// Gets the Servers observable collection.
        /// </summary>
        /// <value>The Servers observable collection.</value>
        public ObservableCollection<ServerViewModel> Servers
        {
            get { return ServersProperty; }
        }
        
        /// <summary>
        /// Performs the ReadServers command.
        /// </summary>
        /// <param name="parameter">The ReadServers command parameter.</param>
        private void DoReadServersCommand(object parameter)
        {
            //  Get each class id.
            using (var classesKey = Registry.ClassesRoot.OpenSubKey("CLSID"))
            {
                var classes = classesKey.GetSubKeyNames();

                //  Read each class.
                foreach(var classGuid in classes)
                {
                    Guid clsid;
                    if(Guid.TryParse(classGuid, out clsid) == false)
                        continue;

                    var serverRegistrationInfo = ServerRegistrationManager.GetServerRegistrationInfo(clsid, 
                        Environment.Is64BitOperatingSystem ? RegistrationType.OS64Bit : RegistrationType.OS32Bit);
                    
                    //  Add the vm.
                    ReadServersCommand.ReportProgress(
                        () =>
                            {
                                var model = serverRegistrationInfo;
                                var serverViewModel = new ServerViewModel();
                                serverViewModel.FromModel(model);
                                Servers.Add(serverViewModel);
                            }
                        );
                }
            }
        }

        /// <summary>
        /// Gets the ReadServers command.
        /// </summary>
        /// <value>The value of .</value>
        public AsynchronousCommand ReadServersCommand
        {
            get;
            private set;
        }

        
        /// <summary>
        /// The NotifyingProperty for the ServersView property.
        /// </summary>
        private readonly NotifyingProperty ServersViewProperty =
          new NotifyingProperty("ServersView", typeof(ListCollectionView), default(ListCollectionView));

        /// <summary>
        /// Gets or sets ServersView.
        /// </summary>
        /// <value>The value of ServersView.</value>
        public ListCollectionView ServersView
        {
            get { return (ListCollectionView)GetValue(ServersViewProperty); }
            set { SetValue(ServersViewProperty, value); }
        }

        
        /// <summary>
        /// The NotifyingProperty for the SearchText property.
        /// </summary>
        private readonly NotifyingProperty SearchTextProperty =
          new NotifyingProperty("SearchText", typeof(string), default(string));

        /// <summary>
        /// Gets or sets SearchText.
        /// </summary>
        /// <value>The value of SearchText.</value>
        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value);
            ServersView.Refresh();}
        }

        
        /// <summary>
        /// The NotifyingProperty for the ShowNativeInProc32 property.
        /// </summary>
        private readonly NotifyingProperty ShowNativeInProc32Property =
          new NotifyingProperty("ShowNativeInProc32", typeof(bool), true);

        /// <summary>
        /// Gets or sets ShowNativeInProc32.
        /// </summary>
        /// <value>The value of ShowNativeInProc32.</value>
        public bool ShowNativeInProc32
        {
            get { return (bool)GetValue(ShowNativeInProc32Property); }
            set { SetValue(ShowNativeInProc32Property, value); ServersView.Refresh(); }
        }

        
        /// <summary>
        /// The NotifyingProperty for the ShowManagedInProc32 property.
        /// </summary>
        private readonly NotifyingProperty ShowManagedInProc32Property =
          new NotifyingProperty("ShowManagedInProc32", typeof(bool), true);

        /// <summary>
        /// Gets or sets ShowManagedInProc32.
        /// </summary>
        /// <value>The value of ShowManagedInProc32.</value>
        public bool ShowManagedInProc32
        {
            get { return (bool)GetValue(ShowManagedInProc32Property); }
            set { SetValue(ShowManagedInProc32Property, value); ServersView.Refresh(); }
        }

        
        /// <summary>
        /// The NotifyingProperty for the ShowPartiallyRegistered property.
        /// </summary>
        private readonly NotifyingProperty ShowPartiallyRegisteredProperty =
          new NotifyingProperty("ShowPartiallyRegistered", typeof(bool), true);

        /// <summary>
        /// Gets or sets ShowPartiallyRegistered.
        /// </summary>
        /// <value>The value of ShowPartiallyRegistered.</value>
        public bool ShowPartiallyRegistered
        {
            get { return (bool)GetValue(ShowPartiallyRegisteredProperty); }
            set { SetValue(ShowPartiallyRegisteredProperty, value); ServersView.Refresh(); }
        }
    }
}
