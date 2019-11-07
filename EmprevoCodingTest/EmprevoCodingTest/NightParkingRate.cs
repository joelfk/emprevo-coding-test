using System;
using System.Collections.Generic;

namespace EmprevoCodingTest
{
    class NightParkingRate : IParkingRate
    {
        public string Name => "Night";

        public decimal CalculateParkingRate(DateTime entryDateTime, DateTime exitDateTime)
        {
            return 6.5m;
        }

        public bool DoesRateApply(DateTime entryDateTime, DateTime exitDateTime)
        {
            return new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }.Contains(entryDateTime.DayOfWeek) && entryDateTime.Hour >= 18 && entryDateTime.Hour < 24 && exitDateTime < entryDateTime.AddDays(1).Date.AddHours(6);
        }
    }
}