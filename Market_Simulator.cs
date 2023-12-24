using System;

public class Market_Simulator : TicksReceptor
{
	private decimal CurrentMarketPrice = -1;
    public Market_Simulator()
	{
		
	}

    public override void ReceivePrices(Object sender, TickEventArgs e)
    {
        Thread market = new Thread(() =>
        {
            this.getSyncTick().WaitOne();
            //Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
			CurrentMarketPrice = e.Tick.Price;
			Console.WriteLine("Current price : " + CurrentMarketPrice);
            this.getSyncTick().Release();
        });
        market.Start();
    }

    public OrderLog receiveOrder(Order order)
    {
        decimal StrikePrice = -1;
        OrderLog orderlog = new OrderLog();
        if (order.typeOrder == TypeOrder.Market)
        {
            // lock the market price ?
            StrikePrice = this.CurrentMarketPrice;
            Console.WriteLine(order.Quantity + " of asset was bought at " + StrikePrice);
            orderlog = new OrderLog(DateTime.Now, order.Quantity, order.typeOrder, StrikePrice);
        }
        return orderlog;
    }
}

public class Order
{
    internal DateTime Time { get; set; }
    internal long Quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal Price { get; set; }

    public Order (DateTime time, long quantity, TypeOrder type_order, decimal price=0)
	{
		Time = time;
		Quantity = quantity;
		typeOrder = type_order;
		Price = price;
	}
    
}

public class OrderLog
{
    internal DateTime Time { get; set; }
    internal long Quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal Price { get; set; }

    public OrderLog ()
    {

    }

    public OrderLog(DateTime time, long quantity, TypeOrder type_order, decimal price)
    {
        Time = time;
        Quantity = quantity;
        typeOrder = type_order;
        Price = price;
    }
}

public enum TypeOrder
{
	Market
}
