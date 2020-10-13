namespace WSRestApiApp
{
    partial class WSRestApiAppMainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btPerformTest = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbWSServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btPerformTest
            // 
            this.btPerformTest.Location = new System.Drawing.Point(41, 28);
            this.btPerformTest.Name = "btPerformTest";
            this.btPerformTest.Size = new System.Drawing.Size(157, 43);
            this.btPerformTest.TabIndex = 0;
            this.btPerformTest.Text = "Perform Test";
            this.btPerformTest.UseVisualStyleBackColor = true;
            this.btPerformTest.Click += new System.EventHandler(this.btPerformTest_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(41, 94);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(708, 321);
            this.textBox1.TabIndex = 1;
            this.textBox1.WordWrap = false;
            // 
            // tbWSServer
            // 
            this.tbWSServer.Location = new System.Drawing.Point(318, 35);
            this.tbWSServer.Name = "tbWSServer";
            this.tbWSServer.Size = new System.Drawing.Size(396, 23);
            this.tbWSServer.TabIndex = 2;
            this.tbWSServer.Text = "https://myserver.geodigital.com:8379";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(318, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(313, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "WorkStudio Server (https://myserver.geodigital.com:8379)";
            // 
            // WSRestApiAppMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWSServer);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btPerformTest);
            this.Name = "WSRestApiAppMainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WSRestApiAppMainForm_FormClosed);
            this.Load += new System.EventHandler(this.WSRestApiAppMainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btPerformTest;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tbWSServer;
        private System.Windows.Forms.Label label1;
    }
}

