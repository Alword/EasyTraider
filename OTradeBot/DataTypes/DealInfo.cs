namespace OTradeBot.DataTypes
{
    public class DealInfo
    {
        string Pair { get; set; }

        int PairProfit { get; set; }

        int Id { get; set; }

        int Trade { get; set; }

        bool IsUp { get; set; }
    }
}