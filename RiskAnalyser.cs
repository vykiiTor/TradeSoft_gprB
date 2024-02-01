using System;
using Serilog;
using static Portfolio;


public class RiskAnalyser
{
    //risk analyser s'abonne au mm event que les start
    private List<OrderExecReport> ordersLog = new List<OrderExecReport>();
    private Portfolio portfolio;
    private MarketSimulator market;

    private decimal lastTickPriceReceived;
    public RiskAnalyser(MarketSimulator market, decimal startingPortfolioCash)
	{
        portfolio = new Portfolio(startingPortfolioCash);
        this.market = market;
        this.market.ReceiveOrder += ProcessOrderExecReport;
    }
    public Portfolio GetPortfolio()
    {
        return portfolio;
    }

    public List<OrderExecReport> GetOrdersLog ()
    {
        return ordersLog;
    }

    public void ProcessTick (decimal tickPrice)
    {
        lastTickPriceReceived = tickPrice;
    }

    // PnL ratio
    public decimal ProfitAndLoss()
    {
        return portfolio.GetCash() - portfolio.GetInitialCash() + portfolio.getPositionsQuantity()* lastTickPriceReceived;
    }

    public void StrategyReport()
    {
        Console.WriteLine(" Profit and Loss : "+ProfitAndLoss());
    }
    private void ProcessOrderExecReport(object sender, OrderExecEventArgs e)
    {
        OrderExecReport orderExecReport = e.Report;

        if (orderExecReport.Quantity > 0)
        {
            portfolio.cash -= orderExecReport.Quantity * orderExecReport.Price;
            portfolio.GetPositions().Add(new Position(GetNewPositionId(), orderExecReport.Price, orderExecReport.Quantity));
        }
        else if (orderExecReport.Quantity < 0)
        {
            int quantityToSell = -orderExecReport.Quantity;
            for (int i = 0; i < portfolio.GetPositions().Count; i++)
            {
                if (portfolio.GetPositions()[i].quantity > quantityToSell)
                {
                    Position tmp = portfolio.GetPositions()[i];
                    portfolio.cash += quantityToSell * orderExecReport.Price;
                    portfolio.GetPositions()[i] = new Position(tmp.positionId, tmp.price, tmp.quantity + quantityToSell);
                    break;
                }
                else
                {
                    quantityToSell -= portfolio.GetPositions()[i].quantity;
                    portfolio.cash += portfolio.GetPositions()[i].quantity * orderExecReport.Price;
                    portfolio.GetPositions().RemoveAt(i);
                }
                if (quantityToSell == 0)
                {
                    break;
                }
            }
        }
    }

}
