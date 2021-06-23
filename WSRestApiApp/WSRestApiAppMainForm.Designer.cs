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
            this.tbUri = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btUpdateUnit = new System.Windows.Forms.Button();
            this.btHalt = new System.Windows.Forms.Button();
            this.btExecuteUIAction = new System.Windows.Forms.Button();
            this.tbScheme = new System.Windows.Forms.TextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btUpdateJob
            // 
            this.btUpdateJob.Location = new System.Drawing.Point(41, 12);
            this.btUpdateJob.Name = "btUpdateJob";
            this.btUpdateJob.Size = new System.Drawing.Size(157, 27);
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
            this.textBox1.Location = new System.Drawing.Point(15, 203);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(773, 235);
            this.textBox1.TabIndex = 1;
            this.textBox1.WordWrap = false;
            // 
            // tbUri
            // 
            this.tbUri.BackColor = System.Drawing.SystemColors.Control;
            this.tbUri.Location = new System.Drawing.Point(320, 110);
            this.tbUri.Name = "tbUri";
            this.tbUri.Size = new System.Drawing.Size(333, 23);
            this.tbUri.TabIndex = 2;
            this.tbUri.Text = "https://myserver.geodigital.com:8379";
            this.tbUri.TextChanged += new System.EventHandler(this.WSUriTextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(318, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "WorkStudio Server Connection Information";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(450, 174);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(203, 23);
            this.tbPassword.TabIndex = 2;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(450, 145);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(203, 23);
            this.tbUserName.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(316, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "WorkStudio Username:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 177);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "WorkStudio Password:";
            // 
            // btUpdateUnit
            // 
            this.btUpdateUnit.Location = new System.Drawing.Point(41, 45);
            this.btUpdateUnit.Name = "btUpdateUnit";
            this.btUpdateUnit.Size = new System.Drawing.Size(157, 27);
            this.btUpdateUnit.TabIndex = 5;
            this.btUpdateUnit.Text = "Update Unit";
            this.btUpdateUnit.UseVisualStyleBackColor = true;
            this.btUpdateUnit.Click += new System.EventHandler(this.Execute);
            // 
            // btHalt
            // 
            this.btHalt.Enabled = false;
            this.btHalt.Location = new System.Drawing.Point(41, 134);
            this.btHalt.Name = "btHalt";
            this.btHalt.Size = new System.Drawing.Size(157, 42);
            this.btHalt.TabIndex = 5;
            this.btHalt.Text = "Halt";
            this.btHalt.UseVisualStyleBackColor = true;
            // 
            // btExecuteUIAction
            // 
            this.btExecuteUIAction.Location = new System.Drawing.Point(41, 79);
            this.btExecuteUIAction.Name = "btExecuteUIAction";
            this.btExecuteUIAction.Size = new System.Drawing.Size(157, 32);
            this.btExecuteUIAction.TabIndex = 6;
            this.btExecuteUIAction.Text = "Execute UIAction";
            this.btExecuteUIAction.UseVisualStyleBackColor = true;
            this.btExecuteUIAction.Click += new System.EventHandler(this.Execute);
            // 
            // tbScheme
            // 
            this.tbScheme.Location = new System.Drawing.Point(320, 53);
            this.tbScheme.Name = "tbScheme";
            this.tbScheme.Size = new System.Drawing.Size(86, 23);
            this.tbScheme.TabIndex = 7;
            this.tbScheme.TextChanged += new System.EventHandler(this.SchemeHostPortTextChanged);
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(430, 53);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(146, 23);
            this.tbHost.TabIndex = 8;
            this.tbHost.TextChanged += new System.EventHandler(this.SchemeHostPortTextChanged);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(593, 53);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(62, 23);
            this.tbPort.TabIndex = 9;
            this.tbPort.TextChanged += new System.EventHandler(this.SchemeHostPortTextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(409, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "://";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(582, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = ":";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(318, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 30);
            this.label6.TabIndex = 12;
            this.label6.Text = "Scheme\r\n(http or https)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(440, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 30);
            this.label7.TabIndex = 13;
            this.label7.Text = "Host\r\n(DNS or IP)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(593, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 15);
            this.label8.TabIndex = 14;
            this.label8.Text = "Port";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(320, 88);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(399, 15);
            this.label9.TabIndex = 15;
            this.label9.Text = "URI (this is generated from the Scheme, Host, and Port information above)";
            // 
            // WSRestApiAppMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.tbHost);
            this.Controls.Add(this.tbScheme);
            this.Controls.Add(this.btExecuteUIAction);
            this.Controls.Add(this.btHalt);
            this.Controls.Add(this.btUpdateUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbUserName);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbUri);
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
        private System.Windows.Forms.TextBox tbUri;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btUpdateUnit;
        private System.Windows.Forms.Button btHalt;
        private System.Windows.Forms.Button btExecuteUIAction;
        private System.Windows.Forms.TextBox tbScheme;
        private System.Windows.Forms.TextBox tbHost;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
    }
}

