using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RF680_Connect;
using System.Net.Sockets;
using NLog;
using System.IO;

namespace RFID_2
{
    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public Form1()
        {
            InitializeComponent();
        }

        TcpClient _Client;
        NetworkStream _nwStream;
        Receiver _Receiver;

        public int id = 0;


        private void button1_Click(object sender, EventArgs e)
        {
            //RF680_Connect.Math r = new RF680_Connect.Math();
            //Console.WriteLine(r.Add(3, 5));

            //Console.WriteLine( RF680_Connect.Cmds.HostGreet("Hallo"));
            logger.Info("´Button 1 klicked");

            string IP = "141.79.224.84";
            int port = 10001;

            _Client = new TcpClient(IP, port);
            _nwStream = _Client.GetStream();
            // connect established

            _Receiver = new Receiver(_nwStream);

           // _Receiver.DataReceived += OnDataReceived;
            { // Greeting
                SendCmd(_nwStream, Cmds.HostGreet(id));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.ShowDialog();
        }

        private string SendCmd(NetworkStream nwStream, string Message)
        {
            SendData(nwStream, Message);
            return "";
        }

        private void SendData(NetworkStream nwStream, string Message)
        {
            byte[] sendData = ASCIIEncoding.ASCII.GetBytes(Message);

            Console.WriteLine(Message);
            nwStream.Write(sendData, 0, sendData.Length);
        }

        private string RecieveData(NetworkStream nwStream)
        {

            string res = "";
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buffer = new byte[2048];
                int count = 0;

                while (nwStream.DataAvailable)
                {
                    count = nwStream.Read(buffer, 0, buffer.Length);
                    ms.Write(buffer, 0, count);
                }
                res = ASCIIEncoding.ASCII.GetString(ms.ToArray(), 0, ms.ToArray().Length);
            }

            Console.WriteLine(res);

            return res;
        }
    }


}
