using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MSDNMagazineSampleNamespaceExtension
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	internal class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Panel panel1;

		public Class1 m_folder = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(8, 16);
			this.button1.Name = "button1";
			this.button1.TabIndex = 0;
			this.button1.Text = "button1";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(144, 120);
			this.button2.Name = "button2";
			this.button2.TabIndex = 1;
			this.button2.Text = "button2";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(292, 273);
			this.panel1.TabIndex = 2;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.ControlBox = false;
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.ShowInTaskbar = false;
			this.TopMost = true;
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public void OnAction(MSDNMagazine.Shell.ShellActionType t)
		{

			if ( t == MSDNMagazine.Shell.ShellActionType.Open ) 
			{

				try 
				{
					System.IO.StreamWriter sw = new System.IO.StreamWriter( this.m_folder.SelectedItem, false );

					sw.WriteLine( Guid.NewGuid().ToString() );
					sw.Close();
				}
				catch
				{
				}
			}

			if ( t == MSDNMagazine.Shell.ShellActionType.Save ) 
			{
				MessageBox.Show("save");
			}

			if ( t == MSDNMagazine.Shell.ShellActionType.Unknown ) 
			{
				MessageBox.Show("beats the hell out of me!");
			}


		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show("click!");
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.m_folder.SelectedItem = "c:\\temp\\test\\foo.txt";
			
		}
	}
}
