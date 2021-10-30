using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rvip
{
    public class MyEvent
    {
        private string discription = "Событие ";
        private int id;
        private bool isSend;
        public MyEvent(int id, bool send)
        {
            this.id = id;
            isSend = send;
        }

        public string GetDiscription()
        {
            return discription + id;
        }

        public bool GetIsSend()
        {
            return isSend;
        }
        public void SetIsSend(bool send)
        {
            isSend = send;
        }

        public int GetId()
        {
            return id;
        }
    }
}
