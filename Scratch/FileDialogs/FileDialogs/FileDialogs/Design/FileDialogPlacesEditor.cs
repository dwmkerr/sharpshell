using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FileDialogs.Design
{
    internal class FileDialogPlacesEditor : CollectionEditor
    {
        #region Construction

        public FileDialogPlacesEditor(Type type)
            : base(type)
        {
        }

        #endregion

        #region Overriden Methods

        protected override CollectionEditor.CollectionForm CreateCollectionForm()
        {
            return new FileDialogPlacesEditorForm(this);
        }

        #endregion

        private class FileDialogPlacesEditorForm : CollectionForm
        {
            #region Member Fields

            private List<FileDialogPlaceBase> m_places = new List<FileDialogPlaceBase>();

            #endregion

            #region Construction

            public FileDialogPlacesEditorForm(CollectionEditor editor)
                : base(editor)
            {
                InitializeComponent();

                placesBar.Renderer = new FileDialogs.PlacesBarRenderer();

                List<KeyValuePair<string, SpecialFolder>> specialFolders = new List<KeyValuePair<string, SpecialFolder>>();
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("Desktop", SpecialFolder.Desktop));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Computer", SpecialFolder.MyComputer));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Network Places", SpecialFolder.Network));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Documents", SpecialFolder.MyDocuments));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Pictures", SpecialFolder.MyPictures));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Music", SpecialFolder.MyMusic));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Video", SpecialFolder.MyVideo));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("Favorites", SpecialFolder.Favorites));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("My Recent Documents", SpecialFolder.Recent));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("Recycle Bin", SpecialFolder.RecycleBin));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("Windows", SpecialFolder.Windows));
                specialFolders.Add(new KeyValuePair<string, SpecialFolder>("System", SpecialFolder.System));

                specialFoldersComboBox.DataSource = specialFolders;
                specialFoldersComboBox.SelectedIndex = 0;

                placesListBox.DataSource = m_places;
                placesListBox.DisplayMember = "Text";
            }

            #endregion

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileDialogPlacesEditor.FileDialogPlacesEditorForm));
                this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.addCustomPathTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.browseButton = new System.Windows.Forms.Button();
                this.customPathTextBox = new System.Windows.Forms.TextBox();
                this.customPlacesLabel = new System.Windows.Forms.Label();
                this.propsLabel = new System.Windows.Forms.Label();
                this.previewLabel = new System.Windows.Forms.Label();
                this.placesBar = new System.Windows.Forms.ToolStrip();
                this.addSpecialFolderTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.specialFoldersComboBox = new System.Windows.Forms.ComboBox();
                this.addSpecialFolderButton = new System.Windows.Forms.Button();
                this.specialFolderLabel = new System.Windows.Forms.Label();
                this.placesLabel = new System.Windows.Forms.Label();
                this.placesListBox = new System.Windows.Forms.ListBox();
                this.moveUpButton = new System.Windows.Forms.Button();
                this.moveDownButton = new System.Windows.Forms.Button();
                this.removeButton = new System.Windows.Forms.Button();
                this.selectedPlaceProps = new FileDialogs.Design.VsPropertyGrid(base.Context);
                this.okCancelTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
                this.okButton = new System.Windows.Forms.Button();
                this.cancelButton = new System.Windows.Forms.Button();
                this.addCustomPathButton = new System.Windows.Forms.Button();
                this.tableLayoutPanel.SuspendLayout();
                this.addCustomPathTableLayoutPanel.SuspendLayout();
                this.addSpecialFolderTableLayoutPanel.SuspendLayout();
                this.okCancelTableLayoutPanel.SuspendLayout();
                this.SuspendLayout();
                // 
                // tableLayoutPanel
                // 
                resources.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
                this.tableLayoutPanel.Controls.Add(this.addCustomPathTableLayoutPanel, 0, 3);
                this.tableLayoutPanel.Controls.Add(this.customPlacesLabel, 0, 2);
                this.tableLayoutPanel.Controls.Add(this.propsLabel, 2, 0);
                this.tableLayoutPanel.Controls.Add(this.previewLabel, 3, 0);
                this.tableLayoutPanel.Controls.Add(this.placesBar, 3, 1);
                this.tableLayoutPanel.Controls.Add(this.addSpecialFolderTableLayoutPanel, 0, 1);
                this.tableLayoutPanel.Controls.Add(this.specialFolderLabel, 0, 0);
                this.tableLayoutPanel.Controls.Add(this.placesLabel, 0, 5);
                this.tableLayoutPanel.Controls.Add(this.placesListBox, 0, 6);
                this.tableLayoutPanel.Controls.Add(this.moveUpButton, 1, 6);
                this.tableLayoutPanel.Controls.Add(this.moveDownButton, 1, 7);
                this.tableLayoutPanel.Controls.Add(this.removeButton, 1, 8);
                this.tableLayoutPanel.Controls.Add(this.selectedPlaceProps, 2, 1);
                this.tableLayoutPanel.Controls.Add(this.okCancelTableLayoutPanel, 0, 9);
                this.tableLayoutPanel.Controls.Add(this.addCustomPathButton, 0, 4);
                this.tableLayoutPanel.Name = "tableLayoutPanel";
                // 
                // addCustomPathTableLayoutPanel
                // 
                resources.ApplyResources(this.addCustomPathTableLayoutPanel, "addCustomPathTableLayoutPanel");
                this.addCustomPathTableLayoutPanel.Controls.Add(this.browseButton, 1, 0);
                this.addCustomPathTableLayoutPanel.Controls.Add(this.customPathTextBox, 0, 0);
                this.addCustomPathTableLayoutPanel.Name = "addCustomPathTableLayoutPanel";
                // 
                // browseButton
                // 
                resources.ApplyResources(this.browseButton, "browseButton");
                this.browseButton.Name = "browseButton";
                this.browseButton.UseVisualStyleBackColor = true;
                this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
                // 
                // customPathTextBox
                // 
                resources.ApplyResources(this.customPathTextBox, "customPathTextBox");
                this.customPathTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
                this.customPathTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
                this.customPathTextBox.Name = "customPathTextBox";
                this.customPathTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(customPathTextBox_KeyDown);
                this.customPathTextBox.TextChanged += new System.EventHandler(this.customPathTextBox_TextChanged);
                // 
                // customPlacesLabel
                // 
                resources.ApplyResources(this.customPlacesLabel, "customPlacesLabel");
                this.customPlacesLabel.Name = "customPlacesLabel";
                // 
                // propsLabel
                // 
                resources.ApplyResources(this.propsLabel, "propsLabel");
                this.propsLabel.Name = "propsLabel";
                // 
                // previewLabel
                // 
                resources.ApplyResources(this.previewLabel, "previewLabel");
                this.previewLabel.Name = "previewLabel";
                // 
                // placesBar
                // 
                resources.ApplyResources(this.placesBar, "placesBar");
                this.placesBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
                this.placesBar.ImageScalingSize = new System.Drawing.Size(32, 32);
                this.placesBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
                this.placesBar.Name = "placesBar";
                this.placesBar.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
                this.tableLayoutPanel.SetRowSpan(this.placesBar, 8);
                this.placesBar.Stretch = true;
                this.placesBar.TabStop = true;
                // 
                // addSpecialFolderTableLayoutPanel
                // 
                resources.ApplyResources(this.addSpecialFolderTableLayoutPanel, "addSpecialFolderTableLayoutPanel");
                this.addSpecialFolderTableLayoutPanel.Controls.Add(this.specialFoldersComboBox, 0, 0);
                this.addSpecialFolderTableLayoutPanel.Controls.Add(this.addSpecialFolderButton, 1, 0);
                this.addSpecialFolderTableLayoutPanel.Name = "addSpecialFolderTableLayoutPanel";
                // 
                // specialFoldersComboBox
                // 
                resources.ApplyResources(this.specialFoldersComboBox, "specialFoldersComboBox");
                this.specialFoldersComboBox.DisplayMember = "Key";
                this.specialFoldersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                this.specialFoldersComboBox.FormattingEnabled = true;
                this.specialFoldersComboBox.Name = "specialFoldersComboBox";
                this.specialFoldersComboBox.ValueMember = "Value";
                // 
                // addSpecialFolderButton
                // 
                resources.ApplyResources(this.addSpecialFolderButton, "addSpecialFolderButton");
                this.addSpecialFolderButton.Name = "addSpecialFolderButton";
                this.addSpecialFolderButton.UseVisualStyleBackColor = true;
                this.addSpecialFolderButton.Click += new System.EventHandler(this.addSpecialFolderButton_Click);
                // 
                // specialFolderLabel
                // 
                resources.ApplyResources(this.specialFolderLabel, "specialFolderLabel");
                this.specialFolderLabel.Name = "specialFolderLabel";
                // 
                // placesLabel
                // 
                resources.ApplyResources(this.placesLabel, "placesLabel");
                this.placesLabel.Name = "placesLabel";
                // 
                // placesListBox
                // 
                resources.ApplyResources(this.placesListBox, "placesListBox");
                this.placesListBox.Name = "placesListBox";
                this.tableLayoutPanel.SetRowSpan(this.placesListBox, 3);
                this.placesListBox.SelectedIndexChanged += new System.EventHandler(this.placesListBox_SelectedIndexChanged);
                // 
                // moveUpButton
                // 
                resources.ApplyResources(this.moveUpButton, "moveUpButton");
                this.moveUpButton.Name = "moveUpButton";
                this.moveUpButton.UseVisualStyleBackColor = true;
                this.moveUpButton.Click += new System.EventHandler(this.moveUpButton_Click);
                // 
                // moveDownButton
                // 
                resources.ApplyResources(this.moveDownButton, "moveDownButton");
                this.moveDownButton.Name = "moveDownButton";
                this.moveDownButton.UseVisualStyleBackColor = true;
                this.moveDownButton.Click += new System.EventHandler(this.moveDownButton_Click);
                // 
                // removeButton
                // 
                resources.ApplyResources(this.removeButton, "removeButton");
                this.removeButton.Name = "removeButton";
                this.removeButton.UseVisualStyleBackColor = true;
                this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
                // 
                // selectedPlaceProps
                // 
                this.selectedPlaceProps.CommandsVisibleIfAvailable = false;
                resources.ApplyResources(this.selectedPlaceProps, "selectedPlaceProps");
                this.selectedPlaceProps.Name = "selectedPlaceProps";
                this.tableLayoutPanel.SetRowSpan(this.selectedPlaceProps, 8);
                this.selectedPlaceProps.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.selectedPlaceProps_PropertyValueChanged);
                // 
                // okCancelTableLayoutPanel
                // 
                resources.ApplyResources(this.okCancelTableLayoutPanel, "okCancelTableLayoutPanel");
                this.tableLayoutPanel.SetColumnSpan(this.okCancelTableLayoutPanel, 4);
                this.okCancelTableLayoutPanel.Controls.Add(this.okButton, 0, 0);
                this.okCancelTableLayoutPanel.Controls.Add(this.cancelButton, 1, 0);
                this.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel";
                this.okCancelTableLayoutPanel.TabStop = true;
                // 
                // okButton
                // 
                resources.ApplyResources(this.okButton, "okButton");
                this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.okButton.Name = "okButton";
                this.okButton.UseVisualStyleBackColor = true;
                this.okButton.Click += new System.EventHandler(okButton_Click);
                // 
                // cancelButton
                // 
                resources.ApplyResources(this.cancelButton, "cancelButton");
                this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.cancelButton.Name = "cancelButton";
                this.cancelButton.UseVisualStyleBackColor = true;
                // 
                // addCustomPathButton
                // 
                resources.ApplyResources(this.addCustomPathButton, "addCustomPathButton");
                this.addCustomPathButton.Name = "addCustomPathButton";
                this.addCustomPathButton.UseVisualStyleBackColor = true;
                this.addCustomPathButton.Click += new System.EventHandler(this.addCustomPathButton_Click);
                // 
                // Form1
                // 
                this.AcceptButton = this.okButton;
                resources.ApplyResources(this, "$this");
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.CancelButton = this.cancelButton;
                this.Controls.Add(this.tableLayoutPanel);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "Form1";
                this.ShowIcon = false;
                this.ShowInTaskbar = false;
                this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
                this.tableLayoutPanel.ResumeLayout(false);
                this.tableLayoutPanel.PerformLayout();
                this.addCustomPathTableLayoutPanel.ResumeLayout(false);
                this.addCustomPathTableLayoutPanel.PerformLayout();
                this.addSpecialFolderTableLayoutPanel.ResumeLayout(false);
                this.addSpecialFolderTableLayoutPanel.PerformLayout();
                this.okCancelTableLayoutPanel.ResumeLayout(false);
                this.ResumeLayout(false);
                this.PerformLayout();

            }

            private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
            private System.Windows.Forms.TableLayoutPanel addSpecialFolderTableLayoutPanel;
            private System.Windows.Forms.ComboBox specialFoldersComboBox;
            private System.Windows.Forms.Button addSpecialFolderButton;
            private System.Windows.Forms.Label specialFolderLabel;
            private System.Windows.Forms.Label placesLabel;
            private System.Windows.Forms.ListBox placesListBox;
            private System.Windows.Forms.Button moveUpButton;
            private System.Windows.Forms.Button moveDownButton;
            private System.Windows.Forms.Button removeButton;
            private FileDialogs.Design.VsPropertyGrid selectedPlaceProps;
            private System.Windows.Forms.ToolStrip placesBar;
            private System.Windows.Forms.TableLayoutPanel okCancelTableLayoutPanel;
            private System.Windows.Forms.Button okButton;
            private System.Windows.Forms.Button cancelButton;
            private System.Windows.Forms.Label previewLabel;
            private System.Windows.Forms.Label propsLabel;
            private System.Windows.Forms.Label customPlacesLabel;
            private System.Windows.Forms.TableLayoutPanel addCustomPathTableLayoutPanel;
            private System.Windows.Forms.Button browseButton;
            private System.Windows.Forms.TextBox customPathTextBox;
            private System.Windows.Forms.Button addCustomPathButton;

            #endregion

            #region Common Methods

            private void AddPlace(FileDialogPlaceBase place)
            {
                m_places.Add(place);
                ((CurrencyManager)placesListBox.BindingContext[m_places]).Refresh();

                bool multipleLines;
                ToolStripButton placeButton = new ToolStripButton(FileDialog.InsertLineBreaks(place.Text, out multipleLines));
                placeButton.Image = ShellImageList.GetImage(place.PIDL);
                placeButton.ImageAlign = ContentAlignment.BottomCenter;
                placeButton.Margin = new Padding(1, 0, 0, 0);
                placeButton.Padding = new Padding(0, multipleLines ? 3 : 8, 0, multipleLines ? 0 : 8);
                placeButton.Tag = place;
                placeButton.TextImageRelation = TextImageRelation.ImageAboveText;
                placesBar.Items.Add(placeButton);

                placesListBox.ClearSelected();
                placesListBox.SelectedIndex = placesListBox.Items.Count - 1;
            }

            private void MovePlace(int fromIndex, int toIndex)
            {
                FileDialogPlaceBase place = (FileDialogPlaceBase)placesListBox.Items[fromIndex];
                ToolStripItem placeButton = placesBar.Items[fromIndex];

                m_places.RemoveAt(fromIndex);
                placesBar.Items.RemoveAt(fromIndex);

                m_places.Insert(toIndex, place);
                placesBar.Items.Insert(toIndex, placeButton);

                ((CurrencyManager)placesListBox.BindingContext[m_places]).Refresh();
            }

            private void RemovePlace(int index)
            {
                m_places.RemoveAt(index);
                placesBar.Items.RemoveAt(index);

                ((CurrencyManager)placesListBox.BindingContext[m_places]).Refresh();

                if (placesListBox.Items.Count > 0)
                {
                    placesListBox.ClearSelected();
                    index = Math.Max(0, Math.Min(index, placesListBox.Items.Count - 1));
                    placesListBox.SelectedIndex = index;
                }
            }

            #endregion

            #region Overriden Methods

            protected override void OnEditValueChanged()
            {
                if (Items.Length > 0)
                {
                    for (int i = 0; i < Items.Length; i++)
                    {
                        AddPlace((FileDialogPlaceBase)Items[i]);
                    }
                }
                else
                {
                    AddPlace(new FileDialogPlace(SpecialFolder.Desktop));
                    AddPlace(new FileDialogPlace(SpecialFolder.MyDocuments));
                    AddPlace(new FileDialogPlace(SpecialFolder.MyComputer));
                }
            }

            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                if (placesListBox.Items.Count > 0)
                    placesListBox.SelectedIndex = 0;
            }

            protected override bool ProcessDialogKey(Keys keyData)
            {
                if (keyData == Keys.Enter && customPathTextBox.Focused)
                {
                    return false;
                }
                return base.ProcessDialogKey(keyData);
            }

            #endregion

            #region Control Specific Methods

            private void addSpecialFolderButton_Click(object sender, EventArgs e)
            {
                AddPlace(new FileDialogPlace((SpecialFolder)specialFoldersComboBox.SelectedValue));
            }

            private void customPathTextBox_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    addCustomPathButton.PerformClick();
                }
            }

            private void customPathTextBox_TextChanged(object sender, EventArgs e)
            {
                addCustomPathButton.Enabled = customPathTextBox.Text.Trim().Length > 0;
            }

            private void browseButton_Click(object sender, EventArgs e)
            {
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    customPathTextBox.Text = dlg.SelectedPath;
            }

            private void addCustomPathButton_Click(object sender, EventArgs e)
            {
                string path = customPathTextBox.Text;
                if (!Directory.Exists(path))
                {
                    MessageBox.Show(this, "The directory at the specified path doesn't exist.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AddPlace(new CustomFileDialogPlace(path));
                customPathTextBox.Clear();
            }

            private void placesListBox_SelectedIndexChanged(object sender, EventArgs e)
            {
                selectedPlaceProps.SelectedObject = placesListBox.SelectedItem;

                int selectedIndex = placesListBox.SelectedIndex;

                moveUpButton.Enabled = (selectedIndex > 0);
                moveDownButton.Enabled = (selectedIndex >= 0 && selectedIndex < placesListBox.Items.Count - 1);
                removeButton.Enabled = (selectedIndex >= 0);
            }

            private void moveUpButton_Click(object sender, EventArgs e)
            {
                int index = placesListBox.SelectedIndex;
                MovePlace(index, --index);
                placesListBox.SelectedIndex = index;
            }

            private void moveDownButton_Click(object sender, EventArgs e)
            {
                int index = placesListBox.SelectedIndex;
                MovePlace(index, ++index);
                placesListBox.SelectedIndex = index;
            }

            private void removeButton_Click(object sender, EventArgs e)
            {
                RemovePlace(placesListBox.SelectedIndex);
            }

            private void selectedPlaceProps_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
            {
                FileDialogPlaceBase place = (FileDialogPlaceBase)placesListBox.SelectedItem;
                ((CurrencyManager)placesListBox.BindingContext[m_places]).Refresh();

                bool multipleLines;
                ToolStripButton placeButton = (ToolStripButton)placesBar.Items[placesListBox.SelectedIndex];
                placeButton.Text = FileDialog.InsertLineBreaks(place.Text, out multipleLines);
                placeButton.Padding = new Padding(0, multipleLines ? 3 : 8, 0, multipleLines ? 0 : 8);
                if (e.ChangedItem.Label == "Path")
                    placeButton.Image = ShellImageList.GetImage(place.PIDL);
            }

            private void okButton_Click(object sender, EventArgs e)
            {
                if (placesListBox.Items.Count == 0)
                {
                    MessageBox.Show(this, "The places bar must at least contain one place.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.None;
                    return;
                }

                object[] places = new object[placesListBox.Items.Count];
                for (int i = 0; i < placesListBox.Items.Count; i++)
                {
                    places[i] = placesListBox.Items[i];
                }

                Items = places;
            }

            #endregion
        }
    }
}
