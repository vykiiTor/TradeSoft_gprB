using System;
using Serilog;

public record Order (int StrategyId, int Quantity);
public record OrderExecReport(int StrategyId, DateTime Time, int Quantity, TypeOrder TypeOrder, decimal Price = 0);

public enum TypeOrder
{
    Market
}