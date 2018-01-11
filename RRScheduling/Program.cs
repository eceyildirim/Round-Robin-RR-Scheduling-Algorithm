using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace RRScheduling
{
    class Program
    {
        Stopwatch Hesapla = new Stopwatch();

        static void Main(string[] args)
        {
            Program p = new Program();
            Thread threadRR = new Thread(new ThreadStart(p.RR));
            threadRR.Start();

            Console.ReadKey();
        }




        public List<Process> FileOperations()
        {
            List<Process> processList = new List<Process>();

            string dosya_yolu = @"processInfo.txt";
            FileStream fs = new FileStream(dosya_yolu, FileMode.Open, FileAccess.Read);
            StreamReader sw = new StreamReader(fs);
            string yazi = sw.ReadLine();
            while (yazi != null)
            {
                string[] line = yazi.Split(',');

                Process process = new Process(line[0], int.Parse(line[1]), int.Parse(line[2]), int.Parse(line[3]));
                processList.Add(process);

                yazi = sw.ReadLine();

            }

            sw.Close();
            fs.Close();

            return processList;
        }




        public List<Process> Sırala()
        {
            //processler Arrival Timelarına göre sıralanır.
            List<Process> processList = FileOperations();
            Process temp;
            for (int i = 0; i < processList.Count; i++)
            {
                for (int j = i + 1; j < processList.Count; j++)
                {
                    if (processList[i].arrivalTime > processList[j].arrivalTime || processList[i].arrivalTime == processList[j].arrivalTime && processList[i].burstTime > processList[j].burstTime)
                    {
                        temp = processList[i];
                        processList[i] = processList[j];
                        processList[j] = temp;
                    }
                }
            }
            return processList;
        }


        public void RR()
        {
            Hesapla.Start();


            int[] q = { 3, 4, 8 }; // q=3 , q=4, q=8 durumları için q dizisi oluşturulur.

            for (int t = 0; t < q.Length; t++)
            {
                List<Process> processList = Sırala();
                Console.WriteLine("______________________________________");
                Console.WriteLine("Gant Chart RR");
                Console.WriteLine("______________________________________");


                int counter = 0;

                int maxBT = 0;
                int maxBTprocessIndex = 0;

                for (int i = 0; i < processList.Count; i++) //max. burst time a sahip processIndex tutulur.
                {
                    if (processList[i].burstTime > maxBT)
                    {
                        maxBT = processList[i].burstTime;
                        maxBTprocessIndex = i;
                    }

                }

                while (processList[maxBTprocessIndex].burstTime != 0) //max burst timelı process sonlanmadan program bitmez.
                {
                    for (int j = 0; j < processList.Count; j++)
                    {
                        if (processList[j].burstTime != 0)
                        {

                            Console.Write(processList[j].processName + "\t");
                            processList[j].start = counter;
                            Console.Write(counter + "\t");


                            if (q[t] >= processList[j].burstTime)
                            {
                                counter += processList[j].burstTime;
                                Console.Write(counter + "\t");
                                processList[j].burstTime = 0;
                            }
                            else
                            {
                                counter += q[t];
                                processList[j].burstTime = (processList[j].burstTime - q[t]);
                                Console.Write(counter + "\t");
                            }
                            Console.WriteLine();
                        }
                    }
                }

            }


            Hesapla.Stop();
            Console.Write("RR çalışma süresi:");
            TimeSpan HesaplananZaman = Hesapla.Elapsed;
            string HesaplamaSonucu = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", HesaplananZaman.Hours, HesaplananZaman.Minutes,
                HesaplananZaman.Seconds, HesaplananZaman.Milliseconds);
            Console.WriteLine(HesaplamaSonucu);

        }

















    }
}
