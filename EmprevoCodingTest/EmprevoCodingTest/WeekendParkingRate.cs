using System;
using System.Collections.Generic;

namespace EmprevoCodingTest
{
    class WeekendParkingRate : IParkingRate
    {
        public string Name => "Weekend";

        public decimal CalculateParkingRate(DateTime entryDateTime, DateTime exitDateTime)
        {
            return 10m;
        }

        public bool DoesRateApply(DateTime entryDateTime, DateTime exitDateTime)
        {
            var weekendDays = new List<DayOfWeek> { DayOfWeek.Saturday, DayOfWeek.Sunday };

            return weekendDays.Contains(entryDateTime.DayOfWeek)
                && weekendDays.Contains(exitDateTime.DayOfWeek)
                && exitDateTime.Subtract(entryDateTime).TotalHours <= 48;
        }
    }
}