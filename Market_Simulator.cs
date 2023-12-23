using System;

public class Market_Simulator
{
	public Market_Simulator()
	{
		Thread market = new Thread(ReceivePrices);
		market.Start();
	}

	public void ReceivePrices()
	{
		while(true)
		{
			//OnReceivePrices
		}
	}

    public void OnReceivePrices(DateTime time, decimal price)
    {
        //Console.WriteLine("Time : " + time.ToString() + " ; Price : " + price.ToString());
    }
}
