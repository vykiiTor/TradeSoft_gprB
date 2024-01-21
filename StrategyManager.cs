using System;
using System.Diagnostics;

public class StrategyManager
{
	private MarketSimulator market;
    private Portfolio portfolio;

    private List<OrderExecReport> ordersLog = new List<OrderExecReport>();
    public List<OrderExecReport> GetOrdersLog () {  return ordersLog; }
    private string strategyName;



    public StrategyManager(MarketSimulator market, RiskAnalyser risk, string strategyName)
	{
		this.market = market;
		this.strategyName = strategyName;
        this.portfolio = new Portfolio(1000);
    }
    public int ApplyStrategy ()
    {
	    if(portfolio.GetCash() >= 0)
		    // return asset quantity to buy
		    return -1;
        return 0;
    }

    public void RunStrategy()
    {
        int quantity = ApplyStrategy();
        if (quantity != 0)
        {
            Order order = new Order("1", DateTime.Now, quantity, TypeOrder.Market);
            OrderExecReport orderLog = market.ReceiveOrder(order);
            ordersLog.Add(orderLog); portfolio.ProcessOrder(orderLog);
            //Console.WriteLine(orderLog.PrintOrder());
        }
    }
}