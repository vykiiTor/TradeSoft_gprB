using System;
using System.Reflection;

// Event arguments class to carry the data
// https://stackoverflow.com/questions/15766219/ok-to-use-dataeventargstdata-instead-of-customized-event-data-class

public class Backtesting_Engine
{
	private List<Ticks_Data> Ticks = new List<Ticks_Data>();

	internal Market_Simulator Market;
	internal RiskAnalyser Risk;
	internal Strategy_Manager Strategy;
	
	//To be removed
	public event EventHandler<ObjectEventArgs<Ticks_Data>> TickSent;
	

    public Backtesting_Engine(Market_Simulator simulator, RiskAnalyser analyser, Strategy_Manager strategy,
		string data_tick_path = "../../../tradesoft-ticks-sample.csv")
	{
		Market = simulator;
		Risk = analyser;
		Strategy = strategy;

		Ticks = Ticks_Data.csvToTicks(data_tick_path);

		SendPrices();
        
		Risk.StrategyReport();

        /*List<Ticks_Data> test = new List<Ticks_Data>();
        for (int i = 0; i < 10000; i++)
        {
            test.Add(new Ticks_Data(DateTime.MinValue.AddSeconds(i), 1, i));
        } //for testing purpose
		Ticks = test;*/

		Risk.StrategyReport();
	}

	// send prices to all component attached to the backtester (strategy, market and risk)
	public void SendPrices()
	{
		foreach (var tick in Ticks)
		{
			//Console.WriteLine("Senging : "+tick.Price );
            OnPriceSend(new ObjectEventArgs<Ticks_Data>(tick));
		}
    }

	protected virtual void OnPriceSend(ObjectEventArgs<Ticks_Data> e)
	{
		TickSent?.Invoke(this, e);
	}

	
}
