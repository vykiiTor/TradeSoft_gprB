using System;

public class RiskAnalyser
{
    private List<OrderExecReport> OrdersLog = new List<OrderExecReport>();
    private Portfolio Portfolio;
    public RiskAnalyser()
	{
        Portfolio = new Portfolio(1000);
	}
    public Portfolio GetPortfolio()
    {
        return Portfolio;
    }

    public List<OrderExecReport> GetOrdersLog ()
    {
        return OrdersLog;
    }

    // PnL ratio
    public decimal ProfitAndLoss()
    {
        return Portfolio.cash - Portfolio.initialCash;
        // todo add the value of the bought asset in the portfolio
    }

    public void StrategyReport()
    {
        Console.WriteLine(" Profit and Loss : "+ProfitAndLoss());
    }
}
