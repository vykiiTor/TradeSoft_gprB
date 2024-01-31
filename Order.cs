using System;

public class Order
{
    internal int strategyId { get; set; }
    internal DateTime time { get; set; }
    internal int quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal price { get; set; }

    public Order()
    {

    }
    public Order(int strategyId, DateTime time, int quantity, TypeOrder type_order, decimal price = 0)
    {
        this.strategyId = strategyId;
        this.time = time;
        this.quantity = quantity;
        this.typeOrder = type_order;
        this.price = price;
    }

    public string PrintOrder()
    {
        return "order " + typeOrder + " from " + strategyId + " request done at " + time + " of " + quantity + " asset at " + price + "";
    }

}

public class OrderExecReport
{
    internal int strategyId { get; set; }
    internal DateTime time { get; set; }
    internal int quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal price { get; set; }

    public OrderExecReport(int strategyId, DateTime time, int quantity, TypeOrder type_order, decimal price = 0)
    {
        this.strategyId= strategyId;
        this.time = time;
        this.quantity = quantity;
        this.typeOrder = type_order;
        this.price = price;
    }
    public OrderExecReport()
    {

    }

    public string PrintOrder()
    {
        return "order " + typeOrder + " done at " + time + " of " + quantity + " asset at " + price + "";
    }
}

public enum TypeOrder
{
    Market
}