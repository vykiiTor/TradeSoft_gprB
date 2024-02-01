using System;
using Serilog;


public class RiskAnalyser
{
    private List<OrderExecReport> ordersLog = new List<OrderExecReport>();
    private Portfolio portfolio;
    public RiskAnalyser()
	{
        portfolio = new Portfolio(1000);
	}
    public Portfolio GetPortfolio()
    {
        return portfolio;
    }

    public List<OrderExecReport> GetOrdersLog ()
    {
        return ordersLog;
    }

    // PnL ratio
    public decimal ProfitAndLoss()
    {
        return portfolio.GetCash() - portfolio.GetInitialCash();
    }

    public void StrategyReport()
    {
        Console.WriteLine(" Profit and Loss : "+ProfitAndLoss());
    }
}
