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
            this.btUpdateJob = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.tbWSServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btUpdateUnit = new System.Windows.Forms.Button();
            this.btHalt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btUpdateJob
            // 
            this.btUpdateJob.Location = new System.Drawing.Point(41, 12);
            this.btUpdateJob.Name = "btUpdateJob";
            this.btUpdateJob.Size = new System.Drawing.Size(157, 43);
            this.btUpdateJob.TabIndex = 0;
            this.btUpdateJob.Text = "Update Job";
            this.btUpdateJob.UseVisualStyleBackColor = true;
            this.btUpdateJob.Click += new System.EventHandler(this.Execute);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(15, 136);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(773, 302);
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
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(501, 93);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(213, 23);
            this.tbPassword.TabIndex = 2;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(501, 64);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(213, 23);
            this.tbUserName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(411, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(411, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // btUpdateUnit
            // 
            this.btUpdateUnit.Location = new System.Drawing.Point(41, 69);
            this.btUpdateUnit.Name = "btUpdateUnit";
            this.btUpdateUnit.Size = new System.Drawing.Size(157, 42);
            this.btUpdateUnit.TabIndex = 5;
            this.btUpdateUnit.Text = "Update Unit";
            this.btUpdateUnit.UseVisualStyleBackColor = true;
            this.btUpdateUnit.Click += new System.EventHandler(this.Execute);
            // 
            // btHalt
            // 
            this.btHalt.Enabled = false;
            this.btHalt.Location = new System.Drawing.Point(218, 69);
            this.btHalt.Name = "btHalt";
            this.btHalt.Size = new System.Drawing.Size(157, 42);
            this.btHalt.TabIndex = 5;
            this.btHalt.Text = "Halt";
            this.btHalt.UseVisualStyleBackColor = true;
            // 
            // WSRestApiAppMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btHalt);
            this.Controls.Add(this.btUpdateUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbWSServer);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btUpdateJob);
            this.Name = "WSRestApiAppMainForm";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WSRestApiAppMainForm_FormClosed);
            this.Load += new System.EventHandler(this.WSRestApiAppMainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btUpdateJob;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tbWSServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btUpdateUnit;
        private System.Windows.Forms.Button btHalt;
    }
}

