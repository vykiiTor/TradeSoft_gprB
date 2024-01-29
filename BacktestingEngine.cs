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
		 *  market.UpdateMarketPrice(data.price);//bye
		   	    strategy.RunStrategy();
		 */
		CsvToTicks(filePath);
		
		
		//to be removed
		/*foreach (var tick in ticks)
		{
			//Console.WriteLine("Senging :  "+tick.price );
			this.market.UpdateMarketPrice(tick.price);
			this.strategy.RunStrategy();
		}
		this.risk.StrategyReport();

        List<TicksData> test = new List<TicksData>();
        for (int i = 0; i < 10000; i++)
        {
            test.Add(new TicksData(DateTime.MinValue.AddSeconds(i), 1, i));
        } //for testing purpose
		Ticks = test;*/
	}
    public  void CsvToTicks(String filePath)
    {
	    using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
	    using (StreamReader reader = new StreamReader(fileStream))
	    {
		    reader.ReadLine();

		    string line;
		    while ((line = reader.ReadLine()) != null)
		    {
			    ProcessCsvLine(line);//bye into -> yield return line
		    }
	    }
    }
    
    public void ProcessCsvLine(string csvLine) //IEnnumerable to tickdata
    {
	    string[] columns = csvLine.Split(',');
	    
	    //revoir la list 
	    //List<TicksData> ticksDatas = new List<TicksData>();
	    TicksData data = new TicksData(
		    DateTime.ParseExact(columns[0], "mm:ss.f", null),
		    Int32.Parse(columns[2]),
		    decimal.Parse(columns[3], CultureInfo.InvariantCulture));
	    //ticksDatas.Add(data);
	    
	    market.UpdateMarketPrice(data.price);//bye
	    strategy.RunStrategy();
    }
}
