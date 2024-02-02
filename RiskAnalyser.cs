using System;
using System.Text;
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

    public void StrategyReport(string strategiesName)
    {
        string path = "./../../../TradeSoft-strategy-report.txt";
        using (FileStream fs = File.Create(path))
        {
            string dataasstring = (strategiesName+"\nProfit and Loss : " + ProfitAndLoss());
            byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
            fs.Write(info, 0, info.Length);
        }
    }

    private void ProcessOrderExecReport(object sender, OrderExecEventArgs e)
    {
        portfolio.ProcessOrderExecReport(e);
    }

}
