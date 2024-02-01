using System;
using Serilog;

public interface IMarketSimulator 
{
    public void SetStrategyManager(StrategyManager strategyManager);
    public void SetRiskManager(RiskAnalyser riskAnalyser);
    public void ProcessOrder(int strategyId, int quantity);
    public void UpdateMarketPrice(Decimal price);

}

public class MarketSimulator : IMarketSimulator
{   
	private static decimal currentMarketPrice = -1;

    private List<Order> orders = new List<Order>();

    private StrategyManager strategyManager;
    internal RiskAnalyser riskAnalyser;

    public MarketSimulator()
    {

    }

    public void SetStrategyManager (StrategyManager strategyManager)
    {
        this.strategyManager = strategyManager;
    }
    public void SetRiskManager(RiskAnalyser riskAnalyser)
    {
        this.riskAnalyser = riskAnalyser;
    }
    
    //l event et les buy/sell
    
    public void ProcessOrder(int strategyId, int quantity)
    {
        orders.Add( new Order(strategyId, quantity) );
    }

    // this function only works for Market Order (OrderType.Market)
    public void UpdateMarketPrice(Decimal price)
    {
        currentMarketPrice = price;
        
        foreach (Order order in orders) {

            if (order.Quantity < 0) 
            {
                OrderExecReport report = Sell(order.StrategyId, order.Quantity);
                strategyManager.GetStrategy(order.StrategyId).processOrderExecReport(report);
            }
            else if (order.Quantity > 0) {
                OrderExecReport report = Buy(order.StrategyId, order.Quantity);
                strategyManager.GetStrategy(order.StrategyId).processOrderExecReport(report);
            }

        }
        orders.Clear();
    }

    public static decimal GetMarketPrice()
    {
        return currentMarketPrice;
    }

    private static OrderExecReport Buy (int strategyId, int quantity)
    {
        return new OrderExecReport (strategyId, DateTime.Now, quantity, TypeOrder.Market, GetMarketPrice() );
    }

    private static OrderExecReport Sell (int strategyId, int quantity)
    {
        return new OrderExecReport(strategyId, DateTime.Now, quantity, TypeOrder.Market, GetMarketPrice() );
    }
}