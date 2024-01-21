using System;

public class Run
{
    public static void Main(string[] args)
    {
        MarketSimulator market = new MarketSimulator();
        RiskAnalyser risk = new RiskAnalyser();
        StrategyManager strategy = new StrategyManager(market, "Strat A");
        market.SetStrategyManager(strategy);

        BacktestingEngine engine = new BacktestingEngine(market, risk, strategy);

    }
}
