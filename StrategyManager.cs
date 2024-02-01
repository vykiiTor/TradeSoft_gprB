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

    /* run fonction that output an order
     * each strategy will have a portofolio assiociated, how is risk manager assiociated
     * strategy manager will handle ticks data and order sending/reception
     * 
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
        if (newPrice < 16 && portfolio.cash > market.GetMarketPrice())
        {
            //Console.WriteLine(" buying " + newPrice+" cash : "+ portfolio.cash+ " nbr pos : "+portfolio.getNbrPositions());
            market.ProcessOrder(new Order(strategyId, DateTime.Now, 1, TypeOrder.Market));

        }
        else if (newPrice > 25 && portfolio.getPositionsQuantity() > 0)
        {
            //Console.WriteLine(" selling " + newPrice + " cash : " + portfolio.cash + " nbr pos : " + portfolio.getNbrPositions());
            market.ProcessOrder(new Order(strategyId, DateTime.Now, -1, TypeOrder.Market));
        }

        // get order exec report

        //OrderExecReport OrderExec = market.ProcessOrder(order);
        //portfolio.ProcessOrder(OrderExec);

    }

    public void processOrderExecReport(OrderExecReport orderExecReport)
    {
        if (orderExecReport.quantity > 0)
        {
            //Console.WriteLine(" je cree un ordre de qqt " + orderExecReport.quantity);
            Console.WriteLine("Buying " + orderExecReport.quantity + " asset " + orderExecReport.strategyId + " at " + orderExecReport.price + " ; portfolio cash : " + portfolio.cash);
            portfolio.cash -= orderExecReport.quantity * orderExecReport.price;
            portfolio.GetPositions().Add(new Position(GetNewPositionId(), orderExecReport.price, orderExecReport.quantity));
        }
        else if (orderExecReport.quantity < 0)
        {
            //Console.WriteLine(" process order " + portfolio.getPositionsQuantity());
            int quantityToSell = -orderExecReport.quantity;
            Console.WriteLine("Selling " + quantityToSell + " asset " + orderExecReport.strategyId + " at " + orderExecReport.price + " ; portfolio cash : " + portfolio.cash);
            for( int i=0; i<portfolio.GetPositions().Count;i++)
            {
                if (portfolio.GetPositions()[i].quantity > quantityToSell)
                {
                    //Console.WriteLine(" first " + portfolio.GetPositions()[i].quantity);
                   // Console.WriteLine(" sec " + quantityToSell);
                    //Console.WriteLine(" 1selling " + quantityToSell + " qqtPortfolio " + portfolio.getPositionsQuantity());
                    Position tmp = portfolio.GetPositions()[i];
                    portfolio.cash += quantityToSell * orderExecReport.price;
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
                    portfolio.cash += portfolio.GetPositions()[i].quantity * orderExecReport.price;
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