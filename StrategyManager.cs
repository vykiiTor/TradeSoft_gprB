using System;
using System.Diagnostics;
using static Portfolio;
using Serilog;


public class StrategyManager
{
	private MarketSimulator market;

    public List<Strategy> strategies = new List<Strategy>();

    public Strategy GetStrategy (int strategyId)
    {
        return strategies[strategyId];
    }

    public StrategyManager(MarketSimulator market, decimal startingPortfolioCash)
	{
		this.market = market;
        Strategy stratA = new Strategy(0, market, startingPortfolioCash);
        strategies.Add(stratA);
    }


    public void RunStategies (decimal ticksPrice)
    {
        foreach (Strategy strategy in strategies)
        {
            strategy.RunStrategy(ticksPrice);            
        }
    }
    
}

public interface IStrategy
{
    public void RunStrategy(decimal ticksPrice);
}

public class Strategy : IStrategy
{
    //une fois l'event recu on check stratId et faire qq chose si c'est bien notre id
    
    internal int strategyId;
    private MarketSimulator market;
    private Portfolio portfolio;

    public Strategy (int strategyId, MarketSimulator market, decimal startingPortfolioCash)
    {
        this.strategyId = strategyId;
        this.market = market;
        // change dynamically the portfolio cash with main args
        this.portfolio = new Portfolio (startingPortfolioCash);
        this.market.ReceiveOrder += ProcessOrderExecReport;

    }

    public void RunStrategy (decimal ticksPrice)
    {
        // check if the strat needs to do an action
        if (ticksPrice < 16 && portfolio.cash > ticksPrice)
        {
            //Console.WriteLine(" buying " + newPrice+" cash : "+ portfolio.cash+ " nbr pos : "+portfolio.getNbrPositions());
            market.ProcessOrder(strategyId, 1);

        }
        else if (ticksPrice > 25 && portfolio.getPositionsQuantity() > 0)
        {
            //Console.WriteLine(" selling " + newPrice + " cash : " + portfolio.cash + " nbr pos : " + portfolio.getNbrPositions());
            market.ProcessOrder(strategyId, -1);
        }

    }

    private void ProcessOrderExecReport(object sender, OrderExecEventArgs e)
    {
        OrderExecReport orderExecReport = e.Report;

        if (orderExecReport.Quantity > 0)
        {
            //Console.WriteLine(" je cree un ordre de qqt " + orderExecReport.quantity);
            //Console.WriteLine("Buying " + orderExecReport.Quantity + " asset " + orderExecReport.StrategyId + " at " + orderExecReport.Price + " ; portfolio cash : " + portfolio.cash);
            portfolio.cash -= orderExecReport.Quantity * orderExecReport.Price;
            portfolio.GetPositions().Add(new Position(GetNewPositionId(), orderExecReport.Price, orderExecReport.Quantity));
        }
        else if (orderExecReport.Quantity < 0)
        {
            //Console.WriteLine(" process order " + portfolio.getPositionsQuantity());
            int quantityToSell = -orderExecReport.Quantity;
            //Console.WriteLine("Selling " + quantityToSell + " asset " + orderExecReport.StrategyId + " at " + orderExecReport.Price + " ; portfolio cash : " + portfolio.cash);
            for( int i=0; i<portfolio.GetPositions().Count;i++)
            {
                if (portfolio.GetPositions()[i].quantity > quantityToSell)
                {
                    //Console.WriteLine(" first " + portfolio.GetPositions()[i].quantity);
                   // Console.WriteLine(" sec " + quantityToSell);
                    //Console.WriteLine(" 1selling " + quantityToSell + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    Position tmp = portfolio.GetPositions()[i];
                    portfolio.cash += quantityToSell * orderExecReport.Price;
                    //Console.WriteLine(" qtt av: " + portfolio.GetPositions()[i].quantity);
                    portfolio.GetPositions()[i] = new Position(tmp.positionId, tmp.price, tmp.quantity + quantityToSell);
                    //Console.WriteLine(" 1sold " + quantityToSell + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    //Console.WriteLine(" qtt ap: " + portfolio.GetPositions()[i].quantity);
                    break;
                }
                else
                {
                    //Console.WriteLine(" 2selling " + portfolio.GetPositions()[i].quantity + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    quantityToSell -= portfolio.GetPositions()[i].quantity;
                    portfolio.cash += portfolio.GetPositions()[i].quantity * orderExecReport.Price;
                    //Console.WriteLine("count avant " + portfolio.GetPositions().Count + " pos " + portfolio.getPositionsQuantity());
                    //portfolio.PrintPortfolio();
                    portfolio.GetPositions().RemoveAt(i);
                    //Console.WriteLine("count apres " + portfolio.GetPositions().Count + " pos " + portfolio.getPositionsQuantity());
                    //portfolio.PrintPortfolio();
                    //Console.WriteLine(" 2sold " + portfolio.GetPositions()[i].quantity + " qqtPortfolio " + portfolio.getPositionsQuantity());

                }
                if (quantityToSell ==0)
                {
                    break;
                }
            }
        }
    }
}