
namespace kiemtrakytu
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.cmd_input = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label11 = new System.Windows.Forms.Label();
            this.TB_Write_x = new System.Windows.Forms.TextBox();
            this.TB_Write_y = new System.Windows.Forms.TextBox();
            this.TB_Write_z = new System.Windows.Forms.TextBox();
            this.TB_Read_x = new System.Windows.Forms.TextBox();
            this.TB_Read_y = new System.Windows.Forms.TextBox();
            this.TB_Read_z = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.cmd_run = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmd_input
            // 
            this.cmd_input.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmd_input.Location = new System.Drawing.Point(45, 27);
            this.cmd_input.MaximumSize = new System.Drawing.Size(500, 500);
            this.cmd_input.Multiline = true;
            this.cmd_input.Name = "cmd_input";
            this.cmd_input.Size = new System.Drawing.Size(351, 69);
            this.cmd_input.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(578, 369);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(193, 23);
            this.progressBar1.TabIndex = 11;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label11.Location = new System.Drawing.Point(452, 370);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(118, 17);
            this.label11.TabIndex = 10;
            this.label11.Text = "Status Connect";
            // 
            // TB_Write_x
            // 
            this.TB_Write_x.Location = new System.Drawing.Point(499, 223);
            this.TB_Write_x.Name = "TB_Write_x";
            this.TB_Write_x.Size = new System.Drawing.Size(95, 20);
            this.TB_Write_x.TabIndex = 12;
            // 
            // TB_Write_y
            // 
            this.TB_Write_y.Location = new System.Drawing.Point(499, 260);
            this.TB_Write_y.Name = "TB_Write_y";
            this.TB_Write_y.Size = new System.Drawing.Size(95, 20);
            this.TB_Write_y.TabIndex = 12;
            // 
            // TB_Write_z
            // 
            this.TB_Write_z.Location = new System.Drawing.Point(499, 300);
            this.TB_Write_z.Name = "TB_Write_z";
            this.TB_Write_z.Size = new System.Drawing.Size(95, 20);
            this.TB_Write_z.TabIndex = 12;
            // 
            // TB_Read_x
            // 
            this.TB_Read_x.Location = new System.Drawing.Point(668, 223);
            this.TB_Read_x.Name = "TB_Read_x";
            this.TB_Read_x.Size = new System.Drawing.Size(95, 20);
            this.TB_Read_x.TabIndex = 12;
            // 
            // TB_Read_y
            // 
            this.TB_Read_y.Location = new System.Drawing.Point(668, 260);
            this.TB_Read_y.Name = "TB_Read_y";
            this.TB_Read_y.Size = new System.Drawing.Size(95, 20);
            this.TB_Read_y.TabIndex = 12;
            // 
            // TB_Read_z
            // 
            this.TB_Read_z.Location = new System.Drawing.Point(668, 300);
            this.TB_Read_z.Name = "TB_Read_z";
            this.TB_Read_z.Size = new System.Drawing.Size(95, 20);
            this.TB_Read_z.TabIndex = 12;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(565, 135);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 57);
            this.button1.TabIndex = 5;
            this.button1.Text = "RUN";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // cmd_run
            // 
            this.cmd_run.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmd_run.Location = new System.Drawing.Point(45, 135);
            this.cmd_run.MaximumSize = new System.Drawing.Size(500, 500);
            this.cmd_run.Multiline = true;
            this.cmd_run.Name = "cmd_run";
            this.cmd_run.Size = new System.Drawing.Size(351, 251);
            this.cmd_run.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TB_Read_z);
            this.Controls.Add(this.TB_Read_y);
            this.Controls.Add(this.TB_Write_z);
            this.Controls.Add(this.TB_Read_x);
            this.Controls.Add(this.TB_Write_y);
            this.Controls.Add(this.TB_Write_x);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmd_run);
            this.Controls.Add(this.cmd_input);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox cmd_input;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox TB_Write_x;
        private System.Windows.Forms.TextBox TB_Write_y;
        private System.Windows.Forms.TextBox TB_Write_z;
        private System.Windows.Forms.TextBox TB_Read_x;
        private System.Windows.Forms.TextBox TB_Read_y;
        private System.Windows.Forms.TextBox TB_Read_z;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox cmd_run;
    }
}

