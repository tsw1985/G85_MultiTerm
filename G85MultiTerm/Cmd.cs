using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace G85MultiTerm
{
    // Aquí usamos UserControl en lugar de Panel para una mejor integración con el diseñador
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class Cmd : UserControl
    {
        private SplitContainer splitContainer1;
        private TextBox responseCommandTextBox;
        private TextBox commandTextBox;
        private Process cmdProcess;
        private int index;
        private Panel selectedPanel;

        public Cmd(int index , Panel selectedPanel)
        {
            InitializeComponent();  // Inicializa los componentes
            InitializeCmdProcess();
            this.index = index;
            this.selectedPanel = selectedPanel;
        }

        private void ResponseCommandTextBox_Click(object sender, EventArgs e)
        {
            SelectParentPanel();
        }

        private void CommandTextBox_Enter(object sender, EventArgs e)
        {
            SelectParentPanel();
        }

        private void SelectParentPanel()
        {
            MainForm.selectedPanel = (Panel)this.Parent;
            MainForm.selectedCmd = (Cmd)this;
        }

        private void InitializeCmdProcess()
        {
            cmdProcess = new Process();
            cmdProcess.StartInfo.FileName = "cmd.exe";
            cmdProcess.StartInfo.RedirectStandardInput = true;
            cmdProcess.StartInfo.RedirectStandardOutput = true;
            cmdProcess.StartInfo.RedirectStandardError = true;
            cmdProcess.StartInfo.UseShellExecute = false;
            cmdProcess.StartInfo.CreateNoWindow = true;
            cmdProcess.OutputDataReceived += CmdProcess_OutputDataReceived;
            cmdProcess.ErrorDataReceived += CmdProcess_OutputDataReceived;
            cmdProcess.Start();
            cmdProcess.BeginOutputReadLine();
            
        }

        private void CmdProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Invoke((MethodInvoker)(() =>
                {
                    responseCommandTextBox.AppendText(e.Data + Environment.NewLine);
                    commandTextBox.Text = "";
                    commandTextBox.Focus();
                    //commandTextBox.Text = "CMD N - " + index;
                }));
            }
        }

        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (commandTextBox.Text.ToLower().Equals("cls"))
                {
                    responseCommandTextBox.Text = "";
                    commandTextBox.Text = "";
                    InitializeCmdProcess();
                }
                else
                {
                    string command = commandTextBox.Text;
                    cmdProcess.StandardInput.WriteLine(command);
                    cmdProcess.StandardInput.Flush();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }

                
            }
        }

        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.responseCommandTextBox = new System.Windows.Forms.TextBox();
            this.commandTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.responseCommandTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.commandTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(1343, 642);
            this.splitContainer1.SplitterDistance = 610;
            this.splitContainer1.TabIndex = 0;
            // 
            // responseCommandTextBox
            // 
            this.responseCommandTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.responseCommandTextBox.Location = new System.Drawing.Point(0, 0);
            this.responseCommandTextBox.Multiline = true;
            this.responseCommandTextBox.Name = "responseCommandTextBox";
            this.responseCommandTextBox.ReadOnly = true;
            this.responseCommandTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.responseCommandTextBox.Size = new System.Drawing.Size(1343, 610);
            this.responseCommandTextBox.TabIndex = 0;
            this.responseCommandTextBox.Click += new System.EventHandler(this.ResponseCommandTextBox_Click);
            // 
            // commandTextBox
            // 
            this.commandTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandTextBox.Location = new System.Drawing.Point(0, 0);
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.Size = new System.Drawing.Size(1343, 20);
            this.commandTextBox.TabIndex = 0;
            this.commandTextBox.Enter += new System.EventHandler(this.CommandTextBox_Enter);
            this.commandTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandTextBox_KeyDown);
            // 
            // Cmd
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "Cmd";
            this.Size = new System.Drawing.Size(1343, 642);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        
    }
}

