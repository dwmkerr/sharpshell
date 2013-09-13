using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SharpShell.SharpPropertySheet
{
    /// <summary>
    /// The SharpPropertyPage class is the base class for Property Pages used in 
    /// Shell Property Sheet Extension servers.
    /// </summary>
    public class SharpPropertyPage : UserControl
    {
        /// <summary>
        /// Gets or sets the page title.
        /// </summary>
        /// <value>
        /// The page title.
        /// </value>
        public string PageTitle { get; set; }

        /// <summary>
        /// Gets the page icon.
        /// </summary>
        public Icon PageIcon { get; set; }

        /// <summary>
        /// Called when the page is initialised.
        /// </summary>
        /// <param name="parent">The parent property sheet.</param>
        protected internal virtual void OnPropertyPageInitialised(SharpPropertySheet parent)
        {
            
        }

        /// <summary>
        /// Called when the property page is activated.
        /// </summary>
        protected internal virtual void OnPropertyPageSetActive()
        {

        }

        /// <summary>
        /// Called when the property page is de-activated or about to be closed.
        /// </summary>
        protected internal virtual void OnPropertyPageKillActive()
        {

        }

        /// <summary>
        /// Called when apply is pressed on the property sheet, or the property
        /// sheet is dismissed with the OK button.
        /// </summary>
        protected internal virtual void OnPropertySheetApply()
        {
            
        }

        /// <summary>
        /// Called when OK is pressed on the property sheet.
        /// </summary>
        protected internal virtual void OnPropertySheetOK()
        {
            
        }

        /// <summary>
        /// Called when Cancel is pressed on the property sheet.
        /// </summary>
        protected internal virtual void OnPropertySheetCancel()
        {
            
        }

        /// <summary>
        /// Called when the cross button on the property sheet is pressed.
        /// </summary>
        protected internal virtual void OnPropertySheetClose()
        {

        }

        /// <summary>
        /// Sets the page data changed.
        /// </summary>
        /// <param name="changed">if set to <c>true</c> the data has changed and the property sheet should enable the 'Apply' button.</param>
        protected void SetPageDataChanged(bool changed)
        {
            //  Pass on to the proxy.
            PropertyPageProxy.SetDataChanged(changed);
        }

        /// <summary>
        /// Gets or sets the proxy.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        internal PropertyPageProxy PropertyPageProxy { get; set; }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SharpPropertyPage
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "SharpPropertyPage";
            this.ResumeLayout(false);

        }
    }
}
