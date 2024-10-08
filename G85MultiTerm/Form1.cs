﻿using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace G85MultiTerm
{
    public partial class MainForm : Form
    {
        public static Panel selectedPanel;
        public static Cmd selectedCmd;
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
                cursorFocusCmdIndex = totalCmds;
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.O))
            {
                SplitSelectedPanel(Orientation.Horizontal);
                totalCmds++;
                cursorFocusCmdIndex = totalCmds;
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.W))
            {
                CloseSelectedPanel();
                totalCmds--;
                cursorFocusCmdIndex = totalCmds;
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.Up)) //arrow up
            {
                cursorFocusCmdIndex--;

                if (cursorFocusCmdIndex == cmdList.Count - 1)
                {
                    cursorFocusCmdIndex--;
                }

                if (cursorFocusCmdIndex < 0)
                {
                    cursorFocusCmdIndex = 0;
                }

                Cmd cmd = (Cmd)cmdList[cursorFocusCmdIndex];
                if (cmd != null)
                {

                    MainForm.selectedCmd = cmd;

                    TextBox cmdCommands = (TextBox)cmd.Controls[0].Controls[1].Controls[0];
                    cmdCommands.BackColor = System.Drawing.Color.Green;
                    cmdCommands.Refresh();
                    System.Threading.Thread.Sleep(150);
                    cmdCommands.BackColor = System.Drawing.Color.White;
                    cmdCommands.Refresh();
                    cmdCommands.Focus();
                }
    
                
            }
            else if (keyData == (Keys.Control | Keys.Shift | Keys.Down)) //arrow down
            {
                int index = 0;
                cursorFocusCmdIndex++;
                if (cursorFocusCmdIndex >= cmdList.Count)
                {
                    cursorFocusCmdIndex = cmdList.Count - 1;
                }

                    Cmd cmd = (Cmd)cmdList[cursorFocusCmdIndex];
                if (cmd != null)
                {

                    MainForm.selectedCmd = cmd;

                    TextBox cmdCommands = (TextBox)cmd.Controls[0].Controls[1].Controls[0];
                    cmdCommands.BackColor = System.Drawing.Color.Green;
                    cmdCommands.Refresh();
                    System.Threading.Thread.Sleep(150);
                    cmdCommands.BackColor = System.Drawing.Color.White;
                    cmdCommands.Refresh();
                    cmdCommands.Focus();

                }

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

            Cmd cmdToRemove = selectedPanel.Controls[0] as Cmd;
            if (cmdToRemove != null)
            {
                cmdList.Remove(cmdToRemove);
            }


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
            if (selectedPanel != null)
            {
                //set focus on current cmd
                Cmd cmd = selectedPanel.Controls[0] as Cmd;
                if (cmd != null)
                {
                    SplitContainer splitContainer = cmd.Controls[0] as SplitContainer;
                    if (splitContainer != null)
                    {
                        TextBox cmdCommandTextBoxPanel2 = splitContainer.Panel2.Controls[0] as TextBox;
                        if (cmdCommandTextBoxPanel2 != null)
                        {
                            cmdCommandTextBoxPanel2.Focus();
                        }
                    }
                }
            }
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
            panel.Controls.Add(cmd);
            cmdList.Add(cmd);
            return panel;
        }
        

        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            selectedPanel = sender as Panel;
            Debug.WriteLine("Panel Selected");
        }
    }
}
