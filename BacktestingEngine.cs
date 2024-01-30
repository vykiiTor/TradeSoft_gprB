using System;
using System.Globalization;
using System.Reflection;

// Event arguments class to carry the data
// https://stackoverflow.com/questions/15766219/ok-to-use-dataeventargstdata-instead-of-customized-event-data-class

public class BacktestingEngine
{
	private List<TicksData> ticks = new List<TicksData>();

	private MarketSimulator market { get; set; }
	private RiskAnalyser risk;
	private StrategyManager strategy;
	
    public BacktestingEngine(MarketSimulator market, RiskAnalyser risk, StrategyManager strategy,
		string filePath = "../../../tradesoft-ticks-sample.csv")
	{
		this.market = market;
		this.risk = risk;
		this.strategy = strategy;
		
		//for IEnumerable do 
		/*
		 *  market.UpdateMarketPrice(data.price);
		   	    strategy.RunStrategy();
		 */
		//to check above 
		TicksData.CsvToTicks(filePath);
		
		
		// to update
		//market.UpdateMarketPrice(data.price);
		//strategy.RunStategies();


    }
    
}
