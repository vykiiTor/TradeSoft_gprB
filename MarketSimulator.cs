using System;

public class MarketSimulator
{
	private decimal currentMarketPrice = -1;

    private List<Order> orders = new List<Order>();
    private List<Order> ordersLog = new List<Order>();

    private StrategyManager strategyManager;
    public MarketSimulator()
    {

    }

    public void SetStrategyManager (StrategyManager strategyManager)
    {
        this.strategyManager = strategyManager;
    }
    
    public Order ReceiveOrder(Order order)
    {
        orders.Add(order);
        decimal strikePrice = -1;
        Order orderLog = new Order();
        if (order.typeOrder == TypeOrder.Market)
        {
            strikePrice = currentMarketPrice;
            orderLog = new Order(DateTime.Now, order.quantity, order.typeOrder, order.striker, strikePrice);
        }
        ordersLog.Add(orderLog);
        return (orderLog);
    }

    public void UpdateMarketPrice(Decimal price)
    {
        currentMarketPrice = price;
    }
}

public class Order
{
    //modif to private later
    internal DateTime time { get; set; }
    internal long quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal price { get; set; }
    internal string striker { get; set; }

    public Order ()
    {

    }
    public Order (DateTime time, long quantity, TypeOrder typeOrder, string striker, decimal price=0)
	{
		this.time = time;
        this.quantity = quantity;
        this.typeOrder = typeOrder;
        this.price = price;
        this.striker = striker;
	}

    public string printOrder()
    {
        return "order from "+striker+" done at "+time+" of "+quantity+" asset at "+price+"";
    }
    
}

public enum TypeOrder
{
	Market
}
