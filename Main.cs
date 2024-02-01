using System;
using static MarketSimulator;
using static TicksData;
using static RiskAnalyser;
using static Portfolio;
using static Order;
using static OrderExecReport;
using static Strategy;
using static StrategyManager;

public class Run
{
    /* args[0] is the starting strategy portfolio cash
     */
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length == 1 && args!= null && decimal.TryParse(args[0], out decimal number))
            {
                MarketSimulator simulator = new MarketSimulator();
                RiskAnalyser risk = new RiskAnalyser();
                StrategyManager strategy = new StrategyManager(simulator, risk, decimal.Parse(args[0]));
                simulator.SetStrategyManager(strategy);
                BacktestingEngine engine = new BacktestingEngine(simulator, risk, strategy);
            }
            else
            {
                throw new Exception();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Please enter a valid unique decimal number like 150.34 or 1000.");
        }
    }
}