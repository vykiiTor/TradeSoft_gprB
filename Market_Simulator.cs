using System;

public class Market_Simulator
{
    static Semaphore syncTick = new Semaphore(1, 1);

    public Semaphore getSyncTick ()
    {
        return syncTick;
    }
    public Market_Simulator()
	{
		
	}

    public void ReceivePrices (Object sender, TickEventArgs e)
    {
        Thread market = new Thread(() =>
        {
            syncTick.WaitOne();
            Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
            syncTick.Release();
        });
        market.Start();
    }

}
