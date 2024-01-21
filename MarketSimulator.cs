using System;

public class MarketSimulator
{
	private decimal currentMarketPrice = -1;

    private List<Order> orders = new List<Order>();

    internal StrategyManager strategyManager;
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

    public OrderExecReport ReceiveOrder(Order order)
    {
        orders.Add(order);
        decimal OrderPrice = -1;
        OrderExecReport orderlog = new OrderExecReport();
        if (order.typeOrder == TypeOrder.Market)
        {
            OrderPrice = this.currentMarketPrice;
            orderlog = new OrderExecReport("1",DateTime.Now, order.quantity, order.typeOrder, OrderPrice);
        }
        return (orderlog);
    }

    public void UpdateMarketPrice(Decimal price)
    {
        this.currentMarketPrice = price;
    }
}
