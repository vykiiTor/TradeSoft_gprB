using System;
using System.Diagnostics;

public class Strategy_Manager : TicksReceptor
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

        if (getObjectList().Count > 2 && (getObjectList()[getObjectList().Count - 1].Price > 
            getObjectList()[getObjectList().Count - 2].Price))
        {
            // check if enough cash to buy
            if(Portfolio.Cash >= getObjectList().Last().Price)
            // return asset quantity to buy
                return 1;
        }
        else if (getObjectList().Count > 2 && (getObjectList()[getObjectList().Count - 1].Price <
            getObjectList()[getObjectList().Count - 2].Price))
        {
            // check if enough asset to sell in the portfolio
            if (Portfolio.Quantity >= 1)
                // return asset quantity to sell
                return -1;
        }
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

// We assess that, in our case, a portfolio can handle only one asset at once
public class Portfolio
{
    internal decimal Cash;
    internal decimal InitialCash;
    internal long Quantity;

    public Portfolio (decimal cash)
    {
        this.Cash = cash;
        InitialCash = cash;
        this.Quantity = 0;

    }

    public decimal getCash()
    {
        return this.Cash;
    }
    public long getQuantity()
    {
        return this.Quantity;
    }
    public void ProcessOrder (Order order)
    {
        lock (this)
        {
            Quantity += order.Quantity;
            if (Quantity > 0)
            {
                Cash -= order.Quantity * order.Price;
            }
            else if (Quantity < 0)
            {
                Cash += order.Quantity * order.Price;
            }
            Console.WriteLine("Cash du portefeuille : " + Cash + " ; quantite du portefeuille : " + Quantity);
        }
    }

}

