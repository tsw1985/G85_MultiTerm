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
        private int totalSplits = 0;
        private int selectedPanelIndex = 0;


        private enum SplitOrientation
        {
            Vertical,
            Horizontal
        }


        public MainForm()
        {
            InitializeComponent();
            // Inicializa el primer panel que ocupará toda la ventana
            Panel initialPanel = CreateNewPanel();

            Button initialButton = getNewButton(initialPanel);
            initialPanel.Controls.Add(initialButton);

            initialPanel.Dock = DockStyle.Fill;
            this.Controls.Add(initialPanel);


        }

        private Button getNewButton(Panel panel)
        {
            Button initialButton = new Button();
            initialButton.Text = "Button : " + Convert.ToString(totalSplits);
            return initialButton;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.E))
            {
                if(totalSplits == 0)
                {
                    totalSplits++;
                    FirstSplitPanel(SplitOrientation.Vertical);
                    
                }
                else if (totalSplits > 0)
                {
                    totalSplits++;
                    SplitPanel(SplitOrientation.Vertical);
                }
                
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.O))
            {

                if (totalSplits == 0)
                {
                    totalSplits++;
                    FirstSplitPanel(SplitOrientation.Horizontal);
                    
                }
                else if (totalSplits > 0)
                {
                    totalSplits++;
                    SplitPanel(SplitOrientation.Horizontal);
                }
                
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.W))
            {
                ClosePanel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

       

        private void FirstSplitPanel(SplitOrientation orientation)
        {
            if (selectedPanel != null)
            {
                //Clone current panel
                Panel clonedPanel = ClonePanelWithEvents(selectedPanel);

                //Remove existing controls on form ( first load )
                this.Controls.Clear();

                //Create new split container
                SplitContainer splitContainer = new SplitContainer
                {
                    Orientation = (orientation == SplitOrientation.Vertical) ? Orientation.Vertical : Orientation.Horizontal,
                    Dock        = DockStyle.Fill
                };

                //splitContainer.Panel1.MouseClick += Panel_1_MouseClick;
                //splitContainer.Panel2.MouseClick += Panel_2_MouseClick;


                //var newPanel1 = CreateNewPanel();
                var newPanel2 = CreateNewPanel();
                newPanel2.Controls.Add(getNewButton(newPanel2));

                splitContainer.Panel1.Controls.Add(clonedPanel);
                splitContainer.Panel2.Controls.Add(newPanel2);



                this.Controls.Add(splitContainer);

                //selectedPanel.Controls.Clear();
                //selectedPanel.Controls.Add(splitContainer);
                selectedPanel = null;
                
            }
        }

     

        private void SplitPanel(SplitOrientation orientation)
        {
            if (selectedPanel != null)
            {
                if (IsParentSplitContainer(selectedPanel))
                {

                    // 1 - Copio panel actual para luego pegarlo en el nuevo SplitContainer
                    Panel currentPanel = ClonePanelWithEvents((Panel)selectedPanel.Parent.Controls[0]);
                    if (currentPanel != null)
                    {

                        //Accedo al actual splitContainer
                        SplitContainer currentSplitContainer = (SplitContainer)selectedPanel.Parent.Parent;
                        if (currentSplitContainer != null)
                        {
                            //Accedo al padre del actual Panel , que puede ser PAnel1 o Panel2 dentro del actual Split Container
                            if (IsSplitPanel(selectedPanel))
                            {

                                //Una vez dentro accedo a el ( Panel1 o PAnel2) y borro su contenido.
                                
                                //selectedPanel.Parent.Controls.Clear();

                                //Creo un nuevo SplitContainer
                                SplitContainer newSplitContainer = new SplitContainer
                                {
                                    Orientation = (orientation == SplitOrientation.Vertical) ? Orientation.Vertical : Orientation.Horizontal,
                                    Dock = DockStyle.Fill
                                };

                                //y le meto el panel que estaba
                                newSplitContainer.Panel1.Controls.Add(currentPanel);

                                //Creo un nuevo panel que será nuevo , con un nuevo boton
                                var newPanel2 = CreateNewPanel();
                                newPanel2.MouseClick += Panel_MouseClick;
                                newPanel2.Controls.Add(getNewButton(newPanel2));

                                //En el panel2 le meto el nuevo panel
                                newSplitContainer.Panel2.Controls.Add(newPanel2);

                                //y en el panel que estabamos, pues meto un nuevo SplitContainer
                                selectedPanel.Parent.Controls.Add(newSplitContainer);

                                int a = 1;
                            }

                            //En el actual split container accedo al panel seleccionado y lo vacio


                            /*
                            //Elimino el contenido del panel actual
                            selectedPanel.Controls.Clear();
                            SplitContainer newSplitContainer = new SplitContainer
                            {
                                Orientation = (orientation == SplitOrientation.Vertical) ? Orientation.Vertical : Orientation.Horizontal,
                                Dock = DockStyle.Fill
                            };

                            newSplitContainer.Panel1.Controls.Add(currentPanel);
                            var newPanel2 = CreateNewPanel();
                            //newPanel2.MouseClick += Panel_MouseClick;
                            newPanel2.Controls.Add(getNewButton(newPanel2));
                            */

                            //currentPanel.Controls.Add(newSplitContainer);
                            //currentSplitContainer.Panel1.Controls.Add(newSplitContainer);
                            //newSplitContainer.Panel2.Controls.Add(newPanel2);

                        }
                    
                    }
                }
                else
                {
                    int b = 0;
                }
                
            }
        }

        private Panel ClonePanelWithEvents(Panel original)
        {
            Panel clonedPanel = new Panel
            {
                BorderStyle = original.BorderStyle,
                BackColor = original.BackColor,
                Dock = original.Dock,
                Size = original.Size,
                Location = original.Location
            };

            clonedPanel.MouseClick += Panel_MouseClick;

            foreach (Control ctrl in original.Controls)
            {
                Control clonedControl = CloneControlWithEvents(ctrl);
                clonedPanel.Controls.Add(clonedControl);
            }

            return clonedPanel;
        }

        private Control CloneControlWithEvents(Control originalControl)
        {
            if (originalControl is Button originalButton)
            {
                Button clonedButton = new Button
                {
                    Text = originalButton.Text,
                    Location = originalButton.Location,
                    Size = originalButton.Size
                };

                // Clonar manualmente el evento Click (u otros eventos)
                clonedButton.Click += (s, e) => originalButton.PerformClick();

                return clonedButton;
            }
            else if (originalControl is TextBox originalTextBox)
            {
                TextBox clonedTextBox = new TextBox
                {
                    Text = originalTextBox.Text,
                    Location = originalTextBox.Location,
                    Size = originalTextBox.Size
                };

                // Clonar otros eventos como necesario, ej: TextChanged
                clonedTextBox.TextChanged += (s, e) => originalTextBox.Text = clonedTextBox.Text;

                return clonedTextBox;
            }

            // Agregar aquí la clonación para otros tipos de controles y sus eventos si es necesario

            return originalControl; // Si el tipo de control no es manejado, simplemente retorna el control original.
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


        private bool IsSplitPanel(Panel panel)
        {
            bool isSplitContainer = false;
            if (panel != null)
            {
                if (typeof(SplitterPanel).IsInstanceOfType(panel.Parent))
                {
                    SplitterPanel sp = (SplitterPanel)panel.Parent;
                    isSplitContainer = true;
                }
            }
            return isSplitContainer;
        }

        private bool IsParentSplitContainer(Panel panel)
        {
            bool isParentSplitContainer = false;
            if (panel != null)
            {
                if (typeof(SplitContainer).IsInstanceOfType(panel.Parent.Parent)){
                    isParentSplitContainer=true;
                }
            }
            return isParentSplitContainer;
        }
    


        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("Click in panel");
            selectedPanel = sender as Panel;

        }

        private void Panel_1_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("Click in panel 111");
            selectedPanel = sender as Panel;
            selectedPanelIndex = 1;

        }

        private void Panel_2_MouseClick(object sender, MouseEventArgs e)
        {
            Debug.WriteLine("Click in panel 222");
            selectedPanel = sender as Panel;
            selectedPanelIndex = 2;

        }


    }
}
