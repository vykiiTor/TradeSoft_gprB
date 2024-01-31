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

        public Position(int positionId, decimal price, int quantity)
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
    public List<Position> GetPositions() { return positions; }

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

    // we are here dealing only one asset, if dealing with multiple, add the assetId in parameter
    public int getNbrPositions ()
    {
        int getNbrPositions = 0;
        foreach (Position position in positions)
        {
            getNbrPositions += position.quantity;
        }
        return getNbrPositions;
    }

}