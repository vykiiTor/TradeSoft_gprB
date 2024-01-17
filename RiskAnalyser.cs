using System;

public class RiskAnalyser : TicksReceptor
{
    private List<Order> OrdersLog = new List<Order>();
    private Portfolio Portfolio;
    public RiskAnalyser()
	{
        Portfolio = new Portfolio(1000);
	}
    public Portfolio GetPortfolio()
    {
        return Portfolio;
    }

    public List<Order> GetOrdersLog ()
    {
        return OrdersLog;
    }

    // PnL ratio
    public decimal ProfitAndLoss()
    {
        return Portfolio.Cash - Portfolio.InitialCash;
    }

    public void StrategyReport()
    {
        Console.WriteLine(" Profit and Loss : "+ProfitAndLoss());
    }
}
