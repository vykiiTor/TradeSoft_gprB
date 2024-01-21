using System;
using System.Diagnostics;

public class Strategy_Manager
{
	internal Market_Simulator Market;
	internal RiskAnalyser Risk;
    internal Portfolio Portfolio;

    private List<Order> OrdersLog = new List<Order>();
    public List<Order> getOrdersLog () {  return OrdersLog; }
    private string StrategyName;



    public Strategy_Manager(Market_Simulator market, RiskAnalyser risk, string strategyName)
	{
		Market = market;
		StrategyName = strategyName;
		Risk = risk;
        Portfolio = new Portfolio(1000);
    }
    public int ApplyStrategy ()
    {
	    if(Portfolio.Cash >= 0)
		    // return asset quantity to buy
		    return 1;
        return 0;
    }

    public void RunStrategy ()
	{
            int quantity = ApplyStrategy();
            if (quantity!=0)
            {
                Order order = new Order(DateTime.Now, quantity, TypeOrder.Market, StrategyName);
                Order orderLog = Market.receiveOrder(order);
                OrdersLog.Add(orderLog); Portfolio.ProcessOrder(orderLog);
                Risk.GetOrdersLog().Add(orderLog); Risk.GetPortfolio().ProcessOrder(orderLog);
                //Console.WriteLine(orderLog.printOrder());
            }
	}
}