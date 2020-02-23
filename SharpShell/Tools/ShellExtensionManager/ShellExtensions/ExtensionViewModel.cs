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
    public class ExtensionViewModel : ViewModel
    {
        
        /// <summary>
        /// The NotifyingProperty for the DisplayName property.
        /// </summary>
        private readonly NotifyingProperty DisplayNameProperty =
          new NotifyingProperty("DisplayName", typeof(string), default(string));

        /// <summary>
        /// Gets or sets DisplayName.
        /// </summary>
        /// <value>The value of DisplayName.</value>
        public string DisplayName
        {
            get { return (string)GetValue(DisplayNameProperty); }
            set { SetValue(DisplayNameProperty, value); }
        }

        
        /// <summary>
        /// The NotifyingProperty for the ShellExtensionType property.
        /// </summary>
        private readonly NotifyingProperty ShellExtensionTypeProperty =
          new NotifyingProperty("ShellExtensionType", typeof(ShellExtensionType), default(ShellExtensionType));

        /// <summary>
        /// Gets or sets ShellExtensionType.
        /// </summary>
        /// <value>The value of ShellExtensionType.</value>
        public ShellExtensionType ShellExtensionType
        {
            get { return (ShellExtensionType)GetValue(ShellExtensionTypeProperty); }
            set { SetValue(ShellExtensionTypeProperty, value); }
        }

        
        /// <summary>
        /// The ClassRegistrations observable collection.
        /// </summary>
        private readonly ObservableCollection<ClassRegistration> ClassRegistrationsProperty =
          new ObservableCollection<ClassRegistration>();

        /// <summary>
        /// Gets the ClassRegistrations observable collection.
        /// </summary>
        /// <value>The ClassRegistrations observable collection.</value>
        public ObservableCollection<ClassRegistration> ClassRegistrations
        {
            get { return ClassRegistrationsProperty; }
        }
    }
}
