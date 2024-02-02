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
        portfolio.ProcessOrderExecReport(e);
    }

}
