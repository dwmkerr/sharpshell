using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Apex.MVVM;
using SharpShell.ServerRegistration;

namespace ServerInspector.Servers
{
    /// <summary>
    /// The ServerViewModel ViewModel class.
    /// </summary>
    [ViewModel]
    public class ServerViewModel : ViewModel<ShellExtensionsRegistrationInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerViewModel"/> class.
        /// </summary>
        public ServerViewModel()
        {
            //  TODO: Use the following snippets to help build viewmodels:
            //      apexnp - Creates a Notifying Property
            //      apexc - Creates a Command.

        }


        public override void FromModel(ShellExtensionsRegistrationInfo model)
        {
            Model = model;
        }

        public override void ToModel(ShellExtensionsRegistrationInfo model)
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// The NotifyingProperty for the Model property.
        /// </summary>
        private readonly NotifyingProperty ModelProperty =
          new NotifyingProperty("Model", typeof(ShellExtensionsRegistrationInfo), default(ShellExtensionsRegistrationInfo));

        /// <summary>
        /// Gets or sets Model.
        /// </summary>
        /// <value>The value of Model.</value>
        public ShellExtensionsRegistrationInfo Model
        {
            get { return (ShellExtensionsRegistrationInfo)GetValue(ModelProperty); }
            set { SetValue(ModelProperty, value); }
        }
    }
}
