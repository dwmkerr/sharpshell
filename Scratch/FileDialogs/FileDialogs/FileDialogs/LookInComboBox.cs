using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace FileDialogs
{
    /// <summary>
    /// A custom combo box that can display icons for each item.
    /// </summary>
    [ToolboxItem(false)]
    internal class LookInComboBox : ComboBox
    {
        #region Member Fields

        // The width of the indent of the ComboItems
        private int m_indentWidth = 10;

        // The item for the browser's current selected directory
        private LookInComboBoxItem m_currentItem;

        #endregion

        #region Construction

        public LookInComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += ComboBox_DrawItem;

            DropDown += ComboBox_DropDown;
            MouseWheel += ComboBox_MouseWheel;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This method will change the currentItem field once a new item is selected
        /// </summary>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndex >= 0)
                m_currentItem = SelectedItem as LookInComboBoxItem;
            
            base.OnSelectedIndexChanged(e);
        }

        /// <summary>
        /// This method will draw the items of the DropDownList. It will draw the icon, the text and
        /// with the indent that goes with the item
        /// </summary>
        private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == -1) // The combo box contains no items and the item is the editing portion
            {
                if (m_currentItem == null)
                    return;

                Brush backBrush = new SolidBrush(SystemColors.Window);
                e.Graphics.FillRectangle(backBrush, e.Bounds);
                backBrush.Dispose();

                int imageYOffset = (e.Bounds.Height - m_currentItem.Icon.Height) / 2;
                Point imagePoint = new Point(
                    e.Bounds.Left + 2,
                    e.Bounds.Top + imageYOffset);

                Size textSize = TextRenderer.MeasureText(m_currentItem.Text, e.Font);
                int textYOffset = (e.Bounds.Height - textSize.Height) / 2;
                Point textPoint = new Point(
                    e.Bounds.Left + m_currentItem.Icon.Width + 5,
                    e.Bounds.Top + textYOffset);
                textSize.Height += textYOffset;
                Rectangle textRect = new Rectangle(textPoint, textSize);

                bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
                if (selected)
                {
                    Brush selectedBrush = new SolidBrush(SystemColors.Highlight);
                    e.Graphics.FillRectangle(selectedBrush, textRect);
                    selectedBrush.Dispose();
                }

                if (((e.State & DrawItemState.Focus) == DrawItemState.Focus) && ((e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect))
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, textRect, e.ForeColor, e.BackColor);
                }

                e.Graphics.DrawIcon(selected ? m_currentItem.SelectedIcon : m_currentItem.Icon, imagePoint.X, imagePoint.Y);
                TextRenderer.DrawText(e.Graphics, m_currentItem.Text, e.Font, textPoint, e.ForeColor);
            }
            else
            {
                LookInComboBoxItem item = (LookInComboBoxItem)Items[e.Index];

                Brush backBrush = new SolidBrush(SystemColors.Window);
                e.Graphics.FillRectangle(backBrush, e.Bounds);
                backBrush.Dispose();

                int indentOffset = m_indentWidth * item.Indent;

                if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
                    indentOffset = 0;

                int imageYOffset = (e.Bounds.Height - item.Icon.Height) / 2;
                Point imagePoint = new Point(
                    e.Bounds.Left + indentOffset + 2,
                    e.Bounds.Top + imageYOffset);

                Size textSize = TextRenderer.MeasureText(item.Text, e.Font);
                int textYOffset = (e.Bounds.Height - textSize.Height) / 2;
                Point textPoint = new Point(
                    e.Bounds.Left + item.Icon.Width + indentOffset + 5,
                    e.Bounds.Top + textYOffset);
                textSize.Height += textYOffset;
                Rectangle textRect = new Rectangle(textPoint, textSize);

                bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
                if (selected)
                {
                    Brush selectedBrush = new SolidBrush(SystemColors.Highlight);
                    e.Graphics.FillRectangle(selectedBrush, textRect);
                    selectedBrush.Dispose();
                }

                if (((e.State & DrawItemState.Focus) == DrawItemState.Focus) && ((e.State & DrawItemState.NoFocusRect) != DrawItemState.NoFocusRect))
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, textRect, e.ForeColor, e.BackColor);
                }

                e.Graphics.DrawIcon(selected ? item.SelectedIcon : item.Icon, imagePoint.X, imagePoint.Y);
                TextRenderer.DrawText(e.Graphics, item.Text, e.Font, textPoint, e.ForeColor);
            }
        }

        /// <summary>
        /// This method will make sure that when the ComboBox is dropped down, the width of the DropDownList
        /// will be sufficient to fit all items
        /// </summary>
        private void ComboBox_DropDown(object sender, EventArgs e)
        {
            // Calculate drop down width
            int ddWidth = 0;
            Graphics g = CreateGraphics();
            foreach (LookInComboBoxItem item in Items)
            {
                int itemWidth =
                    g.MeasureString(item.Text, Font).ToSize().Width +
                    item.Icon.Width +
                    m_indentWidth * item.Indent +
                    (Items.Count > MaxDropDownItems ? SystemInformation.VerticalScrollBarWidth : 0);

                if (itemWidth > ddWidth)
                    ddWidth = itemWidth;
            }

            DropDownWidth = (ddWidth > Width) ? ddWidth : Width;

            // Calculate drop down height
            int ddHeight = Items.Count * ItemHeight + 2;
            Rectangle comboRect = RectangleToScreen(ClientRectangle);
            if (comboRect.Bottom + ddHeight > Screen.PrimaryScreen.WorkingArea.Height)
                ddHeight = Screen.PrimaryScreen.WorkingArea.Height - comboRect.Bottom;

            DropDownHeight = ((ddHeight - 2) / ItemHeight) * ItemHeight + 2;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_CTLCOLORLISTBOX)
            {
                Rectangle comboRect = RectangleToScreen(ClientRectangle);
                int ddX = comboRect.Left;
                int ddY = comboRect.Bottom;

                if (ddX < 0)
                    ddX = 0;
                else if (ddX + DropDownWidth > Screen.PrimaryScreen.WorkingArea.Width)
                    ddX = Screen.PrimaryScreen.WorkingArea.Width - DropDownWidth;

                NativeMethods.User32.SetWindowPos(
                    new HandleRef(null, m.LParam),
                    NativeMethods.NullHandleRef,
                    ddX, ddY, 0, 0,
                    NativeMethods.SWP_NOSIZE);
            }

            base.WndProc(ref m);
        }

        private void ComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public LookInComboBoxItem CurrentItem
        {
            get { return m_currentItem; }
            set { m_currentItem = value; }
        }

        #endregion
    }

    internal class LookInComboBoxItem
    {
        #region Member Fields

        // The text, PIDL and indent that goes with the ComboBoxItem
        private string m_text;
        private IntPtr m_pidl;
        private int m_indent;

        // The icon that has to be drawn for this ComboBoxItem
        private Icon m_icon;
        private Icon m_selectedIcon;

        #endregion

        #region Construction

        public LookInComboBoxItem(string text, IntPtr pidl, int indent)
        {
            m_text = text;
            m_pidl = pidl;
            m_indent = indent;
            m_icon = ShellImageList.GetIcon(ShellImageList.GetIconIndex(pidl, false, false), ShellImageListSize.Small);
            m_selectedIcon = ShellImageList.GetIcon(ShellImageList.GetIconIndex(pidl, true, false), ShellImageListSize.Small);
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return m_text;
        }

        #endregion

        #region Properties

        public IntPtr PIDL
        {
            get { return m_pidl; }
        }

        public int Indent
        {
            get { return m_indent; }
        }

        public Icon Icon
        {
            get { return m_icon; }
        }

        public Icon SelectedIcon
        {
            get { return m_selectedIcon; }
        }

        public string Text
        {
            get { return m_text; }
        }

        #endregion
    }
}
