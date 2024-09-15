using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace G85MultiTerm
{
    public partial class MainForm : Form
    {
        public static Panel selectedPanel;
        private ArrayList cmdList = new ArrayList();

        private int totalCmds = 0;
        private int cursorFocusCmdIndex = 0;

        public MainForm()
        {
            InitializeComponent();
            InitFirstPanel();
        }

        private void InitFirstPanel()
        {
            selectedPanel = CreateNewPanel();
            selectedPanel.Dock = DockStyle.Fill;
            this.Controls.Add(selectedPanel);
            totalCmds++;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Shift | Keys.E))
            {
                SplitSelectedPanel(Orientation.Vertical);
                totalCmds++;
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.O))
            {
                SplitSelectedPanel(Orientation.Horizontal);
                totalCmds++;
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.Up)) //arrow up
            {
                SetFocusOnCmd(this , cursorFocusCmdIndex);
                if (cursorFocusCmdIndex >= 0)
                {
                    cursorFocusCmdIndex--;
                }
                else
                {
                    cursorFocusCmdIndex = 0;
                }
                
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.Down)) //arrow down
            {
                SetFocusOnCmd(this , cursorFocusCmdIndex);
                if (cursorFocusCmdIndex <= totalCmds)
                {
                    cursorFocusCmdIndex++;
                }
                else
                {
                    cursorFocusCmdIndex = totalCmds;
                }
                
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.W))
            {
                CloseSelectedPanel();
                totalCmds--;
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void SetFocusOnCmd(Control mainWindow , int index)
        {

            if (mainWindow != null)
            {
                foreach(Control child in mainWindow.Controls)
                {
                    if (child != null && child is SplitContainer)
                    {

                        SplitContainer splitcontainerParent = (SplitContainer)child;
                        if(splitcontainerParent != null)
                        {
                            

                            Panel panel1 = splitcontainerParent.Panel1 as Panel;
                            if (panel1 != null)
                            {
                                SetFocusOnCmd(panel1, index);
                                //Debug.WriteLine("Split container tag " + splitcontainerParent.Tag);
                                
                                if (panel1.Controls[0] != null && panel1.Controls[0].Controls[0] != null)
                                {
                                    Cmd cmdChild = panel1.Controls[0].Controls[0] as Cmd;
                                    if (cmdChild != null && cmdChild.Tag.Equals("cmd_" + index.ToString()))
                                    {
                                        cmdChild.Controls[0].Controls[1].Controls[0].Focus();
                                        Debug.WriteLine("CMD founded in Panel 1 - TAG  : " + cmdChild.Tag);
                                        //break;
                                    }
                                }
                            }

                            Panel panel2 = splitcontainerParent.Panel2 as Panel;
                            if (panel2 != null)
                            {

                                SetFocusOnCmd(panel2, index);
                                //Debug.WriteLine("Split container tag " + splitcontainerParent.Tag);
                                if (panel2.Controls[0] != null && panel2.Controls[0].Controls[0] != null)
                                {
                                    Cmd cmdChild = panel2.Controls[0].Controls[0] as Cmd;
                                    if (cmdChild != null && cmdChild.Tag.Equals("cmd_" + index.ToString()))
                                    {
                                        cmdChild.Controls[0].Controls[1].Controls[0].Focus();
                                        Debug.WriteLine("CMD founded in Panel 2 - TAG  : " + cmdChild.Tag);
                                        //break;
                                    }
                                }
                                
                            }
                        }

                    }
                    
                }
            }
        }

        private void SplitSelectedPanel(Orientation orientation)
        {
            if (selectedPanel == null) return;

            
            // Crear nuevo SplitContainer
            SplitContainer splitContainer = new SplitContainer
            {
                Orientation = orientation,
                Dock = DockStyle.Fill,
                SplitterWidth = 5,
            };
            splitContainer.IsSplitterFixed = true;
            splitContainer.SplitterWidth = 1;


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

            //We access to the Parent PANEL of the splitContainer ( can be a SplitContainer , a splitContainer Panel or a WinForm - the first load )
            Control parentOfsplitContainerOfSelectedPanel = splitContainerOfSelectedPanel.Parent;

            //Remove the SplitContainer into this panel.
            parentOfsplitContainerOfSelectedPanel.Controls.Clear();
            panelToKeep.Dock = DockStyle.Fill;
            parentOfsplitContainerOfSelectedPanel.Controls.Add(panelToKeep);

            selectedPanel = panelToKeep as Panel;
            //totalCmds--;
        }

        private Panel CreateNewPanel()
        {
            var panel = new Panel
            {
                BorderStyle = BorderStyle.FixedSingle,
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };

            panel.MouseClick += Panel_MouseClick;

            
            Cmd cmd = new Cmd(totalCmds, selectedPanel);
            cmd.Dock = DockStyle.Fill;
            cmd.Tag = "cmd_" + Convert.ToString(totalCmds);
            panel.Controls.Add(cmd);


            //totalCmds++;

            return panel;
        }

        

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            selectedPanel = sender as Panel;
            Debug.WriteLine("Panel Selected");
        }
    }
}
