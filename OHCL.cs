using System;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

// author: Antonin Boyon
public class Ticks_Data
{
    internal DateTime Time { get; set; }
    internal long Quantity { get; set; }
    internal decimal Price { get; set; }



    public Ticks_Data()
	{
		Time = DateTime.MinValue;
		Quantity = -1;
		Price = -1;
	}

	public Ticks_Data(DateTime time, long quantity, decimal price)
	{
		Time = time;
		Quantity = quantity;
		Price = price;
	}
	
	override
	public String ToString ()
	{
		return Time+","+Quantity+","+Price;
	}

	public static void Print_Ticks_List (List<Ticks_Data> list) 
	{
		foreach (Ticks_Data t in list)
		{
			Console.WriteLine(t.ToString());
		}
	}

    // transform a csv file (with ',' separator) into a List<Ticks_Data>
    public static List<Ticks_Data> csv_To_Ticks (String filepath)
	{
		List<Ticks_Data> ticks_Datas = new List<Ticks_Data>();

        // http://dotnet-tutorials.net/Article/read-a-csv-file-in-csharp
        string[] lines = System.IO.File.ReadAllLines(filepath);
        foreach (string line in lines)
        {
            // check if the line is not a header
			if (!Regex.IsMatch(line, @"^[a-zA-Z,]+$"))
			{
				
				string[] columns = line.Split(',');
                Ticks_Data data = new Ticks_Data(
					DateTime.ParseExact(columns[0], "yyyy-MM-dd HH:mm:ss.fff", null),
					Int32.Parse(columns[2]),
					decimal.Parse(columns[3]));
				ticks_Datas.Add(data);
			}
        }

        return ticks_Datas;
	}

	
}

public class OHCL_Data
{
	private DateTime Time { get; set; }
	private decimal Open { get; set; }
    private decimal High { get; set; }
    private decimal Close { get; set; }
    private decimal Low { get; set; }

	public OHCL_Data ()
	{
		Time = DateTime.MinValue; Open = -1; High = -1; Close = -1; Low = -1;
	}

	public OHCL_Data(DateTime time, decimal open, decimal high, decimal close, decimal low)
    {
        Time = time;
        Open = open;
        High = high;
        Close = close;
        Low = low;
    }

    public OHCL_Data(DateTime time, decimal open)
    {
        Time = time;
        Open = open;
        High = open;
        Close = -1;
        Low = open;
    }

    // Transform ticks (List<Ticks_Data>) into an OHCL of period (milliseconds)
    public static List<OHCL_Data> Ticks_To_OHCL (List<Ticks_Data> ticks, long period)
	{
        List<OHCL_Data> oHCL_Datas = new List<OHCL_Data>();

		DateTime firstPeriod = ticks[0].Time;
        // the first period is based of the first tick
		OHCL_Data oHCL_Data = new OHCL_Data(ticks[0].Time, ticks[0].Price);
		foreach (Ticks_Data tick in ticks)
		{
            //Console.WriteLine(ticks[ticks.IndexOf(tick) + 1].Time.Millisecond - firstPeriod.Millisecond);
            // check if the tick is the last of the file
            if (ticks.IndexOf(tick) == ticks.Count - 1)
            {
                oHCL_Data.Close = tick.Price;
                oHCL_Datas.Add(oHCL_Data);
            }
            // check if the current tick is the last of the period
            
            else if ((ticks[ticks.IndexOf(tick) + 1].Time.Millisecond - firstPeriod.Millisecond) > period)
            {
                //Console.WriteLine("inside");
                oHCL_Data.Close = tick.Price;
                //Console.WriteLine(oHCL_Data.ToString());
                oHCL_Datas.Add(oHCL_Data);
                // create the next period based on the next tick
                oHCL_Data = new OHCL_Data(ticks[ticks.IndexOf(tick) + 1].Time, ticks[ticks.IndexOf(tick) + 1].Price);
            }
            if (tick.Price > oHCL_Data.High)
            {
                oHCL_Data.High = tick.Price;
            }
            else if (tick.Price < oHCL_Data.Low)
            {
                oHCL_Data.Low = tick.Price;
            }
        }

		return oHCL_Datas;
    }

    override
    public String ToString()
    {
        return Time + "," + Open + "," + High + "," + Close + "," + Low;
    }

    public static void Print_OHCL_List(List<OHCL_Data> list)
    {
		Console.WriteLine(list.Count);

        foreach (OHCL_Data d in list)
        {
            Console.WriteLine(d.ToString());
        }
    }

    public static void Main(string[] args)
    {
        // console testing
        String path = "../../../tradesoft-ticks-sample.csv";
		List<Ticks_Data> list = Ticks_Data.csv_To_Ticks(path);
		List<OHCL_Data> oHCL_Datas = OHCL_Data.Ticks_To_OHCL(list,100);
		OHCL_Data.Print_OHCL_List(oHCL_Datas);
    }
}


