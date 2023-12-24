using System;

public class TicksReceptor
{
    static Semaphore syncTick = new Semaphore(1, 1);

    public Semaphore getSyncTick()
    {
        return syncTick;
    }
    public TicksReceptor()
    {

    }

    public virtual void ReceivePrices(Object sender, TickEventArgs e)
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
