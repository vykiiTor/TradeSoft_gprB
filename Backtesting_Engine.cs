using System;
using System.Reflection;

// Event arguments class to carry the data
// https://stackoverflow.com/questions/15766219/ok-to-use-dataeventargstdata-instead-of-customized-event-data-class

public class Backtesting_Engine
{
	private List<Ticks_Data> ticks = new List<Ticks_Data>();

	internal Market_Simulator Market;
	internal RiskAnalyser Risk;
	internal Strategy_Manager Strategy;
	
	

    public Backtesting_Engine(Market_Simulator simulator, RiskAnalyser analyser, Strategy_Manager strategy,
		string filePath = "../../../tradesoft-ticks-sample.csv")
	{
		Market = simulator;
		Risk = analyser;
		Strategy = strategy;

		ticks = Ticks_Data.csvToTicks(filePath);

		foreach (var tick in ticks)
		{
			//Console.WriteLine("Senging : "+tick.Price );
			Market.UpdateMarketPrice(tick.Price);
			/* refacto the data recep
			Risk.DataReception;
			Strategy.DataReception;*/
			Strategy.RunStrategy();
			
		}
        
		Risk.StrategyReport();

        /*List<Ticks_Data> test = new List<Ticks_Data>();
        for (int i = 0; i < 10000; i++)
        {
            test.Add(new Ticks_Data(DateTime.MinValue.AddSeconds(i), 1, i));
        } //for testing purpose
		Ticks = test;*/
        
	}
}
