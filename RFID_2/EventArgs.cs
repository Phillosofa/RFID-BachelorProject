using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFID_2
{
    class MyEventArg : EventArgs
    {
        private string data;

        public MyEventArg(string data)
        {
            this.data = data;
        }

        public string GetData()
        {
            return this.data;
        }
    }
}
