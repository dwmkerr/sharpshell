using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharpShell.SharpPropertySheet;

namespace FileTimesPropertySheet
{
    /// <summary>
    /// The FileTimesPropertyPage class.
    /// </summary>
    public partial class FileTimesPropertyPage : SharpPropertyPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileTimesPropertyPage"/> class.
        /// </summary>
        public FileTimesPropertyPage()
        {
            InitializeComponent();

            //  Set the page title.
            PageTitle = "File Times";

            //  Note: You can also set the icon to be used:
            //  PageIcon = Properties.Resources.SomeIcon;
        }

        /// <summary>
        /// Called when the page is initialised.
        /// </summary>
        /// <param name="parent">The parent property sheet.</param>
        protected override void OnPropertyPageInitialised(SharpPropertySheet parent)
        {
            //  Store the file path.
            filePath = parent.SelectedItemPaths.First();

            //  Load the file times into the dialog.
            LoadFileTimes();
        }

        /// <summary>
        /// Called when apply is pressed on the property sheet, or the property
        /// sheet is dismissed with the OK button.
        /// </summary>
        protected override void OnPropertySheetApply()
        {
            //  Save the changes.
            SaveFileTimes();
        }

        /// <summary>
        /// Called when OK is pressed on the property sheet.
        /// </summary>
        protected override void OnPropertySheetOK()
        {
            //  Save the changes.
            SaveFileTimes();
        }
        
        /// <summary>
        /// Handles the Click event of the buttonTouch control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonTouch_Click(object sender, System.EventArgs e)
        {
            //  Set the modified and accessed date.
            var timeNow = DateTime.Now;
            dateTimePickerModifiedDate.Value = timeNow;
            dateTimePickerModifiedTime.Value = timeNow;
            dateTimePickerAccessedDate.Value = timeNow;
            dateTimePickerAccessedTime.Value = timeNow;

            //  Save the file times.
            SaveFileTimes();
        }

        /// <summary>
        /// Loads the file times.
        /// </summary>
        private void LoadFileTimes()
        {
            //  While we're initialising, disable activation of the apply button (otherwise
            //  just by setting the control values, we'll activate the 'apply' button).
            disableActivationOfApplyButton = true;

            //  Get the file info.
            var fileInfo = new FileInfo(filePath);

            //  Set the file properties.
            dateTimePickerCreatedDate.Value = fileInfo.CreationTime;
            dateTimePickerCreatedTime.Value = fileInfo.CreationTime;
            dateTimePickerModifiedDate.Value = fileInfo.LastWriteTime;
            dateTimePickerModifiedTime.Value = fileInfo.LastWriteTime;
            dateTimePickerAccessedDate.Value = fileInfo.LastAccessTime;
            dateTimePickerAccessedTime.Value = fileInfo.LastAccessTime;

            //  We can now let the Apply button be activated if needed.
            disableActivationOfApplyButton = false;
        }

        /// <summary>
        /// Saves the file times.
        /// </summary>
        private void SaveFileTimes()
        {
            //  Get the file info.
            var fileInfo = new FileInfo(filePath);

            //  Set the file properties.
            fileInfo.CreationTime = DateTimeFromDateAndTime(dateTimePickerCreatedDate.Value, dateTimePickerCreatedTime.Value);
            fileInfo.LastWriteTime = DateTimeFromDateAndTime(dateTimePickerModifiedDate.Value, dateTimePickerModifiedTime.Value);
            fileInfo.LastAccessTime = DateTimeFromDateAndTime(dateTimePickerAccessedDate.Value, dateTimePickerAccessedTime.Value);
        }

        /// <summary>
        /// Creates a DateTime from a date and a time.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="time">The time.</param>
        /// <returns>A DateTime from the date and time.</returns>
        private static DateTime DateTimeFromDateAndTime(DateTime date, DateTime time)
        {
            return new DateTime(date.Year, date.Month, date.Day,
                time.Hour, time.Minute, time.Second, time.Millisecond);
        }

        /// <summary>
        /// The file path.
        /// </summary>
        private string filePath;

        /// <summary>
        /// If true, disable activation of the appy button when a value changes.
        /// </summary>
        private bool disableActivationOfApplyButton;

        /// <summary>
        /// Called when any date time control value is changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnAnyDateTimeControlValueChanged(object sender, EventArgs e)
        {
            //  Enable the 'apply' button, but only if we haven't disabled that functionality.
            if (!disableActivationOfApplyButton)
                SetPageDataChanged(true);
        }
    }
}
