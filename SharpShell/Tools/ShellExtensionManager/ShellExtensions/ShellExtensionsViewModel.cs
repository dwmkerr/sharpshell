using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Apex.MVVM;
using SharpShell.ServerRegistration;

namespace ShellExtensionManager.ShellExtensions
{
    [ViewModel]
    public class ShellExtensionsViewModel : ViewModel
    {
        public ShellExtensionsViewModel()
        {
            //  Create the RefreshExtensions Asynchronous Command.
            RefreshExtensionsCommand = new AsynchronousCommand(DoRefreshExtensionsCommand);

            RefreshExtensionsCommand.DoExecute();
        }

        /// <summary>
        /// Performs the  command.
        /// </summary>
        /// <param name="parameter">The RefreshExtensions command parameter.</param>
        private void DoRefreshExtensionsCommand(object parameter)
        {
            //  Get all servers.
            var servers = ServerRegistrationManager.EnumerateExtensions(RegistrationType.OS64Bit, ShellExtensionType.IconHandler);
            foreach (var server in servers)
            {
                var extensionViewModel = new ExtensionViewModel();
                extensionViewModel.DisplayName = server.DisplayName;
                extensionViewModel.ShellExtensionType = server.ShellExtensionType;
                foreach (var classReg in server.ClassRegistrations)
                    extensionViewModel.ClassRegistrations.Add(classReg);
                RefreshExtensionsCommand.ReportProgress(() => Extensions.Add(extensionViewModel));
            }
        }

        /// <summary>
        /// Gets the RefreshExtensions command.
        /// </summary>
        /// <value>The value of .</value>
        public AsynchronousCommand RefreshExtensionsCommand
        {
            get;
            private set;
        }

        
        /// <summary>
        /// The Extensions observable collection.
        /// </summary>
        private readonly ObservableCollection<ExtensionViewModel> ExtensionsProperty =
          new ObservableCollection<ExtensionViewModel>();

        /// <summary>
        /// Gets the Extensions observable collection.
        /// </summary>
        /// <value>The Extensions observable collection.</value>
        public ObservableCollection<ExtensionViewModel> Extensions
        {
            get { return ExtensionsProperty; }
        }

        
        /// <summary>
        /// The NotifyingProperty for the SelectedExtension property.
        /// </summary>
        private readonly NotifyingProperty SelectedExtensionProperty =
          new NotifyingProperty("SelectedExtension", typeof(ExtensionViewModel), default(ExtensionViewModel));

        /// <summary>
        /// Gets or sets SelectedExtension.
        /// </summary>
        /// <value>The value of SelectedExtension.</value>
        public ExtensionViewModel SelectedExtension
        {
            get { return (ExtensionViewModel)GetValue(SelectedExtensionProperty); }
            set { SetValue(SelectedExtensionProperty, value); }
        }
    }
}
