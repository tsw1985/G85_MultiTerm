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

namespace G85MultiTerm
{
    public partial class MainForm : Form
    {
        private Panel selectedPanel;

        public MainForm()
        {
            InitializeComponent();
            // Inicializa el primer panel que ocupará toda la ventana
            var initialPanel = CreateNewPanel();
            initialPanel.Dock = DockStyle.Fill;
            this.Controls.Add(initialPanel);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.E))
            {
                SplitPanel(SplitOrientation.Vertical);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.O))
            {
                SplitPanel(SplitOrientation.Horizontal);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.W))
            {
                ClosePanel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private enum SplitOrientation
        {
            Vertical,
            Horizontal
        }

        private void SplitPanel(SplitOrientation orientation)
        {
            if (selectedPanel != null)
            {
                SplitContainer splitContainer = new SplitContainer
                {
                    Orientation = (orientation == SplitOrientation.Vertical)
                        ? Orientation.Vertical
                        : Orientation.Horizontal,
                    Dock = DockStyle.Fill
                };

                var newPanel1 = CreateNewPanel();
                var newPanel2 = CreateNewPanel();

                splitContainer.Panel1.Controls.Add(newPanel1);
                splitContainer.Panel2.Controls.Add(newPanel2);

                selectedPanel.Controls.Clear();
                selectedPanel.Controls.Add(splitContainer);
                selectedPanel = null;  // Desmarca el panel después de dividir
            }
        }

        private void ClosePanel()
        {
            if (selectedPanel != null && selectedPanel.Parent is SplitContainer splitContainer)
            {
                var parent = splitContainer.Parent;

                if (parent is Panel parentPanel)
                {
                    Control remainingPanel = (selectedPanel == splitContainer.Panel1) ? splitContainer.Panel2.Controls[0] : splitContainer.Panel1.Controls[0];

                    parentPanel.Controls.Clear();
                    remainingPanel.Dock = DockStyle.Fill;
                    parentPanel.Controls.Add(remainingPanel);

                    selectedPanel = null;
                }
                else if (parent is SplitContainer parentSplitContainer)
                {
                    Control remainingPanel = (selectedPanel == splitContainer.Panel1) ? splitContainer.Panel2.Controls[0] : splitContainer.Panel1.Controls[0];

                    parentSplitContainer.Panel1.Controls.Clear();
                    parentSplitContainer.Panel2.Controls.Clear();

                    if (selectedPanel == splitContainer.Panel1)
                    {
                        parentSplitContainer.Panel1.Controls.Add(remainingPanel);
                    }
                    else
                    {
                        parentSplitContainer.Panel2.Controls.Add(remainingPanel);
                    }

                    selectedPanel = null;
                }
            }
        }

        private Panel CreateNewPanel()
        {
            var panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                //BackColor = System.Drawing.Color.Black
            };
            panel.MouseClick += Panel_MouseClick;

            // Crear la consola en el panel

            return panel;
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            selectedPanel = sender as Panel;
        }


    }
}
