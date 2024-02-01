using System;
using Serilog;

public interface IMarketSimulator 
{
    public void SetStrategyManager(StrategyManager strategyManager);
    public void SetRiskManager(RiskAnalyser riskAnalyser);
    public void ProcessOrder(int strategyId, int quantity);
    public void UpdateMarketPrice(Decimal price);
    public event EventHandler<OrderExecEventArgs> ReceiveOrder;

}
public class OrderExecEventArgs : EventArgs
{
    public OrderExecReport Report { get; }

    public OrderExecEventArgs(OrderExecReport report)
    {
        Report = report;
    }
}


public class MarketSimulator : IMarketSimulator
{
    public event EventHandler<OrderExecEventArgs> ReceiveOrder;

    private static decimal currentMarketPrice = -1;

    private List<Order> orders = new List<Order>();

    private StrategyManager strategyManager;
    internal RiskAnalyser riskAnalyser;

    public MarketSimulator()
    {

    }
    private void RaiseReceiveOrderEvent(OrderExecReport report)
    {
        ReceiveOrder?.Invoke(this, new OrderExecEventArgs(report));
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
                Sell(order.StrategyId, order.Quantity);
            }
            else if (order.Quantity > 0) {
                Buy(order.StrategyId, order.Quantity);
            }

        }
        orders.Clear();
    }

    public static decimal GetMarketPrice()
    {
        return currentMarketPrice;
    }

    private void Buy(int strategyId, int quantity)
    {
        OrderExecReport report = new OrderExecReport(strategyId, DateTime.Now, quantity, TypeOrder.Market, GetMarketPrice());
        RaiseReceiveOrderEvent(report);
    }

    private void Sell(int strategyId, int quantity)
    {
        OrderExecReport report = new OrderExecReport(strategyId, DateTime.Now, quantity, TypeOrder.Market, GetMarketPrice());
        RaiseReceiveOrderEvent(report);
    }
}