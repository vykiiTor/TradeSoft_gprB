using System;

public class Market_Simulator : TicksReceptor
{
	private decimal CurrentMarketPrice = -1;

    private List<Order> orders = new List<Order>();
    private List<Order> ordersLog = new List<Order>();

    internal Strategy_Manager StrategyManager;
    internal RiskAnalyser RiskAnalyser;
    public Market_Simulator()
    {

    }

    public void setStrategyManager (Strategy_Manager strategyManager)
    {
        StrategyManager = strategyManager;
    }
    public void setRiskAnalyser(RiskAnalyser riskAnalyser)
    {
        RiskAnalyser = riskAnalyser;
    }

    public override void DataReception(Object sender, ObjectEventArgs<Ticks_Data> e)
    {
        Thread market = new Thread(() =>
        {
            this.getSyncObject().WaitOne();
            //Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
            getObjectList().Add(e.Data);
            CurrentMarketPrice = ((Ticks_Data)e.Data).Price;
            Console.WriteLine(" ticks price : " + getObjectList().Last().Price);
			Console.WriteLine("Current price : " + CurrentMarketPrice);
            this.getSyncObject().Release();
        });
        market.Start();
    }

    public Order receiveOrder(Order order)
    {
        decimal StrikePrice = -1;
        Order orderlog = new Order();
        if (order.typeOrder == TypeOrder.Market)
        {
            // lock the market price ?
            StrikePrice = this.CurrentMarketPrice;
            Console.WriteLine(order.Quantity + " of asset was bought at " + StrikePrice);
            orderlog = new Order(DateTime.Now, order.Quantity, order.typeOrder, order.Striker, StrikePrice);
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
    
}

public enum TypeOrder
{
	Market
}
