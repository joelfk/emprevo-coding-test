using System;
using System.Collections.Generic;

namespace EmprevoCodingTest
{
    class ParkingRateFactory
    {
        private IEnumerable<IParkingRate> parkingRates = new List<IParkingRate> { new NightParkingRate(), new WeekendParkingRate(), new EarlyBirdParkingRate(), new StandardParkingRate() };

        internal IParkingRate GetParkingRate(DateTime entryDateTime, DateTime exitDateTime)
        {
            IParkingRate appliedParkingRate = null;

            foreach (var parkingRate in parkingRates)
            {
                if (appliedParkingRate == null)
                {
                    if (parkingRate.DoesRateApply(entryDateTime, exitDateTime))
                    {
                        appliedParkingRate = parkingRate;
                    }
                }
            }

            return appliedParkingRate;
        }
    }
}
