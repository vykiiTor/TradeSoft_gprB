using System;

public class RiskAnalyser : TicksReceptor
{
	Market_Simulator Market = new Market_Simulator();
	public RiskAnalyser(Market_Simulator market)
	{
		Market = market;
	}
}
