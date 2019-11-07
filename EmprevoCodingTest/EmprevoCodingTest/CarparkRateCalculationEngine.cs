using System;
using System.Linq;

namespace EmprevoCodingTest
{
    public class CarparkRateCalculationEngine
    {
        public static void Main(string[] args)
        {
            if (!(args ?? Enumerable.Empty<string>().ToArray()).Any())
            {
                Console.WriteLine("Please provide the entry date and time as the first argument and the exit date and time as the second argument.");
                return;
            }
            
            if (args.Count() == 1)
            {
                Console.WriteLine("Please provide the exit date and time as the second argument.");
                return;
            }

            if (!DateTime.TryParse(args[0], out var entryDateTime))
            {
                Console.WriteLine("The entry date and time is not valid.");
                return;
            }

            if (!DateTime.TryParse(args[1], out var exitDateTime))
            {
                Console.WriteLine("The exit date and time is not valid.");
                return;
            }

            if (entryDateTime > exitDateTime)
            {
                Console.WriteLine("The entry date and time should be earlier than the exit date and time.");
                return;
            }

            var appliedParkingRate = new ParkingRateFactory().GetParkingRate(entryDateTime, exitDateTime);

            Console.WriteLine($"Rate: {appliedParkingRate.Name} Rate. Price: {appliedParkingRate.CalculateParkingRate(entryDateTime, exitDateTime).ToString("C")}.");
        }
    }
}