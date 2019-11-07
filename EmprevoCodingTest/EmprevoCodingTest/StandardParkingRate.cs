using System;

namespace EmprevoCodingTest
{
    class StandardParkingRate : IParkingRate
    {
        public string Name => "Standard";

        public decimal CalculateParkingRate(DateTime entryDateTime, DateTime exitDateTime)
        {
            var totalHours = Math.Ceiling(exitDateTime.Subtract(entryDateTime).TotalHours);

            if (totalHours <= 3)
            {
                return 5m * (decimal)totalHours;
            }

            var totalCalendarDays = Math.Floor(exitDateTime.Date.Subtract(entryDateTime.Date).TotalDays) + 1;

            return 20m * (decimal)totalCalendarDays;
        }

        public bool DoesRateApply(DateTime entryDateTime, DateTime exitDateTime)
        {
            return true;
        }
    }
}