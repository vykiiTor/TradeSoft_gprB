using System;

public class Market_Simulator : TicksReceptor
{
	private decimal currentMarketPrice = -1;

    private List<Order> orders = new List<Order>();
    private List<Order> ordersLog = new List<Order>();

    internal Strategy_Manager StrategyManager;
    public Market_Simulator()
    {

    }

    public void setStrategyManager (Strategy_Manager strategyManager)
    {
        StrategyManager = strategyManager;
    }

    public override void DataReception(Object sender, ObjectEventArgs<Ticks_Data> e)
    {
        this.getSyncObject().WaitOne();
        //Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
        getObjectList().Add(e.Data);
        currentMarketPrice = ((Ticks_Data)e.Data).Price;
        //Console.WriteLine(" ticks price : " + getObjectList().Last().Price);
        //Console.WriteLine("Current price : " + CurrentMarketPrice);
    }

    public Order receiveOrder(Order order)
    {
        orders.Add(order);
        decimal StrikePrice = -1;
        Order orderlog = new Order();
        if (order.typeOrder == TypeOrder.Market)
        {
            StrikePrice = this.currentMarketPrice;
            orderlog = new Order(DateTime.Now, order.Quantity, order.typeOrder, order.Striker, StrikePrice);
        }
        ordersLog.Add(orderlog);
        return (orderlog);
    }

    public void UpdateMarketPrice(Decimal price)
    {
        currentMarketPrice = price;
    }
}

public class Order
{
    internal DateTime Time { get; set; }
    internal long Quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal Price { get; set; }
    internal string Striker { get; set; }

    public Order ()
    {

    }
    public Order (DateTime time, long quantity, TypeOrder type_order, string striker, decimal price=0)
	{
		Time = time;
		Quantity = quantity;
		typeOrder = type_order;
		Price = price;
        Striker = striker;
	}

    public string printOrder()
    {
        return "order from "+Striker+" done at "+Time+" of "+Quantity+" asset at "+Price+"";
    }
    
}

public enum TypeOrder
{
	Market
}
