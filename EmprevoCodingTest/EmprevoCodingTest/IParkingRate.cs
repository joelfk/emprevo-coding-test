using System;

namespace EmprevoCodingTest
{
    interface IParkingRate
    {
        string Name { get; }

        bool DoesRateApply(DateTime entryDateTime, DateTime exitDateTime);

        decimal CalculateParkingRate(DateTime entryDateTime, DateTime exitDateTime);
    }
}