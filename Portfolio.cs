public class Portfolio // We assess that, in our case, a portfolio can handle only one asset at once
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