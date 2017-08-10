using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RFID_2
{

    public sealed class Receiver
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal event EventHandler<MyEventArg> DataReceived;

        internal Receiver(NetworkStream stream)
        {
            _Stream = stream;
            _Thread = new Thread(Run);
            logger.Info("Starting Thread");
            _Thread.Start();
        }

        private void Run()
        {
            while (true)
            {
                string res = "";

                if (!_Stream.DataAvailable)
                {
                    logger.Info("noData!");
                    Thread.Sleep(500);
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] buffer = new byte[2048];
                        int count = 0;

                        while (_Stream.DataAvailable)
                        {
                            count = _Stream.Read(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, count);
                        }
                        res = ASCIIEncoding.ASCII.GetString(ms.ToArray(), 0, ms.ToArray().Length);



                        MyEventArg _e = new MyEventArg(res);
                        DataReceived(this, _e);
                    }
                }
            }
        }

        private NetworkStream _Stream;
        private Thread _Thread;
    }

}
