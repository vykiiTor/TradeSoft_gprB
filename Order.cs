using System;

public class Order
{
    internal string assetId { get; set; }
    internal DateTime time { get; set; }
    internal int quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal price { get; set; }

    public Order()
    {

    }
    public Order(string assetId, DateTime time, int quantity, TypeOrder type_order, decimal price = 0)
    {
        this.assetId = assetId;
        this.time = time;
        this.quantity = quantity;
        this.typeOrder = type_order;
        this.price = price;
    }

    public string printOrder()
    {
        return "order " + typeOrder + " request done at " + time + " of " + quantity + " asset at " + price + "";
    }

}

public class OrderExecReport
{
    internal string assetId { get; set; }
    internal DateTime time { get; set; }
    internal int quantity { get; set; }
    internal TypeOrder typeOrder { get; set; }
    internal decimal price { get; set; }

    public OrderExecReport(string assetId, DateTime time, int quantity, TypeOrder type_order, decimal price = 0)
    {
        this.assetId = assetId;
        this.time = time;
        this.quantity = quantity;
        this.typeOrder = type_order;
        this.price = price;
    }
    public OrderExecReport()
    {

    }

    public string printOrder()
    {
        return "order " + typeOrder + " done at " + time + " of " + quantity + " asset at " + price + "";
    }
}

public enum TypeOrder
{
    Market
}