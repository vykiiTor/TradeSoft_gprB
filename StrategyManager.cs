using System;
using System.Diagnostics;

public class StrategyManager
{
	private MarketSimulator market;

    public List<Strategy> strategies = new List<Strategy>();
    


    /* run fonction that output an order
     * each strategy will have a portofolio assiociated, how is risk manager assiociated
     * strategy manager will handle ticks data and order sending/reception
     */



    public StrategyManager(MarketSimulator market, RiskAnalyser risk, string strategyName)
	{
		this.market = market;
        Strategy stratA = new Strategy("StratA");
        strategies.Add(stratA);
    }

    public void RunStategies ()
    {
        foreach (Strategy strategy in strategies)
        {
            strategy.RunStrategy(market);

            // send ticks
            // wait to receive an Order from the Strategy
            // send the OrderExecReport to the strategy
        }
    }
    
}

public interface IStrategy
{
    public void RunStrategy(MarketSimulator market);
}

public class Strategy : IStrategy
{
    internal String strategyName;
    private Portfolio portfolio;

    public Strategy (String strategyName)
    {
        this.strategyName = strategyName;
        this.portfolio = new Portfolio (1000);
    }

    // return the quantity to buy to its not conveniant
    public void RunStrategy (MarketSimulator market)
    {
        // check if the strat needs to do an action
        Order order = new Order("1", DateTime.Now, 1, TypeOrder.Market);
        OrderExecReport OrderExec = market.ProcessOrder(order);
        portfolio.ProcessOrder(OrderExec);
    }

}