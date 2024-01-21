using System;

public class RiskAnalyser
{
    private List<Order> ordersLog = new List<Order>();
    private Portfolio portfolio;
    public RiskAnalyser()
	{
        portfolio = new Portfolio(1000);
	}
    public Portfolio GetPortfolio()
    {
        return portfolio;
    }

    public List<Order> GetOrdersLog ()
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
