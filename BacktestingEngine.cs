using System;
using System.Globalization;
using System.Reflection;
using Serilog;
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
		
		IEnumerable<TicksData> ticks = TicksData.BuildEnum();
		foreach (var tick in ticks)
		{
			market.UpdateMarketPrice(tick.price);
			strategy.RunStategies(tick.price);
			risk.ProcessTick(tick.price);
		}
    }
    
}
