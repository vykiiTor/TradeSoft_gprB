using System;

public class Strategy_Manager : TicksReceptor
{
	internal Market_Simulator Market;
	private List<Order> orders = new List<Order>();
	private string StrategyName;

    static Semaphore sendingOrder = new Semaphore(1, 1);
    public Semaphore getSendingOrder()
    {
        return sendingOrder;
    }
    public Strategy_Manager(Market_Simulator market, string strategyName)
	{
		Market = market;
		StrategyName = strategyName
	}
	public void RunStrategy ()
	{
		if ( (getObjectList()[getObjectList().Count-1].Price - getObjectList()[getObjectList().Count - 2].Price) > (decimal)0.1)
		{
			sendingOrder.WaitOne();
			Console.Write("blabla");
			sendingOrder.Release
		}
	}

	
}

