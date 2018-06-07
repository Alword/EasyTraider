using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

using OTradeBot.Edge;

namespace OTradeBot
{
    class Program
    {
        static void Main(string[] args)
        {
            OTradeEdge testchrome = new OTradeEdge();
            testchrome.Start();
            Environment.Exit(0);
        }
    }
}
