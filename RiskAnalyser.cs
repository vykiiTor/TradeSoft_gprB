using System;

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
        return portfolio.cash - portfolio.initialCash;
        // todo add the value of the bought asset in the portfolio
    }

    public void StrategyReport()
    {
        Console.WriteLine(" Profit and Loss : "+ProfitAndLoss());
    }
}
