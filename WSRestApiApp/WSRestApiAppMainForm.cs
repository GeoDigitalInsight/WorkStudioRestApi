using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WSRestApiApp
{
    public partial class WSRestApiAppMainForm : Form
    {
        public WSRestApiAppMainForm()
        {
            InitializeComponent();
        }

        private void btPerformTest_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            UriBuilder url = new UriBuilder(tbWSServer.Text);
            url.Path = "DDOProtocol/CREATEEDITSESSION";
            sb.AppendLine(url.Host);
            sb.AppendLine(url.ToString());



            textBox1.Text = sb.ToString();
        }
    }
}
