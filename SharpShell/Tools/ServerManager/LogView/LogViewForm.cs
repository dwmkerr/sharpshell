using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerManager.LogView
{
    public partial class LogViewForm : Form
    {
        public LogViewForm()
        {
            InitializeComponent();

            uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            //  Get the sharp shell event log.
            sharpShellEventLog = EventLog.GetEventLogs().FirstOrDefault(el => el.Log == "Application");

            Closing += new CancelEventHandler(LogViewForm_Closing);
        }

        void LogViewForm_Closing(object sender, CancelEventArgs e)
        {
            //  Do we have an event log?
            if (sharpShellEventLog != null)
            {
                //  Unregister the event.
                sharpShellEventLog.EnableRaisingEvents = false;
                sharpShellEventLog.EntryWritten -= sharpShellEventLog_EntryWritten;
            }
        }

        private void LogViewForm_Load(object sender, EventArgs e)
        {
            //  If we don't an event log one, we cannot log.
            if (sharpShellEventLog == null)
                return;

            //  Register the event.
            sharpShellEventLog.EnableRaisingEvents = true;
            sharpShellEventLog.EntryWritten += sharpShellEventLog_EntryWritten;
        }

        void sharpShellEventLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            if (e.Entry.Source != SharpShell.Diagnostics.Logging.EventLog_Source)
                return;

            //  Add the event.
            var task = new Task( () => listViewEvents.Items.Add(new ListViewItem(new string[] {e.Entry.TimeWritten.ToString(), e.Entry.EntryType.ToString(), e.Entry.Message}) { Tag = e}));
            task.Start(uiScheduler);
        }

        private readonly TaskScheduler uiScheduler;

        private readonly EventLog sharpShellEventLog;

        private void listViewEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewEvents.SelectedIndices.Count == 0)
                return;

            var entry = (EventLogEntry) listViewEvents.SelectedItems[0].Tag;
            textBoxDetails.Text = entry.Message;
        }
    }
}
