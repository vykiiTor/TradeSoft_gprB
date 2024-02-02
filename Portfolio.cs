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

        public Position(int positionId, decimal price, int quantity)
        {
            this.positionId = positionId;
            this.price = price;
            this.quantity = quantity;
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
    public int getPositionsQuantity ()
    {
        int getNbrPositions = 0;
        foreach (Position position in positions)
        {
            getNbrPositions += position.quantity;
        }
        return getNbrPositions;
    }

    public override String ToString()
    {
        string print = "";
        foreach (Position position in positions)
        {
            print += ("PositionId : " + position.positionId + " ; price : " + position.price + " ; qtt : " + position.quantity) + "\n";
        }
        return print;
    }

    internal void ProcessOrderExecReport(OrderExecEventArgs e)
    {
        OrderExecReport orderExecReport = e.Report;

        if (orderExecReport.Quantity > 0)
        {
            //Console.WriteLine(" je cree un ordre de qqt " + orderExecReport.quantity);
            //Console.WriteLine("Buying " + orderExecReport.Quantity + " asset " + orderExecReport.StrategyId + " at " + orderExecReport.Price + " ; portfolio cash : " + portfolio.cash);
            cash -= orderExecReport.Quantity * orderExecReport.Price;
            GetPositions().Add(new Position(GetNewPositionId(), orderExecReport.Price, orderExecReport.Quantity));
        }
        else if (orderExecReport.Quantity < 0)
        {
            //Console.WriteLine(" process order " + portfolio.getPositionsQuantity());
            int quantityToSell = -orderExecReport.Quantity;
            //Console.WriteLine("Selling " + quantityToSell + " asset " + orderExecReport.StrategyId + " at " + orderExecReport.Price + " ; portfolio cash : " + portfolio.cash);
            for (int i = 0; i < GetPositions().Count; i++)
            {
                if (GetPositions()[i].quantity > quantityToSell)
                {
                    //Console.WriteLine(" first " + portfolio.GetPositions()[i].quantity);
                    // Console.WriteLine(" sec " + quantityToSell);
                    //Console.WriteLine(" 1selling " + quantityToSell + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    Position tmp = GetPositions()[i];
                    cash += quantityToSell * orderExecReport.Price;
                    //Console.WriteLine(" qtt av: " + portfolio.GetPositions()[i].quantity);
                    GetPositions()[i] = new Position(tmp.positionId, tmp.price, tmp.quantity + quantityToSell);
                    //Console.WriteLine(" 1sold " + quantityToSell + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    //Console.WriteLine(" qtt ap: " + portfolio.GetPositions()[i].quantity);
                    break;
                }
                else
                {
                    //Console.WriteLine(" 2selling " + portfolio.GetPositions()[i].quantity + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    quantityToSell -= GetPositions()[i].quantity;
                    cash += GetPositions()[i].quantity * orderExecReport.Price;
                    //Console.WriteLine("count avant " + portfolio.GetPositions().Count + " pos " + portfolio.getPositionsQuantity());
                    //portfolio.PrintPortfolio();
                    GetPositions().RemoveAt(i);
                    //Console.WriteLine("count apres " + portfolio.GetPositions().Count + " pos " + portfolio.getPositionsQuantity());
                    //portfolio.PrintPortfolio();
                    //Console.WriteLine(" 2sold " + portfolio.GetPositions()[i].quantity + " qqtPortfolio " + portfolio.getPositionsQuantity());

                }
                if (quantityToSell == 0)
                {
                    break;
                }
            }
        }
    }
}