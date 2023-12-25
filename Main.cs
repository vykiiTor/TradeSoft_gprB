using System;

public class Run
{
    public static void Main(string[] args)
    {
        Market_Simulator simulator = new Market_Simulator();
        RiskAnalyser risk = new RiskAnalyser();
        Strategy_Manager strategy = new Strategy_Manager(simulator, "Strat A");
        simulator.setStrategyManager(strategy);
        simulator.setRiskAnalyser(risk);

        Backtesting_Engine engine = new Backtesting_Engine(simulator, risk, strategy);

    }
}
