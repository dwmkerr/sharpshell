using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FileDialogs
{
    /// <summary>
    /// Represents a combination of a standard button on the left and a drop-down
    /// button on the right, or the other way around if the value of RightToLeft is Yes.
    /// </summary>
    internal class SplitButton : Button
    {
        #region Constants & Member Fields

        private const int PushButtonWidth = 12;
        private static readonly int BorderSize = SystemInformation.Border3DSize.Width * 2;

        private PushButtonState m_state;
        private Rectangle m_dropDownRectangle = new Rectangle();

        private bool m_showSplit = false;
        private bool m_skipNextOpen = false;

        private int m_clickedItemIndex = -1;
        private int m_menuItemIndex = -1;

        #endregion

        #region Construction

        public SplitButton()
        {
            base.AutoSize = true;
        }

        #endregion

        #region Methods

        private void PaintArrow(Graphics g, Rectangle dropDownRect)
        {
            Point middle =
                new Point(Convert.ToInt32(dropDownRect.Left + dropDownRect.Width / 2),
                          Convert.ToInt32(dropDownRect.Top + dropDownRect.Height / 2));

            // If the width is odd - favor pushing it over one pixel right.
            middle.X += (dropDownRect.Width % 2);

            Point[] arrow =
                new Point[]
                    {
                        new Point(middle.X - 2, middle.Y - 1), new Point(middle.X + 3, middle.Y - 1),
                        new Point(middle.X, middle.Y + 2)
                    };

            g.FillPolygon(State == PushButtonState.Disabled ? SystemBrushes.ControlDark : SystemBrushes.ControlText,
                          arrow);
        }

        private void ShowContextMenuStrip()
        {
            if (m_skipNextOpen)
            {
                // The dropdown button was clicked and the context menu strip was closing
                m_skipNextOpen = false;
                return;
            }

            State = PushButtonState.Pressed;

            if (ContextMenuStrip != null)
            {
                ContextMenuStrip.ItemClicked += ContextMenuStrip_ItemClicked;
                ContextMenuStrip.Closing += ContextMenuStrip_Closing;
                ContextMenuStrip.Show(this, new Point(0, Height), ToolStripDropDownDirection.BelowRight);
            }
        }

        private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
            if (contextMenuStrip != null)
                contextMenuStrip.ItemClicked -= ContextMenuStrip_ItemClicked;

            if (contextMenuStrip == null)
                m_menuItemIndex = -1;
            else
                m_menuItemIndex = contextMenuStrip.Items.IndexOf(e.ClickedItem);
            
            PerformClick();
        }

        private void ContextMenuStrip_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            ContextMenuStrip contextMenuStrip = sender as ContextMenuStrip;
            if (contextMenuStrip != null)
                contextMenuStrip.Closing -= ContextMenuStrip_Closing;

            SetButtonDrawState();

            if (e.CloseReason == ToolStripDropDownCloseReason.AppClicked)
                m_skipNextOpen = (m_dropDownRectangle.Contains(PointToClient(Cursor.Position)));
        }

        private void SetButtonDrawState()
        {
            if (!Enabled)
                State = PushButtonState.Disabled;
            // If handle is not created Parent.PointToClient causes it to be created
            else if (Parent != null && Parent.IsHandleCreated && Bounds.Contains(Parent.PointToClient(Cursor.Position)))
                State = PushButtonState.Hot;
            else if (Focused || IsAcceptButton)
                State = PushButtonState.Default;
            else
                State = PushButtonState.Normal;
        }

        public override void NotifyDefault(bool value)
        {
            base.NotifyDefault(value);

            if (m_showSplit)
                SetButtonDrawState();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            if (m_menuItemIndex == -1)
                m_clickedItemIndex = -1;
            else
            {
                m_clickedItemIndex = m_menuItemIndex;
                m_menuItemIndex = -1;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (!m_showSplit)
            {
                base.OnGotFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
                State = PushButtonState.Default;
            
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (!m_showSplit)
            {
                base.OnLostFocus(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                if (IsDefault)
                    State = PushButtonState.Default;
                else
                    State = PushButtonState.Normal;
            }
            
            Invalidate();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize = base.GetPreferredSize(proposedSize);

            if (m_showSplit && !string.IsNullOrEmpty(Text) &&
                TextRenderer.MeasureText(Text, Font).Width + PushButtonWidth > preferredSize.Width)
            {
                return preferredSize + new Size(PushButtonWidth + BorderSize * 2, 0);
            }

            return preferredSize;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData == Keys.Down || keyData == Keys.Return) && m_showSplit)
                return true;
            else
                return base.IsInputKey(keyData);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (m_showSplit)
            {
                if (e.KeyData == Keys.Down)
                    ShowContextMenuStrip();
                else if (e.KeyData == Keys.Space || e.KeyData == Keys.Return)
                    State = PushButtonState.Pressed;
            }

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space || e.KeyData == Keys.Return)
                State = Focused ? PushButtonState.Default : PushButtonState.Normal;

            if (e.KeyData == Keys.Return)
                PerformClick();

            base.OnKeyUp(e);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            if (!m_showSplit)
            {
                base.OnMouseEnter(e);
                return;
            }

            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
                State = PushButtonState.Hot;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!m_showSplit)
            {
                base.OnMouseLeave(e);
                return;
            }


            if (!State.Equals(PushButtonState.Pressed) && !State.Equals(PushButtonState.Disabled))
            {
                if (Focused || IsAcceptButton)
                    State = PushButtonState.Default;
                else
                    State = PushButtonState.Normal;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!m_showSplit)
            {
                base.OnMouseDown(e);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                if (m_dropDownRectangle.Contains(e.Location))
                    ShowContextMenuStrip();
                else
                    State = PushButtonState.Pressed;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!m_showSplit)
            {
                base.OnMouseUp(e);
                return;
            }

            if (ContextMenuStrip == null || !ContextMenuStrip.Visible && e.Button == MouseButtons.Left)
            {
                SetButtonDrawState();

                if (Bounds.Contains(Parent.PointToClient(Cursor.Position)) &&
                    !m_dropDownRectangle.Contains(e.Location))
                {
                    PerformClick();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!m_showSplit)
                return;

            Graphics g = e.Graphics;
            Rectangle bounds = ClientRectangle;

            // Draw the button background as according to the current state.
            if (State != PushButtonState.Pressed && IsAcceptButton && !Application.RenderWithVisualStyles)
            {
                Rectangle backgroundBounds = bounds;
                backgroundBounds.Inflate(-1, -1);
                ButtonRenderer.DrawButton(g, backgroundBounds, State);

                // Button renderer doesnt draw the black frame when themes are off =(
                g.DrawRectangle(SystemPens.WindowFrame, 0, 0, bounds.Width - 1, bounds.Height - 1);
            }
            else
            {
                ButtonRenderer.DrawButton(g, bounds, State);
            }

            // Calculate the current dropdown rectangle.
            m_dropDownRectangle =
                new Rectangle(bounds.Right - PushButtonWidth - 1, BorderSize, PushButtonWidth,
                              bounds.Height - BorderSize * 2);

            int internalBorder = BorderSize;
            Rectangle focusRect =
                new Rectangle(internalBorder,
                              internalBorder,
                              bounds.Width - m_dropDownRectangle.Width - internalBorder - 1,
                              bounds.Height - (internalBorder * 2));

            bool drawSplitLine = (State == PushButtonState.Hot || State == PushButtonState.Pressed || State == PushButtonState.Default ||
                                  !Application.RenderWithVisualStyles);

            if (RightToLeft == RightToLeft.Yes)
            {
                m_dropDownRectangle.X = bounds.Left + 1;
                focusRect.X = m_dropDownRectangle.Right;

                if (drawSplitLine)
                {
                    // Draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Left + PushButtonWidth, BorderSize,
                               bounds.Left + PushButtonWidth, bounds.Bottom - BorderSize - 1);
                    g.DrawLine(SystemPens.ButtonHighlight, bounds.Left + PushButtonWidth - 1, BorderSize,
                               bounds.Left + PushButtonWidth - 1, bounds.Bottom - BorderSize - 1);
                }
            }

            else
            {
                if (drawSplitLine)
                {
                    // Draw two lines at the edge of the dropdown button
                    g.DrawLine(SystemPens.ButtonShadow, bounds.Right - PushButtonWidth, BorderSize,
                               bounds.Right - PushButtonWidth, bounds.Bottom - BorderSize - 1);
                    g.DrawLine(SystemPens.ButtonHighlight, bounds.Right - PushButtonWidth + 1, BorderSize,
                               bounds.Right - PushButtonWidth + 1, bounds.Bottom - BorderSize - 1);
                }
            }

            // Draw an arrow in the correct location 
            PaintArrow(g, m_dropDownRectangle);

            // Figure out how to draw the text
            TextFormatFlags formatFlags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;

            // If we dont' use mnemonic, set formatFlag to NoPrefix as this will show ampersand.
            if (!UseMnemonic)
                formatFlags = formatFlags | TextFormatFlags.NoPrefix;
            else if (!ShowKeyboardCues)
                formatFlags = formatFlags | TextFormatFlags.HidePrefix;

            if (!string.IsNullOrEmpty(Text))
                TextRenderer.DrawText(g, Text, Font, focusRect,
                                      State == PushButtonState.Disabled
                                          ? SystemColors.ControlDark
                                          : SystemColors.ControlText, formatFlags);

            // Draw the focus rectangle.
            if (ContextMenuStrip == null || !ContextMenuStrip.Visible && Focused)
                ControlPaint.DrawFocusRectangle(g, focusRect);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_CONTEXTMENU)
                return;

            base.WndProc(ref m);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating weather the drop-down should be visible.
        /// </summary>
        [Category("Behaviour")]
        [DefaultValue(false)]
        public bool ShowSplit
        {
            set
            {
                if (value != m_showSplit)
                {
                    m_showSplit = value;

                    Invalidate();

                    if (Parent != null)
                        Parent.PerformLayout();
                }
            }
        }

        /// <summary>
        /// Gets the index of the clicked menu item or -1 if the button was clicked.
        /// </summary>
        public int ClickedItemIndex
        {
            get { return m_clickedItemIndex; }
        }

        private PushButtonState State
        {
            get { return m_state; }
            set
            {
                if (!m_state.Equals(value))
                {
                    m_state = value;

                    Invalidate();
                }
            }
        }

        private bool IsAcceptButton
        {
            get
            {
                Form form = FindForm();
                return (form != null && form.AcceptButton == this);
            }
        }

        public new bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                base.Enabled = value;
                SetButtonDrawState();
            }
        }

        #endregion
    }
}
