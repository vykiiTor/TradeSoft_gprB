using System;

public class Market_Simulator
{
	public Market_Simulator()
	{
		
	}

    public void ReceivePrices (Object sender, TickEventArgs e)
    {
        Thread market = new Thread(() =>
        {
            Console.WriteLine($"Received data: {e.Tick.Price}");
        });
        market.Start();
    }

}
