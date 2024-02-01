using System;

public class Run
{
    // todo use args
    public static void Main(string[] args)
    {
        MarketSimulator simulator = new MarketSimulator();
        RiskAnalyser risk = new RiskAnalyser();
        StrategyManager strategy = new StrategyManager(simulator, risk, "Strat A");
        simulator.SetStrategyManager(strategy);

        BacktestingEngine engine = new BacktestingEngine(simulator, risk, strategy);

    }
}
