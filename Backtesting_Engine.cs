using System;

// Event arguments class to carry the data
// https://stackoverflow.com/questions/15766219/ok-to-use-dataeventargstdata-instead-of-customized-event-data-class
public class TickEventArgs : EventArgs
{
    public Ticks_Data Tick { get; }

    public TickEventArgs(Ticks_Data tick)
    {
        Tick = tick;
    }
}

public class Backtesting_Engine
{
	private List<Ticks_Data> ticks = new List<Ticks_Data>();

    Market_Simulator market = new Market_Simulator();

	public event EventHandler<TickEventArgs> TickSent;

    public Backtesting_Engine(string data_tick_path = "../../../tradesoft-ticks-sample.csv")
	{
        ticks = Ticks_Data.csvToTicks(data_tick_path);
        market = new Market_Simulator();
		// create risk manager
		// create strategy manager

		// link all between them



        Thread backtester = new Thread(SendPrices);
		backtester.Start();
	}

	// send prices to all component attached to the backtester (strategy, market and risk)
	public void SendPrices()
	{
		foreach (var tick in ticks)
		{
			Console.WriteLine("Senging : "+tick.Price );
			OnPriceSend(new TickEventArgs(tick));
		}
	}

	protected virtual void OnPriceSend(TickEventArgs e)
	{
		TickSent?.Invoke(this, e);
	}

	public static void Main(string[] args)
	{
		Backtesting_Engine engine = new Backtesting_Engine();
		engine.TickSent += engine.market.ReceivePrices;
	}
}
