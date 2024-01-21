public class Portfolio // We assess that, in our case, a portfolio can handle only one asset at once
{
    private decimal cash;
    private decimal initialCash;
    private long quantity;

    public Portfolio (decimal cash)
    {
        this.cash = cash;
        initialCash = cash;
        quantity = 0;

    }
    public decimal GetCash()
    {
        return cash;
    }
    public long GetQuantity()
    {
        return quantity;
    }
    public decimal GetInitialCash()
    {
        return initialCash;
    }
    public void ProcessOrder (Order order)
    {
        lock (this)//check this lock cuz not thread
        {
            quantity += order.quantity;
            if (quantity > 0)
            {
                cash -= order.quantity * order.price;
            }
            else if (quantity < 0)
            {
                cash += order.quantity * order.price;
            }
            Console.WriteLine("Cash du portefeuille : " + cash + " ; quantite du portefeuille : " + quantity);
        }
    }

}