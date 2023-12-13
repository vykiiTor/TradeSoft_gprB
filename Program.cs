// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    public static List<string> listA = new List<string>();
    public static List<string> listB = new List<string>();

    static void Main(string[] args)
    {
        Console.WriteLine("Hello this a test from Antonin");
        using (var reader = new StreamReader("/Users/victortran/TradeSoft_gprB-1/tradesoft-ticks-sample.csv"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                listA.Add(line);
                listB.Add(values[1]);
            }
        }
        foreach(string l in listA){
            Console.WriteLine(l);
        }
    }
}
