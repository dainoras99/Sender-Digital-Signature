using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sender_Digital_Signature_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
        }


        private void connectButton_Click(object sender, EventArgs e)
        {
            client.Connect(ipTextBox.Text, Convert.ToInt32(portTextBox.Text));
            connectButton.Enabled = false;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(messageTextBox.Text, TimeSpan.FromSeconds(3));
        }
    }
}
