using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTradeBot.DataTypes
{
    class Deal
    {
        DealInfo Info { get; }

        int Profit { get; }

        string Status { get; }

        public Deal(DealInfo info, int profit, string status)
        {
            Info = info;
            this.Profit = profit;
            this.Status = status;
        }
    }
}
