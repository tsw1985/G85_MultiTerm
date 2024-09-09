using System;
using System.ComponentModel;
using System.ComponentModel.Design;
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

        public Cmd()
        {
            InitializeComponent();  // Inicializa los componentes
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
            this.splitContainer1.SplitterDistance = 582;
            this.splitContainer1.TabIndex = 0;
            // 
            // responseCommandTextBox
            // 
            this.responseCommandTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.responseCommandTextBox.Location = new System.Drawing.Point(0, 0);
            this.responseCommandTextBox.Multiline = true;
            this.responseCommandTextBox.Name = "responseCommandTextBox";
            this.responseCommandTextBox.Size = new System.Drawing.Size(1343, 582);
            this.responseCommandTextBox.TabIndex = 0;
            // 
            // commandTextBox
            // 
            this.commandTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandTextBox.Location = new System.Drawing.Point(0, 0);
            this.commandTextBox.Name = "commandTextBox";
            this.commandTextBox.Size = new System.Drawing.Size(1343, 20);
            this.commandTextBox.TabIndex = 0;
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
