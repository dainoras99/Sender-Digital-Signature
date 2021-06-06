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
        RSAParameters Key;

        public string HashAndSignBytes()
        {
            try
            {
                Key = new RSAParameters();
                byte[] dataToSign = encoding.GetBytes(messageTextBox.Text);

                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                Key = RSAalg.ExportParameters(true);

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

        public string correctedString()
        {
            string splitter = "AtSkYrImaSA!@345f";

            string sendThisString = messageTextBox.Text + splitter + HashAndSignBytes();

            string modulus = encoding.GetString(Key.Modulus);
            string exponent = encoding.GetString(Key.Exponent);
            string p = encoding.GetString(Key.P);
            string q = encoding.GetString(Key.Q);
            string d = encoding.GetString(Key.D);
            string dp = encoding.GetString(Key.DP);
            string dq = encoding.GetString(Key.DQ);
            string inverseq = encoding.GetString(Key.InverseQ);

            sendThisString += splitter + modulus + splitter + exponent + splitter + p
                + splitter + q + splitter + d + splitter + dp + splitter + dq + splitter +
                inverseq;
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
            client.WriteLineAndGetReply(correctedString(), TimeSpan.FromSeconds(0));
        }
    }
}
