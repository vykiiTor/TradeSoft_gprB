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

    private void ProcessOrderExecReport (object sender, OrderExecEventArgs e)
    {
        portfolio.ProcessOrderExecReport(e);
    }

}