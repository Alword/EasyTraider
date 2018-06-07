using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace OTradeBot
{
    class Program
    {
        static void Main(string[] args)
        {
            TestChrome testchrome = new TestChrome();
            testchrome.Start();
            Environment.Exit(0);
        }
    }
}
