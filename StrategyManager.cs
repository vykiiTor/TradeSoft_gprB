using System;
using System.Diagnostics;

public class StrategyManager
{
	private MarketSimulator market;
    private List<OrderExecReport> ordersLog = new List<OrderExecReport>();
    public List<OrderExecReport> GetOrdersLog () {  return ordersLog; }

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
            int quantityToBuy = strategy.run();
            if (quantityToBuy != 0)
            {
                Order order = new Order("1", DateTime.Now, quantityToBuy, TypeOrder.Market);
                OrderExecReport orderLog = market.ReceiveOrder(order);
                ordersLog.Add(orderLog); strategy.portfolio.ProcessOrder(orderLog);
                //Console.WriteLine(orderLog.PrintOrder());
            }

            // send ticks
            // wait to receive an Order from the Strategy
            // send the OrderExecReport to the strategy
        }
    }
    
}

public interface IStrategy
{
    public int run();
}

public class Strategy : IStrategy
{
    internal String strategyName;
    internal Portfolio portfolio;

    public Strategy (String strategyName)
    {
        this.strategyName = strategyName;
        this.portfolio = new Portfolio (1000);
    }

    // return the quantity to buy to its not conveniant
    public int run ()
    {
        if (portfolio.GetCash() >= 0)
            // return asset quantity to buy
            return -1;
        return 0;
    }

}