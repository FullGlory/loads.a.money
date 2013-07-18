namespace SpreadBet.Common.Components
{
    using System;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using System.Linq;
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using SpreadBet.Domain.Interfaces;

    public class PriceUpdate : IUpdate
    {
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IAutomationProvider _automationProvider;
        private readonly IRepository _repository;

        public PriceUpdate(IStockDataProvider stockDataProvider, 
                            IAutomationProvider automationProvider,
                            IRepository repository)
        {
            Condition.Requires(stockDataProvider).IsNotNull();
            Condition.Requires(automationProvider).IsNotNull();
            Condition.Requires(repository).IsNotNull();

            this._stockDataProvider = stockDataProvider;
            this._automationProvider = automationProvider;
            this._repository = repository;
        }

        public void Update(IEnumerable<Stock> stocks)
        {
            foreach (var stock in stocks)
            {
                var latestPrice = this._automationProvider.Latest(stock);

                if (latestPrice != null)
                {
                    var period = _repository.GetAll<Period>().Last();
                    var sp = _repository.GetAll<StockPrice>(x => x.Period.Id == period.Id && x.Stock.Identifier == stock.Identifier).FirstOrDefault();
                    sp.Price.Bid = latestPrice.Bid;
                    sp.Price.Offer = latestPrice.Offer;
                    this._stockDataProvider.UpdateStockPrice(sp);
                }
            }
        }

        public void Update(IEnumerable<Bet> bets)
        {
            foreach (var bet in bets)
            {
                var latestPrice = this._automationProvider.Latest(bet.Stock);

                if (latestPrice != null)
                {
                    var period = _repository.GetAll<Period>().Last();
                    var sp = _repository.GetAll<StockPrice>(x => x.Period.Id == period.Id && x.Stock.Identifier == bet.Stock.Identifier).FirstOrDefault();
                    sp.Price.Bid = latestPrice.Bid;
                    sp.Price.Offer = latestPrice.Offer;
                    this._stockDataProvider.UpdateStockPrice(sp);
                }
            }
        }
    }
}
