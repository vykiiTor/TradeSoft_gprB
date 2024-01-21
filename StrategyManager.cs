using System;
using System.Diagnostics;

public class StrategyManager
{
	private MarketSimulator market;
	private RiskAnalyser risk;
    private Portfolio portfolio;

    private List<Order> ordersLogList = new List<Order>();
    public List<Order> getOrdersLog () {  return ordersLogList; }
    private string strategyName;



    public StrategyManager(MarketSimulator market, RiskAnalyser risk, string strategyName)
	{
		this.market = market;
		this.strategyName = strategyName;
		this.risk = risk;
        portfolio = new Portfolio(1000);
    }
    public int ApplyStrategy ()
    {
	    if(portfolio.GetCash() >= 0)
		    // return asset quantity to buy
		    return 1;
        return 0;
    }

    public void RunStrategy ()
	{
            int quantity = ApplyStrategy();
            if (quantity!=0)
            {
                Order order = new Order(DateTime.Now, quantity, TypeOrder.Market, strategyName);
                Order orderLog = market.ReceiveOrder(order);
                ordersLogList.Add(orderLog); portfolio.ProcessOrder(orderLog);
                risk.GetOrdersLog().Add(orderLog); risk.GetPortfolio().ProcessOrder(orderLog);
                //Console.WriteLine(orderLog.printOrder());
            }
	}
}