using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FileDialogs
{
    /// <summary>
    /// Renderer for the tool strip.
    /// </summary>
    internal class ToolStripFileDialogRenderer : ToolStripSystemRenderer
    {
        #region Member Fields

        private ToolStripProfessionalRenderer renderer = new ToolStripProfessionalRenderer();

        #endregion

        #region Methods

        protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
        {
            renderer.DrawItemImage(e);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is ToolStripDropDown)
                renderer.DrawToolStripBorder(e);
        }

        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            renderer.DrawImageMargin(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            if (e.Item is ToolStripSeparator)
            {
                if (!e.Vertical)
                {
                    ToolStripDropDownMenu parent = e.Item.GetCurrentParent() as ToolStripDropDownMenu;
                    if (parent != null)
                    {
                        renderer.DrawSeparator(e);
                        return;
                    }
                }
            }

            base.OnRenderSeparator(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            renderer.DrawMenuItemBackground(e);
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if ((e.Item is ToolStripMenuItem) && (e.Item.Selected || e.Item.Pressed))
                e.TextColor = e.Item.ForeColor;

            base.OnRenderItemText(e);
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripDropDownItem item = e.Item as ToolStripDropDownItem;
            if (item != null && item.Pressed && item.HasDropDownItems)
            {
                renderer.DrawDropDownButtonBackground(e);
                return;
            }

            base.OnRenderDropDownButtonBackground(e);
        }

        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            ToolStripSplitButton item = e.Item as ToolStripSplitButton;
            if (item != null && item.DropDownButtonPressed && item.HasDropDownItems)
            {
                renderer.DrawSplitButton(e);
                return;
            }

            base.OnRenderSplitButtonBackground(e);
        }

        #endregion
    }

    /// <summary>
    /// Renderer for the places bar.
    /// </summary>
    internal class PlacesBarRenderer : ToolStripSystemRenderer
    {
        #region Methods

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            Rectangle rect = e.AffectedBounds;
            Color borderColor = VisualStyleInformation.TextControlBorder;
            int borderWidth = 1;

            ControlPaint.DrawBorder(e.Graphics, rect, borderColor, borderWidth,
                                    ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid,
                                    borderColor, borderWidth, ButtonBorderStyle.Solid, borderColor,
                                    borderWidth, ButtonBorderStyle.Solid);
        }

        #endregion
    }
}
