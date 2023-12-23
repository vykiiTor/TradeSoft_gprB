using System;

public class Backtesting_Engine
{
	private string data_ticks_path = "../../../tradesoft-ticks-sample.csv";
	private List<Ticks_Data> ticks = []; 

    public Backtesting_Engine()
	{
		Setup();
	}
	
	public void Setup ()
	{
        List<Ticks_Data> sample_ticks = Ticks_Data.csv_To_Ticks(data_ticks_path);
    }

	public static void Main(string[] args)
	{
		Backtesting_Engine engine = new Backtesting_Engine();
	}
}
