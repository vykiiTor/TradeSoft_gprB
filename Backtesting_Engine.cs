using System;
using System.Reflection;

// Event arguments class to carry the data
// https://stackoverflow.com/questions/15766219/ok-to-use-dataeventargstdata-instead-of-customized-event-data-class

public class Backtesting_Engine
{
	private List<Ticks_Data> ticks = new List<Ticks_Data>();

	private Market_Simulator market;
	private RiskAnalyser risk;
	private Strategy_Manager strategy;
	
    public Backtesting_Engine(Market_Simulator market, RiskAnalyser risk, Strategy_Manager strategy,
		string filePath = "../../../tradesoft-ticks-sample.csv")
	{
		this.market = market;
		this.risk = risk;
		this.strategy = strategy;
		ticks = Ticks_Data.csvToTicks(filePath);
		foreach (var tick in ticks)
		{
			//Console.WriteLine("Senging : "+tick.Price );
			market.UpdateMarketPrice(tick.Price);
			strategy.RunStrategy();
		}
		risk.StrategyReport();

        /*List<Ticks_Data> test = new List<Ticks_Data>();
        for (int i = 0; i < 10000; i++)
        {
            test.Add(new Ticks_Data(DateTime.MinValue.AddSeconds(i), 1, i));
        } //for testing purpose
		Ticks = test;*/
        
	}
    //getList
    //getListAtoB //past only
}
