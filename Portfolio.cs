public class Portfolio
{
    static int positionId = 0;

    public static int GetNewPositionId()
    {
        return positionId++;
    }

    public struct Position
    {
        internal readonly int positionId;
        internal readonly decimal price;
        internal int quantity;
        internal readonly string assetId;

        public Position(int positionId, decimal price, int quantity, string assetId)
        {
            this.positionId = positionId;
            this.price = price;
            this.quantity = quantity;
            this.assetId = assetId;
        }
    }

    internal decimal cash;
    internal decimal initialCash;
    List<Position> positions;

    public Portfolio(decimal cash)
    {
        this.cash = cash;
        this.initialCash = cash;
        this.positions = new List<Position>();

    }

    public decimal GetCash()
    {
        return this.cash;
    }

    public decimal GetInitialCash()
    {
        return this.initialCash;
    }

    public void ProcessOrder(OrderExecReport orderLog)
    {
        if (orderLog.quantity < 0)
        {
            Console.WriteLine("Buying " + orderLog.quantity + " asset " + orderLog.assetId + " at " + orderLog.price + " ; portfolio cash : " + this.cash);
            cash += orderLog.quantity * orderLog.price;
            positions.Add(new Position(GetNewPositionId(), orderLog.price, orderLog.quantity, orderLog.assetId));
        }
        else
        {
            int quantityToSell = orderLog.quantity;
            Console.WriteLine("Selling " + quantityToSell + " asset " + orderLog.assetId + " at " + orderLog.price + " ; portfolio cash : " + this.cash);
            for (int i = 0; i < positions.Count; i++)
            {
                {
                    if (positions[i].assetId == orderLog.assetId)
                    {
                        // look if there is others positions to sell the asset (to not be blocked where only 50% of the order quantity can be sold)
                        if (positions[i].quantity <= quantityToSell)
                        {
                            quantityToSell -= positions[i].quantity;
                            cash += positions[i].quantity * orderLog.price;
                            positions.RemoveAt(i);
                            i--;
                            if (quantityToSell == 0) break;
                        }
                        else
                        {
                            cash += quantityToSell * orderLog.price;
                            positions[i] = new Position(positions[i].positionId, positions[i].price,
                                    positions[i].quantity - quantityToSell, positions[i].assetId);
                            quantityToSell = 0;
                            break;
                        }
                    }
                }
            }
        }

    }
}