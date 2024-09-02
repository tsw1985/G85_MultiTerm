using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace G85MultiTerm
{
    public partial class MainForm : Form
    {
        private Panel selectedPanel;
        private int totalButtons;

        public MainForm()
        {
            InitializeComponent();

            // Inicializa el primer panel que ocupará toda la ventana
            selectedPanel = CreateNewPanel();
            selectedPanel.Dock = DockStyle.Fill;
            this.Controls.Add(selectedPanel);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.E))
            {
                SplitSelectedPanel(Orientation.Vertical);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.O))
            {
                SplitSelectedPanel(Orientation.Horizontal);
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.W))
            {
                CloseSelectedPanel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SplitSelectedPanel(Orientation orientation)
        {
            if (selectedPanel == null) return;

            // Crear nuevo SplitContainer
            SplitContainer splitContainer = new SplitContainer
            {
                Orientation = orientation,
                Dock = DockStyle.Fill,
                SplitterWidth = 5
            };
            splitContainer.Tag = "P_" + Convert.ToString(totalButtons);



            // Mover el contenido del panel seleccionado al Panel1 del SplitContainer
            Control parent = selectedPanel.Parent;
            parent.Controls.Clear();
            splitContainer.Panel1.Controls.Add(selectedPanel);

            // Crear un nuevo panel y agregarlo al Panel2 del SplitContainer
            Panel newPanel = CreateNewPanel();
            splitContainer.Panel2.Controls.Add(newPanel);

            // Agregar el SplitContainer al padre del panel seleccionado
            parent.Controls.Add(splitContainer);

            // Actualizar la referencia del panel seleccionado
            selectedPanel = newPanel;
        }

        private void CloseSelectedPanel()
        {
            if (selectedPanel == null) return;

            SplitContainer splitContainerOfSelectedPanel = selectedPanel.Parent.Parent as SplitContainer;
            if (splitContainerOfSelectedPanel == null) return;

            Control panelToKeep;
            if (selectedPanel.Parent == splitContainerOfSelectedPanel.Panel1)
            {
                panelToKeep = splitContainerOfSelectedPanel.Panel2.Controls[0];
            }
            else
            {
                panelToKeep = splitContainerOfSelectedPanel.Panel1.Controls[0];
            }

            //We access to the Parent Panel of the splitContainer
            Control parentOfsplitContainerOfSelectedPanel = splitContainerOfSelectedPanel.Parent;

            //Remove the SplitContainer into this panel.
            parentOfsplitContainerOfSelectedPanel.Controls.Clear();
            panelToKeep.Dock = DockStyle.Fill;
            parentOfsplitContainerOfSelectedPanel.Controls.Add(panelToKeep);

            selectedPanel = panelToKeep as Panel;
        }

        private Panel CreateNewPanel()
        {
            var panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                //BackColor = Color.Black
            };

            panel.MouseClick += Panel_MouseClick;

            totalButtons++;
            Button initialButton = new Button
            {

                Text = "Button : " + Convert.ToString(totalButtons),
                //Dock = DockStyle.Top
            };

            panel.Controls.Add(initialButton);
            return panel;
        }

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            selectedPanel = sender as Panel;
            Debug.WriteLine("Panel Selected");
        }
    }
}
