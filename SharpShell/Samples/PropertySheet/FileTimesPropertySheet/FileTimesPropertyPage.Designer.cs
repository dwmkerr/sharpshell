namespace FileTimesPropertySheet
{
    partial class FileTimesPropertyPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerCreatedDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerCreatedTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerModifiedTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerModifiedDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePickerAccessedTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerAccessedDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonTouch = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Time Created";
            // 
            // dateTimePickerCreatedDate
            // 
            this.dateTimePickerCreatedDate.Location = new System.Drawing.Point(20, 48);
            this.dateTimePickerCreatedDate.Name = "dateTimePickerCreatedDate";
            this.dateTimePickerCreatedDate.Size = new System.Drawing.Size(141, 20);
            this.dateTimePickerCreatedDate.TabIndex = 1;
            this.dateTimePickerCreatedDate.ValueChanged += new System.EventHandler(this.OnAnyDateTimeControlValueChanged);
            // 
            // dateTimePickerCreatedTime
            // 
            this.dateTimePickerCreatedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerCreatedTime.Location = new System.Drawing.Point(167, 48);
            this.dateTimePickerCreatedTime.Name = "dateTimePickerCreatedTime";
            this.dateTimePickerCreatedTime.ShowUpDown = true;
            this.dateTimePickerCreatedTime.Size = new System.Drawing.Size(75, 20);
            this.dateTimePickerCreatedTime.TabIndex = 2;
            this.dateTimePickerCreatedTime.ValueChanged += new System.EventHandler(this.OnAnyDateTimeControlValueChanged);
            // 
            // dateTimePickerModifiedTime
            // 
            this.dateTimePickerModifiedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerModifiedTime.Location = new System.Drawing.Point(167, 112);
            this.dateTimePickerModifiedTime.Name = "dateTimePickerModifiedTime";
            this.dateTimePickerModifiedTime.ShowUpDown = true;
            this.dateTimePickerModifiedTime.Size = new System.Drawing.Size(75, 20);
            this.dateTimePickerModifiedTime.TabIndex = 5;
            this.dateTimePickerModifiedTime.ValueChanged += new System.EventHandler(this.OnAnyDateTimeControlValueChanged);
            // 
            // dateTimePickerModifiedDate
            // 
            this.dateTimePickerModifiedDate.Location = new System.Drawing.Point(20, 112);
            this.dateTimePickerModifiedDate.Name = "dateTimePickerModifiedDate";
            this.dateTimePickerModifiedDate.Size = new System.Drawing.Size(141, 20);
            this.dateTimePickerModifiedDate.TabIndex = 4;
            this.dateTimePickerModifiedDate.ValueChanged += new System.EventHandler(this.OnAnyDateTimeControlValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Time Modified";
            // 
            // dateTimePickerAccessedTime
            // 
            this.dateTimePickerAccessedTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerAccessedTime.Location = new System.Drawing.Point(167, 178);
            this.dateTimePickerAccessedTime.Name = "dateTimePickerAccessedTime";
            this.dateTimePickerAccessedTime.ShowUpDown = true;
            this.dateTimePickerAccessedTime.Size = new System.Drawing.Size(75, 20);
            this.dateTimePickerAccessedTime.TabIndex = 8;
            this.dateTimePickerAccessedTime.ValueChanged += new System.EventHandler(this.OnAnyDateTimeControlValueChanged);
            // 
            // dateTimePickerAccessedDate
            // 
            this.dateTimePickerAccessedDate.Location = new System.Drawing.Point(20, 178);
            this.dateTimePickerAccessedDate.Name = "dateTimePickerAccessedDate";
            this.dateTimePickerAccessedDate.Size = new System.Drawing.Size(141, 20);
            this.dateTimePickerAccessedDate.TabIndex = 7;
            this.dateTimePickerAccessedDate.ValueChanged += new System.EventHandler(this.OnAnyDateTimeControlValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Time Accessed";
            // 
            // buttonTouch
            // 
            this.buttonTouch.Location = new System.Drawing.Point(20, 215);
            this.buttonTouch.Name = "buttonTouch";
            this.buttonTouch.Size = new System.Drawing.Size(75, 20);
            this.buttonTouch.TabIndex = 9;
            this.buttonTouch.Text = "Touch";
            this.buttonTouch.UseVisualStyleBackColor = true;
            this.buttonTouch.Click += new System.EventHandler(this.buttonTouch_Click);
            // 
            // FileTimesPropertyPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.buttonTouch);
            this.Controls.Add(this.dateTimePickerAccessedTime);
            this.Controls.Add(this.dateTimePickerAccessedDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimePickerModifiedTime);
            this.Controls.Add(this.dateTimePickerModifiedDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePickerCreatedTime);
            this.Controls.Add(this.dateTimePickerCreatedDate);
            this.Controls.Add(this.label1);
            this.Name = "FileTimesPropertyPage";
            this.Size = new System.Drawing.Size(339, 422);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerCreatedDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerCreatedTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerModifiedTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerModifiedDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePickerAccessedTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerAccessedDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonTouch;

    }
}
