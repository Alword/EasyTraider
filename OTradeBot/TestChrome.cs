namespace OTradeBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;

    public class TestChrome
    {
        private readonly string isLearningAcc = "Учебный счёт";

        IWebDriver Browser;

        public void Start()
        {
            //Browser = new ChromeDriver();
            Browser = new EdgeDriver();

            Browser.Manage().Window.Maximize();
            Browser.Navigate().GoToUrl("https://olymptrade.com/platform");

            Console.WriteLine("Log-In and press any key");
            Console.ReadLine();

            IWebElement trainingAccaunt = Browser.FindElement(By.ClassName("title header-row__balance-title"));
            Console.WriteLine(trainingAccaunt.Text);
            if (trainingAccaunt.Text == isLearningAcc)
            {

            }
        }

        public void Exit()
        {
            Browser.Quit();
        }

    }
}
