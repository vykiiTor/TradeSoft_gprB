using System;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Globalization;

// author: Antonin Boyon
public class TicksData
{
    internal DateTime time { get; set; }
    internal long quantity { get; set; }
    internal decimal price { get; set; }

    public TicksData()
    {
        time = DateTime.MinValue;
        quantity = -1;
        price = -1;
    }

    public TicksData(DateTime time, long quantity, decimal price)
    {
        this.time = time;
        this.quantity = quantity;
        this.price = price;
    }

    override
    public String ToString()
    {
        return time + "," + quantity + "," + price;
    }

    public static void PrintTicksList(List<TicksData> list)
    {
        foreach (TicksData t in list)
        {
            Console.WriteLine(t.ToString());
        }
    }

    // transform a csv file (with ',' separator) into a List<TicksData>
    public static List<TicksData> CsvToTicks(String filepath)
    {
        List<TicksData> TicksDatas = new List<TicksData>();

        // http://dotnet-tutorials.net/Article/read-a-csv-file-in-csharp
        string[] lines = System.IO.File.ReadAllLines(filepath);
        foreach (string line in lines)
        {
            // check if the line is not a header
            if (!Regex.IsMatch(line, @"^[a-zA-Z,]+$"))
            {

                string[] columns = line.Split(',');
                TicksData data = new TicksData(
                    DateTime.ParseExact(columns[0], "mm:ss.f", null),
                    Int32.Parse(columns[2]),
                    decimal.Parse(columns[3], CultureInfo.InvariantCulture));
                TicksDatas.Add(data);
            }
        }

        return TicksDatas;
    }


}
public class OHCL_Data
{
    private DateTime time { get; set; }
    private decimal open { get; set; }
    private decimal high { get; set; }
    private decimal close { get; set; }
    private decimal low { get; set; }

    public OHCL_Data()
    {
        time = DateTime.MinValue; open = -1; high = -1; close = -1; low = -1;
    }
    public OHCL_Data(DateTime time, decimal open, decimal high, decimal close, decimal low)
    {
        this.time = time;
        this.open = open;
        this.high = high;
        this.close = close;
        this.low = low;
    }

    public OHCL_Data(DateTime time, decimal open)
    {
        this.time = time;
        this.open = open;
        this.high = open;
        this.close = -1;
        this.low = open;
    }
    // Transform ticks (List<TicksData>) into an OHCL of period (milliseconds)
    public static List<OHCL_Data> TicksToOHCL(List<TicksData> ticks, long period)
    {
        List<OHCL_Data> oHCL_Datas = new List<OHCL_Data>();

        DateTime firstPeriod = ticks[0].time;
        TimeSpan timeScale = TimeSpan.FromMilliseconds(period);

        // group ticks by period of time
        // https://stackoverflow.com/questions/73080797/c-sharp-tick-by-tick-stock-data-to-ohlc-candles-resample-on-different-timeframe
        var groupedTicksData = ticks
            .GroupBy(ticksData => (ticksData.time - firstPeriod).Ticks / timeScale.Ticks)
            .Select(group => new
            {
                StartTime = firstPeriod.AddTicks(group.Key * timeScale.Ticks),
                EndTime = firstPeriod.AddTicks((group.Key + 1) * timeScale.Ticks - 1),
                OpenPrice = group.First().price,
                ClosePrice = group.Last().price,
                HighPrice = group.Max(ticksData => ticksData.price),
                LowPrice = group.Min(ticksData => ticksData.price)

            });

        // Print the grouped TicksData
        foreach (var group in groupedTicksData)
        {
            oHCL_Datas.Add(new OHCL_Data(group.StartTime, group.OpenPrice, group.HighPrice, group.ClosePrice, group.LowPrice));
        }

        return oHCL_Datas;
    }

    override
    public String ToString()
    {
        return time + "," + open + "," + high + "," + close + "," + low;
    }

    public static void PrintOHCLList(List<OHCL_Data> list)
    {
        Console.WriteLine(list.Count);

        foreach (OHCL_Data d in list)
        {
            Console.WriteLine(d.ToString());
        }
    }

    /*public static void Main(string[] args)
    {
        // console testing
        String path = "../../../tradesoft-ticks-sample.csv";
        List<TicksData> list = TicksData.csvToTicks(path);
        List<OHCL_Data> oHCL_Datas = OHCL_Data.TicksToOHCL(list, 1000 * 60 * 15);
        OHCL_Data.PrintOHCLList(oHCL_Datas);
    }*/
}


