using System;
using Serilog;

public class MarketSimulator
{
	private decimal currentMarketPrice = -1;

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
    //possiblement faire buyMarket pour juste specifier la quantite 
    
    //sur l'interface du market sim ya levent et les buy/sell
    
    //receive order and put into a data structure 
    //private
    public void ProcessOrder(Order order)
    {
        orders.Add(order);
        /*decimal OrderPrice = -1;
        OrderExecReport orderlog = new OrderExecReport();
        if (order.typeOrder == TypeOrder.Market)
        {
            OrderPrice = this.currentMarketPrice;
            orderlog = new OrderExecReport("1", DateTime.Now, order.quantity, order.typeOrder, OrderPrice);
        }
        // send orderLog to risk
        return (orderlog);*/
    }

    //list of order 
    //check if order are for this price act accordingly
    //send back info
    //faire attention au type parce que actuellement on ne prend que le type market
    public void UpdateMarketPrice(Decimal price)
    {
        currentMarketPrice = price;

        // Process orders
        OrderExecReport report = new OrderExecReport();
        foreach (Order order in orders) {
            if (order.quantity < 0) 
            {
                order.price = price;
                report = Sell(order);          
            }
            else if (order.quantity>0) {
                order.price = price;
                report = Buy(order);
            }
            if (report != null)
            {
                //event au moment l'order process auquel les strats s'abonnent et lui fait l'exec report 
                strategyManager.GetStrategy(order.strategyId).processOrderExecReport(report);
            }
        }
        orders.Clear();
    }

    public decimal GetMarketPrice()
    {
        return currentMarketPrice;
    }

    // new method for buying and selling that uses the data from receive order
    private static OrderExecReport Buy (Order order)
    {
        return new OrderExecReport (order.strategyId, DateTime.Now, order.quantity, TypeOrder.Market, order.price);
    }

    private static OrderExecReport Sell (Order order)
    {
        return new OrderExecReport(order.strategyId, DateTime.Now, order.quantity, TypeOrder.Market, order.price);
    }
}