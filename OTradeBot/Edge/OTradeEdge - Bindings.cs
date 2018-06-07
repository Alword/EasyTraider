namespace OTradeBot.Edge
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Edge;
    using OTradeBot.DataTypes;

    public partial class OTradeEdge
    {
        private void BindControls()
        {
            List<Action> bindingTasks = new List<Action>
            {
                new Action(BindBuyButton),
                new Action(BindSellButton),
                new Action(BindCurrencyInput),
                new Action(BindtimeInput),
                new Action(BindSentimentDown),
                new Action(BindBalance)
            };

            Parallel.ForEach(bindingTasks, item => item.Invoke());
        }
        #region Binding
        /// <summary>
        /// Элемент кнопки покупки
        /// </summary>
        private IWebElement buyUpButton = null;

        /// <summary>
        /// Элемент кнопки продажи
        /// </summary>
        private IWebElement sellDownButton = null;

        private IWebElement currencyInput = null;

        private IWebElement timeInput = null;

        private IWebElement sentimentDown = null;

        private IWebElement balance = null;

        /// <summary>
        /// Связываение кнопок, выполнять параллельно
        /// </summary>
        private void BindBuyButton()
        {
            buyUpButton = browser.FindElement(By.ClassName("container -up"));
            buyUpButton = buyUpButton.FindElement(By.TagName("button"));
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindSellButton()
        {
            sellDownButton = browser.FindElement(By.ClassName("container -down"));
            sellDownButton = sellDownButton.FindElement(By.TagName("button"));
        }

        private void BindCurrencyInput()
        {
            currencyInput = browser.FindElement(By.ClassName("input-currency__input"));
        }

        private void BindtimeInput()
        {
            currencyInput = browser.FindElement(By.ClassName("timeinput__input timeinput__input_minutes"));
        }

        private void BindSentimentDown()
        {
            sentimentDown = browser.FindElement(By.ClassName("sentiment--text sentiment--text__up"));
        }

        private void BindBalance()
        {
            balance = browser.FindElement(By.ClassName("sum header-row__balance-sum"));
        }
        #endregion
    }
}
