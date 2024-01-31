using System;
using Serilog;

public class MarketSimulator
{
	private decimal currentMarketPrice = -1;


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
    
    //receive order and put into a data structure 
    //private
    public OrderExecReport ProcessOrder(Order order)
    {
        decimal OrderPrice = -1;
        OrderExecReport orderlog = new OrderExecReport();
        if (order.typeOrder == TypeOrder.Market)
        {
            OrderPrice = this.currentMarketPrice;
            orderlog = new OrderExecReport("1", DateTime.Now, order.quantity, order.typeOrder, OrderPrice);
        }
        // send orderLog to risk
        return (orderlog);
    }

    //list of order 
    //check if order are for this price act accordingly
    //send back info
    public void UpdateMarketPrice(Decimal price)
    {
        currentMarketPrice = price;
    }
    
    // new method for buying and selling that uses the data from receive order
}