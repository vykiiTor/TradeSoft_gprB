using System;

public class RiskAnalyser : TicksReceptor
{
    private List<Order> OrdersLog = new List<Order>();
    public RiskAnalyser()
	{
	}

    public List<Order> GetOrdersLog ()
    {
        return OrdersLog;
    }
}
