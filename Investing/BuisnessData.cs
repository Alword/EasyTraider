using System;
using System.Collections.Generic;
using System.Text;

namespace Investing
{
    public enum BuisnessSynopsis
    {
        ActivelySell = -2,
        Sell,
        Neutral,
        Buy,
        ActivelyBuy
    }

    public class BuisnessData
    {
        public string Pair { get; set; }
        public double Pid { get; set; }
        public double Percent { get; set; }

        private DateTime accessDate;
        public DateTime AccessDate
        {
            get
            {
                return accessDate;
            }
            set
            {
                accessDate = value;
                DataUpdated.Invoke(this);
            }
        }

        public delegate void DataChanged(object sender);

        public event DataChanged DataUpdated;

        public BuisnessSynopsis Min5 { get; set; }
        public BuisnessSynopsis Min15 { get; set; }

        public BuisnessData()
        {
            //testOnlu //todo
            new BuisnessTester(this, BuisnessAgression.Agresive, BuisnessSpeed.Min5);
            new BuisnessTester(this, BuisnessAgression.Conservative, BuisnessSpeed.Min5);
            new BuisnessTester(this, BuisnessAgression.Agresive, BuisnessSpeed.Min15);
            new BuisnessTester(this, BuisnessAgression.Conservative, BuisnessSpeed.Min15);
        }

        public int TotalSeconds
        {
            get
            {
                return (int)Math.Truncate((DateTime.Now - AccessDate).TotalSeconds);
            }
        }
    }
}
