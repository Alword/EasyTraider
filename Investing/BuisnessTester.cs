using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Investing
{
    public enum BuisnessAgression
    {
        Root,
        Agresive = 1,
        Conservative = 2
    }

    public enum BuisnessSpeed
    {
        Min5 = 5,
        Min15 = 15
    }

    public class BuisnessTester
    {
        int youLose = 0;
        int youWon = 0;
        int countContracnt = 0;


        StreamWriter loger = null;

        public DateTime CreateDate { private set; get; }
        public DateTime ExmineDate { private set; get; }

        public double CreationPid { private set; get; }

        public BuisnessSynopsis Contract { private set; get; }
        public BuisnessSpeed Speed { private set; get; }
        public BuisnessAgression Aggression { set; get; }

        bool isAlive = false;

        public BuisnessTester(BuisnessData data, BuisnessAgression aggression = BuisnessAgression.Root, BuisnessSpeed speed = BuisnessSpeed.Min5)
        {
            Aggression = aggression;
            Speed = speed;

            data.DataUpdated += Data_DataUpdated;

        }

        private void Data_DataUpdated(object sender)
        {
            var data = sender as BuisnessData;

            if (!isAlive)
            {
                if (Speed == BuisnessSpeed.Min5 && (Math.Abs((int)data.Min5) >= (int)Aggression)
                || Speed == BuisnessSpeed.Min15 && (Math.Abs((int)data.Min15) >= (int)Aggression) && data.TotalSeconds < 20)
                {
                    CreateDate = DateTime.Now;
                    ExmineDate = CreateDate.AddMinutes((int)Speed);
                    CreationPid = data.Pid;
                    switch (Speed)
                    {
                        case BuisnessSpeed.Min5: Contract = data.Min5; break;
                        case BuisnessSpeed.Min15: Contract = data.Min15; break;
                    }
                    isAlive = true;

                    if (loger == null)
                    {
                        Directory.CreateDirectory("tests");
                        loger = File.AppendText("tests/" + $"#{DateTime.Now.Ticks}{data.Pair}.txt");
                        loger.AutoFlush = true;
                    }
                    loger.WriteLine($"{DateTime.Now.TimeOfDay} Начало теста {Speed}; {Aggression}; data.TotalSeconds:{data.TotalSeconds}");
                    loger.WriteLine($"  создан контракт {data.Pair} на сумму amount:0$");
                    loger.WriteLine($"  CreateDate:{CreateDate}; ExmineDate:{ExmineDate};");
                    loger.WriteLine($"  CreationPid:{CreationPid}; Speed:{Speed}");
                    loger.WriteLine($"  data.Min5:{data.Min5} data.Min15:{data.Min15}");
                    loger.WriteLine($"  Contract:{Contract}");
                }
                else
                {
                    //todo условия торговли не подходят
                }
            }
            else
            {
                if (ExmineDate < DateTime.Now)
                {
                    isAlive = false;

                    if ((int)Contract > 0 == data.Pid > CreationPid)
                    {
                        youWon++;
                        //testOnly
                        loger.WriteLine($"{DateTime.Now} good Now {data.Pid} before {CreationPid}; {Contract};");
                    }
                    else if (data.Pid != CreationPid)
                    {
                        youLose++;
                        //testOnly
                        loger.WriteLine($"{DateTime.Now} you lose Now {data.Pid} before {CreationPid}; {Contract};");
                    }
                    else
                    {
                        //testOnly
                        loger.WriteLine($"{DateTime.Now} i don't care {data.Pid}=={CreationPid}; {Contract};");
                    }
                    countContracnt++;
                    loger.WriteLine($"===countContracnt:{countContracnt}===youLose:{youLose}===youWon:{youWon}===");
                }
            }
        }
    }
}
