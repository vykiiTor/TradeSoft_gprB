using System;
using System.Diagnostics;

public class Strategy_Manager : TicksReceptor
{
	internal Market_Simulator Market;
	internal RiskAnalyser Risk;

    private List<Order> Orders = new List<Order>();
    private List<Order> OrdersLog = new List<Order>();
    public List<Order> getOrdersLog () {  return OrdersLog; }
    private string StrategyName;

    public event Action<Order> OrderCompleted;

    public Action<Order> getOrderCompleted()
    {
        return OrderCompleted;
    }
    public Strategy_Manager(Market_Simulator market, RiskAnalyser risk, string strategyName)
	{
		Market = market;
		StrategyName = strategyName;
		Risk = risk;

        Thread strategy = new Thread(RunStrategy);
        strategy.Start();
    }

    //https://stackoverflow.com/questions/9931723/passing-a-callback-function-to-another-class
    public void RunStrategy ()
	{
        while (true)
        {
            //if ( getObjectList().Count >2 && (getObjectList()[getObjectList().Count-1].Price - getObjectList()[getObjectList().Count - 2].Price) > (decimal)0.1)
            if (true)
            {
                Order order = new Order(DateTime.Now, 1, TypeOrder.Market, StrategyName);
                Order orderLog = Market.receiveOrder(order);
                OrdersLog.Add(orderLog);
                Risk.GetOrdersLog().Add(orderLog);
                Console.WriteLine(orderLog.printOrder());
            }
        }

	}

}

