using System;

public class Backtesting_Engine
{
	private List<Ticks_Data> ticks = new List<Ticks_Data>();

    Market_Simulator market = new Market_Simulator();

    public Backtesting_Engine(string data_tick_path = "../../../tradesoft-ticks-sample.csv")
	{
        ticks = Ticks_Data.csvToTicks(data_tick_path);
        market = new Market_Simulator();
		// create risk manager
		// create strategy manager

		// link all between them

        Thread backtester = new Thread(Send_Prices);
		backtester.Start();
	}

	// send prices to all component attached to the backtester (strategy, market and risk)
	public void Send_Prices()
	{
		foreach (var tick in ticks)
		{
			Console.WriteLine("Senging : "+tick.Price );
			//market.Receive_prices(tick.Time, tick.Price);
		}
	}

	public static void Main(string[] args)
	{
		Backtesting_Engine engine = new Backtesting_Engine();
	}
}
