namespace SpreadBet.Domain.Commands
{
    using SpreadBet.CommandBus;

    public class PlaceBetCommand : ICommand
    {
        public string StockIdentifier { get; set; }

        public decimal InitialLoss { get; set; }

        public decimal BidAmount { get; set; }

        public decimal OpeningPosition { get; set; }

        public decimal ExitPrice { get; set; }

        public bool IsIncrease { get; set; }
    }
}
