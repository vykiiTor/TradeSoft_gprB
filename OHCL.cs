using System;
using System.Data.Common;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
    public String ToString()
    {
        return Time + "," + Quantity + "," + Price;
    }

    public static void Print_Ticks_List(List<Ticks_Data> list)
    {
        foreach (Ticks_Data t in list)
        {
            Console.WriteLine(t.ToString());
        }
    }

    // transform a csv file (with ',' separator) into a List<Ticks_Data>
    public static List<Ticks_Data> csv_To_Ticks(String filepath)
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

    public OHCL_Data()
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
    public static List<OHCL_Data> Ticks_To_OHCL(List<Ticks_Data> ticks, long period)
    {
        List<OHCL_Data> oHCL_Datas = new List<OHCL_Data>();

        DateTime firstPeriod = ticks[0].Time;
        TimeSpan timeScale = TimeSpan.FromMilliseconds(period);

        // group ticks by period of time
        var groupedTicksData = ticks
            .GroupBy(ticksData => (ticksData.Time - firstPeriod).Ticks / timeScale.Ticks)
            .Select(group => new
            {
                StartTime = firstPeriod.AddTicks(group.Key * timeScale.Ticks),
                EndTime = firstPeriod.AddTicks((group.Key + 1) * timeScale.Ticks - 1),
                OpenPrice = group.First().Price,
                ClosePrice = group.Last().Price,
                HighPrice = group.Max(ticksData => ticksData.Price),
                LowPrice = group.Min(ticksData => ticksData.Price)

            });

        // Print the grouped Ticks_Data
        foreach (var group in groupedTicksData)
        {
            oHCL_Datas.Add(new OHCL_Data(group.StartTime, group.OpenPrice, group.HighPrice, group.ClosePrice, group.LowPrice));
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

    /*public static void Main(string[] args)
    {
        // console testing
        String path = "../../../tradesoft-ticks-sample.csv";
        List<Ticks_Data> list = Ticks_Data.csv_To_Ticks(path);
        List<OHCL_Data> oHCL_Datas = OHCL_Data.Ticks_To_OHCL(list, 1000 * 60 * 15);
        OHCL_Data.Print_OHCL_List(oHCL_Datas);
    }*/
}


