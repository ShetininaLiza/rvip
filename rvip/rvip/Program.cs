using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rvip
{
    class Program
    {
        static Random r=new Random();
        //прирост
        static int d = 1;
        //количество процессов
        static int count_p = 3;
        //количество событий для каждого процесса
        static int[] count_e = new int[] { 5, 6, 4 };
        static Process[] processes = new Process[count_p];
        static void Main(string[] args)
        {
            
            Console.WriteLine("Метод скалярного времени Лэмпорта.");
            
            for (int i = 0; i < count_p; i++)
            {
                processes[i] = new Process((i + 1), count_e[i]);
            }
            processes[0].GetEvents()[1].SetIsSend(true);
            processes[0].GetEvents()[4].SetIsSend(true);
            processes[2].GetEvents()[3].SetIsSend(true);

            for (int i = 0; i < count_p; i++)
            {
                string str = processes[i].GetDiscription() + ": ";
                var es = processes[i].GetEvents();
                for (int j = 0; j < es.Length; j++)
                {
                    str = str + es[j].GetDiscription() + "(" + es[j].GetIsSend() + ")" + " ";
                }
                Console.WriteLine(str);
            }
            List < (int, int, int, int)> links = GenerLink();
            Algorithm(links);
            Console.WriteLine();
            for (int i = 0; i < count_p; i++)
            {
                Console.WriteLine("Логическое время процесса "+ processes[i].GetId()+ " равно: "+processes[i].GetLProcess());
            }
            Console.ReadKey();
        }
        private static void PolMes(int l_mes, int index_e,
                                              Process process)
        {
            Console.WriteLine();
            int l = process.GetLProcess();
            //Console.WriteLine(l);
            
            int max = Math.Max(l, l_mes);
            //Console.WriteLine("Max: "+max);
            max = max + d;
            //Console.WriteLine("ZN: " +max);
            //Console.WriteLine(index_e);
            process.SetLProcess(max);
            
        }

        private static void SendMes(int l, Process process_otp, Process process_pol, int index_e,
            List<(int, int, int, int)> links){
            //Console.WriteLine("L Process "+process_otp.GetId()+": "+ l) ;
            l = l + d;
            //Console.WriteLine("L= " + l);
            process_otp.SetLProcess(l);
            PolMes(l, index_e, process_pol);
        }
        private static void InitL(int start, int end, Process process)
        {
            int l = process.GetLProcess();
            if (l == 0)
            {
                for (int j = 0; j < end; j++)
                {
                    l = l + d;
                }
                process.SetLProcess(l);
            }
            else
            { 
                for(int j=start+1; j<end; j++)
                {
                    l = l + d;
                }
                process.SetLProcess(l);
            }
        }
       private static void Algorithm(List<(int, int, int, int)> links)
        {
            if (links.Count > 0)
            {
                int startP = 0;
                int startO = 0;
                List<(int, int)> indexs = new List<(int, int)>();
                for (int i = 0; i < processes.Length; i++)
                {
                    if ((!links.Select(rec => rec.Item1).ToList().Contains(i))
                         && (!links.Select(rec => rec.Item3).ToList().Contains(i)))
                    {
                        InitL(0, processes.ElementAt(i).GetEvents().Length, processes.ElementAt(i));
                    }
                }
                for (int i = 0; i < links.Count; i++)
                {
                    //Console.WriteLine(links.ElementAt(i));

                    int index_process_otprav = links.ElementAt(i).Item3;
                    int e_process_otprav = links.ElementAt(i).Item4;

                    int index_process_pol = links.ElementAt(i).Item1;
                    int e_process_pol = links.ElementAt(i).Item2;
                    
                    InitL(startO, e_process_otprav, processes.ElementAt(index_process_otprav));
                    InitL(startP, e_process_pol, processes.ElementAt(index_process_pol));
                    
                    startO = e_process_otprav;
                    startP = e_process_pol;

                    SendMes(processes.ElementAt(index_process_otprav).GetLProcess(),
                        processes.ElementAt(index_process_otprav),
                        processes.ElementAt(index_process_pol), e_process_pol, links);
                    
                    indexs.Add((index_process_pol, e_process_pol));
                    indexs.Add((index_process_otprav, e_process_otprav));
                    
                    links.RemoveAt(i);
                    i--;
                }
                var list = indexs.GroupBy(rec => rec.Item1);
                for (int i = 0; i < list.Count(); i++)
                {
                    int key = list.ElementAt(i).Key;
                    int max = indexs.Where(rec => rec.Item1 == key).Select(rec => rec.Item2).Max();
                    if (max < processes.ElementAt(key).GetEvents().Length)
                    {
                        InitL(max, processes.ElementAt(key).GetEvents().Length,
                            processes.ElementAt(key));
                    }
                }
            }
            else if (links.Count == 0)
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    var proces = processes[i];
                    var es = proces.GetEvents();
                    int l = proces.GetLProcess();
                    for (int j = 0; j < es.Length; j++)
                    {
                        l = l + d;
                    }
                    proces.SetLProcess(l);
                }
            }
        }
        
        private static List<(int, int, int, int)> GenerLink()
        {
            //первое значение - номер процесса, которому отправляется
            //второе значение - номер события, которому отправляется сообщение
            //третье значение - номер процесса, который отправляет
            //четвертое значение - номер события, которое отправляет
            List<(int, int, int, int)> link = new List<(int, int, int, int)>();
            Console.WriteLine();
            link.Add((2, 1, 0, 1));
            link.Add((0, 2, 2, 3));
            for (int i = 0; i < link.Count; i++)
            {
                Console.WriteLine("Событие в процессе № " + processes[link.ElementAt(i).Item3].GetId()
                               + " посылает сообщение в событие № " + (link.ElementAt(i).Item2 + 1) + " в процессе № " + processes[link.ElementAt(i).Item1].GetId());

            }
            return link;
        }
    }
}
