namespace OTradeBot.Edge
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OTradeBot.DataTypes;

    public partial class OTradeEdge
    {
        delegate void DealEndDelegate(object sender, Deal deal);

        event DealEndDelegate OnDealEnd;

        public double Balance
        {
            get
            {
                string regExp = "[^0-9.]";
                string noBalance = balance.Text;
                string tmp = Regex.Replace(noBalance, regExp, "").Replace(".", ",");
                return Convert.ToDouble(tmp);
            }
        }

        public void Initialization(object settings = null)
        {
            //TODO Выбор браузера
            SetBrowser();

            //
            SetWindow();

            //
            LogIn();

            //Параллельное связываение;
            BindControls();
        }

        public void Start()
        {
            Initialization();
            Console.WriteLine(Balance);
            Random x = new Random();
            while (true)
            {
                IWebElement traidTable = browser.FindElement(By.ClassName("user-deals-table__body"));
                IReadOnlyCollection<IWebElement> traidResults = traidTable.FindElements(By.ClassName("user-deals-table__row"));
                IWebElement lastTrade = null;
                string lastId = "";
                if (traidResults.Count > 0)
                {
                    lastTrade = traidResults.ElementAt(0);
                    IWebElement lastTradeIdRow = lastTrade.FindElement(By.ClassName("user-deals-table__cell user-deals-table__cell_id"));
                    IWebElement lastTradeId = lastTradeIdRow.FindElement(By.ClassName("user-deals-table__cell-content user-deals-table-content"));
                    lastId = lastTradeId.Text;
                }



                if (x.Next(0, 2) == 0)
                {
                    buyUpButton.Click();
                }
                else
                {
                    sellDownButton.Click();
                }

                string traidId = lastId;
                while (lastId == traidId)
                {
                    System.Threading.Thread.Sleep(1000);
                    traidResults = traidTable.FindElements(By.ClassName("user-deals-table__row"));
                    lastTrade = traidResults.ElementAt(0);

                    IWebElement lastTradeIdRow = lastTrade.FindElement(By.ClassName("user-deals-table__cell user-deals-table__cell_id"));
                    IWebElement lastTradeId = lastTradeIdRow.FindElement(By.ClassName("user-deals-table__cell-content user-deals-table-content"));
                    traidId = lastTradeId.Text;
                }

                IWebElement lastTradeProfitRow = lastTrade.FindElement(By.ClassName("user-deals-table__cell user-deals-table__cell_profit"));
                IWebElement lastTradeProfitBox = lastTradeProfitRow.FindElement(By.TagName("span"));
                IWebElement lastTradeProfit = lastTradeProfitRow.FindElement(By.TagName("span"));
                string profit = lastTradeProfit.Text;

                Console.WriteLine($"Резульат торговли {profit}");
            }
        }

        public void Exit()
        {
            browser.Quit();
        }

        public void BuyUp(DealInfo dealinfo)
        {
            //setData(dealinfo)
            buyUpButton.Click();
        }

        public void SellDown(DealInfo dealinfo)
        {
            //SellDownButton.Click();
            sellDownButton.Click();
        }

        #region StartUp premitives
        private readonly string isLearningAcc = "Учебный счёт";

        /// <summary>
        /// Элемент браузера
        /// </summary>
        private IWebDriver browser = null;

        /// <summary>
        /// Установка браузера, выполнять первоочередно
        /// </summary>
        protected virtual void SetBrowser()
        {
            foreach (var process in Process.GetProcessesByName("MicrosoftEdge"))
            {
                process.Kill();
            }
            foreach (var process in Process.GetProcessesByName("MicrosoftEdgeCP"))
            {
                process.Kill();
            }
            browser = new EdgeDriver();
        }

        private void SetWindow()
        {
            while (browser.CurrentWindowHandle == null) { System.Threading.Thread.SpinWait(100); }
            browser.Manage().Window.Maximize();
        }

        private void LogIn()
        {
            browser.Navigate().GoToUrl("https://olymptrade.com/platform");
            //btn-t4 -sm -h40 -submit
            IWebElement trainingAccaunt = null;
            IWebElement checkLoginBox = null;
            while (trainingAccaunt == null)
            {
                try
                {
                    checkLoginBox = browser.FindElement(By.ClassName("btn-t4 -sm -h40 -submit"));
                    if (checkLoginBox != null)
                    {
                        Console.WriteLine("Log-In and press any key");
                        Console.ReadLine();
                    }
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Попытка авторизации");
                    System.Threading.Thread.Sleep(1000);
                }

                try
                {
                    trainingAccaunt = browser.FindElement(By.ClassName("title header-row__balance-title"));
                }
                catch (NoSuchElementException)
                {
                    Console.WriteLine("Попытка входа в учебный режим");
                    System.Threading.Thread.Sleep(1000);
                }

                if (isLearningAcc != trainingAccaunt.Text)
                {
                    this.Exit();
                }
                Console.WriteLine(trainingAccaunt.Text);
            }
        }

        #endregion
    }
}
