using System;
using System.Collections.Generic;

namespace EmprevoCodingTest
{
    class EarlyBirdParkingRate : IParkingRate
    {
        public string Name => "Early Bird";

        public decimal CalculateParkingRate(DateTime entryDateTime, DateTime exitDateTime)
        {
            return 13m;
        }

        public bool DoesRateApply(DateTime entryDateTime, DateTime exitDateTime)
        {
            var weekdays = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            return weekdays.Contains(entryDateTime.DayOfWeek)
                && entryDateTime.Date == exitDateTime.Date
                && entryDateTime.Hour >= 6
                && entryDateTime.Hour < 9
                && (exitDateTime.Hour * 60 + exitDateTime.Minute) >= 930
                && (exitDateTime.Hour * 60 + exitDateTime.Minute) < 1410;
        }
    }
}