using System;
using System.Diagnostics;
using static Portfolio;

public class StrategyManager
{
	private MarketSimulator market;

    public List<Strategy> strategies = new List<Strategy>();

    public Strategy GetStrategy (int strategyId)
    {
        return strategies[strategyId];
    }

    /* run fonction that output an order
     * each strategy will have a portofolio assiociated, how is risk manager assiociated
     * strategy manager will handle ticks data and order sending/reception
     */



    public StrategyManager(MarketSimulator market, RiskAnalyser risk, string strategyName)
	{
		this.market = market;
        Strategy stratA = new Strategy(0);
        strategies.Add(stratA);
    }

    public void RunStategies ()
    {
        foreach (Strategy strategy in strategies)
        {
            strategy.RunStrategy(market);

            // wait to receive an Order from the Strategy
            
        }
    }
    
}

public interface IStrategy
{
    public void RunStrategy(MarketSimulator market);
}

public class Strategy : IStrategy
{
    internal int strategyId;
    private Portfolio portfolio;

    public Strategy (int strategyId)
    {
        this.strategyId = strategyId;
        this.portfolio = new Portfolio (1000);
    }

    // return the quantity to buy to its not conveniant
    public void RunStrategy (MarketSimulator market)
    {
        decimal newPrice = market.GetMarketPrice();

        // check if the strat needs to do an action
        if (newPrice < 16 && portfolio.cash > 0)
        {
            market.ProcessOrder(new Order(strategyId, DateTime.Now, 1, TypeOrder.Market));

        }
        else if (newPrice > 25) { }
        {
            market.ProcessOrder(new Order(strategyId, DateTime.Now, -1, TypeOrder.Market));
        }

        // get order exec report

        //OrderExecReport OrderExec = market.ProcessOrder(order);
        //portfolio.ProcessOrder(OrderExec);

    }

    public void processOrderExecReport(OrderExecReport orderExecReport)
    {
        // si c'est un achat
            // on créer une position
        if (orderExecReport.quantity > 0)
        {
            Console.WriteLine("Buying " + orderExecReport.quantity + " asset " + orderExecReport.strategyId + " at " + orderExecReport.price + " ; portfolio cash : " + portfolio.cash);
            portfolio.cash += orderExecReport.quantity * orderExecReport.price;
            portfolio.GetPositions().Add(new Position(GetNewPositionId(), orderExecReport.price, orderExecReport.quantity));
        }
        // si c'est une vente
        // on cherche 
        else if (orderExecReport.quantity < 0)
        {
            int nbrPositionsInPortfolio = portfolio.getNbrPositions();
            int quantityToSell = orderExecReport.quantity;
            Console.WriteLine("Selling " + quantityToSell + " asset " + orderExecReport.strategyId + " at " + orderExecReport.price + " ; portfolio cash : " + portfolio.cash);
            for (int i = 0; i < portfolio.GetPositions().Count; i++)
            {
                // look if there is others positions to sell the asset (to not be blocked where only 50% of the order quantity can be sold)
                if (portfolio.GetPositions()[i].quantity <= quantityToSell)
                {
                    quantityToSell -= portfolio.GetPositions()[i].quantity;
                    portfolio.cash += portfolio.GetPositions()[i].quantity * orderExecReport.price;
                    portfolio.GetPositions().RemoveAt(i);
                    i--;
                    if (quantityToSell == 0) break;
                }
                else
                {
                    portfolio.cash += quantityToSell * orderExecReport.price;
                    portfolio.GetPositions()[i] = new Position(portfolio.GetPositions()[i].positionId, portfolio.GetPositions()[i].price,
                            portfolio.GetPositions()[i].quantity - quantityToSell);
                    quantityToSell = 0;
                    break;
                }
                if (quantityToSell == 0) break;
            }
        }
    }
}