using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServerManager
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = CreateAboutInfo().ToString();
        }

        private StringBuilder CreateAboutInfo()
        {
            var builder = new StringBuilder();

            builder.AppendLine(@"SharpShell Server Manager");
            builder.AppendLine(@"Server Manager Version: " + typeof (ServerManagerForm).Assembly.GetName().Version);
            builder.AppendLine(@"SharpShell Version:" + typeof (SharpShell.ISharpShellServer).Assembly.GetName().Version);
            builder.AppendLine(@"Copyright (c) Dave Kerr 2013");
            builder.AppendLine(@"More Info: http://sharpshell.codeplex.com");

            return builder;
        }
    }
}
