using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rvip
{
    public class Process
    {
        private int id;
        private int count_e;
        private int L = 0;
        private MyEvent[] es;
        private string discription = "Процесс ";
        public Process(int _id, int count_e)
        {
            this.id = _id;
            this.count_e = count_e;
            es = new MyEvent[count_e];
            for (int i = 0; i < count_e; i++)
            {
                es[i] = new MyEvent((i + 1), false);
            }
            /*
            Random r = new Random();
            int met = r.Next(0, 2);
            int poz = r.Next(0, count_e);
            if (met == 1)
                es[poz].SetIsSend(true);
            */
        }

        public string GetDiscription()
        {
            return discription + id;
        }
        public int GetId()
        {
            return id;
        }
        public MyEvent[] GetEvents()
        {
            return es;
        }

        public int GetLProcess()
        {
            return L;
        }
        public void SetLProcess(int l)
        {
            L = l;
        }
    }
}
