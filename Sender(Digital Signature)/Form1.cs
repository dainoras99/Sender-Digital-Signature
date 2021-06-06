using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
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
        Encoding encoding = Encoding.GetEncoding("437");

        public string HashAndSignBytes()
        {
            try
            {
                byte[] dataToSign = encoding.GetBytes(messageTextBox.Text);

                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAParameters Key = RSAalg.ExportParameters(true);

                byte[] signedData = RSAalg.SignData(dataToSign, SHA256.Create());

                string signedString = encoding.GetString(signedData);

                return signedString;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public string doubleString()
        {
            string sendThisString = messageTextBox.Text + "AtSkYrImaSA!@345f" + HashAndSignBytes();
            return sendThisString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8;
        }


        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect(ipTextBox.Text, Convert.ToInt32(portTextBox.Text));
                connectButton.Enabled = false;
            }
            catch
            {
                MessageBox.Show("Receiver is turned off");
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            client.WriteLineAndGetReply(doubleString(), TimeSpan.FromSeconds(0));
        }
    }
}
