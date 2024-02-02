using System;
using static MarketSimulator;
using static TicksData;
using static RiskAnalyser;
using static Portfolio;
using static Order;
using static OrderExecReport;
using static Strategy;
using static StrategyManager;
using System.IO;
using Serilog;
public class Run
{
    /* args[0] is the starting strategy portfolio cash
     * args[1] is the ticks data file (csv format)
     */
    public static void Main(string[] args)
    {
        var log = Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();
        try
        {
            if (args.Length == 2 && args!= null && decimal.TryParse(args[0], out decimal number) && File.Exists(args[1]))
            {
                MarketSimulator market = new MarketSimulator();
                RiskAnalyser risk = new RiskAnalyser(market, decimal.Parse(args[0]));
                StrategyManager strategy = new StrategyManager(market, decimal.Parse(args[0]));
                market.SetStrategyManager(strategy);
                BacktestingEngine engine = new BacktestingEngine(market, risk, strategy, args[1]);
                risk.StrategyReport();
            }
            else
            {
                throw new Exception();
            }
        }
        catch (Exception ex)
        {
            //Console.WriteLine("Please enter a valid unique decimal number and a valid file path");
            log.Information("Please enter a valid unique decimal number and a valid file path");
        }
    }
}