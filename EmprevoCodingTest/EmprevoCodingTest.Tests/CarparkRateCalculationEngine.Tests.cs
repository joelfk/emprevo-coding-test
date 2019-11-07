using NUnit.Framework;
using System;
using System.IO;

namespace EmprevoCodingTest.Tests
{
    class When_The_Carpark_Rate_Calculation_Engine_Is_Called
    {
        class And_Neither_The_Entry_Date_And_Time_Or_The_Exit_Date_And_Time_Is_Given
        {
            [Test]
            public void Then_An_Error_Message_Should_Be_Displayed()
            {
                using (var stringWriter = new StringWriter())
                {
                    Console.SetOut(stringWriter);

                    CarparkRateCalculationEngine.Main(null);

                    var output = stringWriter.ToString();

                    Assert.AreEqual($"Please provide the entry date and time as the first argument and the exit date and time as the second argument.{Environment.NewLine}", output);
                }
            }
        }

        class And_Only_The_Entry_Date_And_Time_Is_Given
        {
            [Test]
            public void Then_An_Error_Message_Should_Be_Displayed()
            {
                using (var stringWriter = new StringWriter())
                {
                    Console.SetOut(stringWriter);

                    CarparkRateCalculationEngine.Main(new string[] { "argument1" });

                    var output = stringWriter.ToString();

                    Assert.AreEqual($"Please provide the exit date and time as the second argument.{Environment.NewLine}", output);
                }
            }
        }

        class And_Both_The_Entry_Date_And_Time_And_The_Exit_Date_And_Time_Is_Given
        {
            class And_The_Entry_Date_And_Time_Is_Not_A_Valid_DateTime
            {
                [Test]
                public void Then_An_Error_Message_Should_Be_Displayed()
                {
                    using (var stringWriter = new StringWriter())
                    {
                        Console.SetOut(stringWriter);

                        CarparkRateCalculationEngine.Main(new string[] { "InvalidDate", DateTime.Now.ToString() });

                        var output = stringWriter.ToString();

                        Assert.AreEqual($"The entry date and time is not valid.{Environment.NewLine}", output);
                    }
                }
            }

            class And_The_Entry_Date_And_Time_Input_Is_A_Valid_DateTime
            {
                class And_The_Exit_Date_And_Time_Is_Not_A_Valid_DateTime
                {
                    [Test]
                    public void Then_An_Error_Message_Should_Be_Displayed()
                    {
                        using (var stringWriter = new StringWriter())
                        {
                            Console.SetOut(stringWriter);

                            CarparkRateCalculationEngine.Main(new string[] { DateTime.Now.ToString(), "InvalidDate" });

                            var output = stringWriter.ToString();

                            Assert.AreEqual($"The exit date and time is not valid.{Environment.NewLine}", output);
                        }
                    }
                }

                class And_The_Exit_Date_And_Time_Input_Is_A_Valid_DateTime
                {
                    class And_The_Exit_Date_And_Time_Is_Earlier_Than_The_Entry_Date_And_Time
                    {
                        [Test]
                        public void Then_An_Error_Message_Should_Be_Displayed()
                        {
                            var now = DateTime.Now;

                            using (var stringWriter = new StringWriter())
                            {
                                Console.SetOut(stringWriter);

                                CarparkRateCalculationEngine.Main(new string[] { now.ToString(), now.AddMinutes(-1).ToString() });

                                var output = stringWriter.ToString();

                                Assert.AreEqual($"The entry date and time should be earlier than the exit date and time.{Environment.NewLine}", output);
                            }
                        }
                    }

                    class And_The_Entry_Date_And_Time_Is_Equal_To_Or_Earlier_Than_The_Exit_Date_And_Time
                    {
                        class And_The_Entry_Date_And_Time_Is_Between_6PM_And_Midnight_On_A_Weekday_And_The_Exit_Date_And_Time_Is_On_The_Following_Day_Before_6AM
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Night_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(new Random().Next(18, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddDays(1).AddHours(new Random().Next(0, 6)).AddMinutes(new Random().Next(0, 60));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Night Rate. Price: $6.50.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_6PM_And_Midnight_On_A_Weekday_And_The_Exit_Date_And_Time_Is_Before_Midnight_On_The_Same_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Night_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(new Random().Next(18, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = entryDateTime.AddMinutes(new Random().Next(0, (int)entryDateTime.AddDays(1).Date.Subtract(entryDateTime).TotalMinutes));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Night Rate. Price: $6.50.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_Midnight_Saturday_And_Midnight_Sunday_And_The_Exit_Date_And_Time_Is_Between_Midnight_Sunday_And_Midnight_Monday
                        {
                            [Test]
                            public void Then_The_Weekend_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Saturday).AddHours(new Random().Next(0, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Saturday).AddDays(1).AddHours(new Random().Next(0, 24)).AddMinutes(new Random().Next(0, 60));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Weekend Rate. Price: $10.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_Midnight_Saturday_And_Midnight_Sunday_And_The_Exit_Date_And_Time_Is_Between_Midnight_Saturday_And_Midnight_Sunday
                        {
                            [Test]
                            public void Then_The_Weekend_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Saturday).AddHours(new Random().Next(0, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = entryDateTime.AddMinutes(new Random().Next(0, (int)entryDateTime.AddDays(1).Date.Subtract(entryDateTime).TotalMinutes));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Weekend Rate. Price: $10.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_Midnight_Sunday_And_Midnight_Monday_And_The_Exit_Date_And_Time_Is_Between_Midnight_Sunday_And_Midnight_Monday
                        {
                            [Test]
                            public void Then_The_Weekend_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Sunday).AddHours(new Random().Next(0, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = entryDateTime.AddMinutes(new Random().Next(0, (int)entryDateTime.AddDays(1).Date.Subtract(entryDateTime).TotalMinutes));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Weekend Rate. Price: $10.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_6AM_And_9AM_On_A_Weekday_And_The_Exit_Date_And_Time_Is_Between_3_30PM_And_11_30PM_On_The_Same_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Early_Bird_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(new Random().Next(6, 9)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(new Random().Next(15, 23)).AddMinutes(new Random().Next(0, 60)).AddMinutes(30);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Early Bird Rate. Price: $13.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_6PM_And_Midnight_On_A_Weekday_And_The_Exit_Date_And_Time_Is_Two_Days_Later_Before_6AM
                        {
                            [Test]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Monday).AddHours(new Random().Next(18, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Monday).AddDays(2).AddHours(new Random().Next(0, 6)).AddMinutes(new Random().Next(0, 60));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $60.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_Midnight_Saturday_And_Midnight_Sunday_And_The_Exit_Date_And_Time_Is_The_Next_Weekend_Between_Midnight_Sunday_And_Midnight_Monday
                        {
                            [Test]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Saturday).AddHours(new Random().Next(0, 24)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Saturday).AddDays(8).AddHours(new Random().Next(0, 24)).AddMinutes(new Random().Next(0, 60));

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $180.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_Between_6AM_And_9AM_On_A_Weekday_And_The_Exit_Date_And_Time_Is_Between_3_30PM_And_11_30PM_On_The_Next_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(new Random().Next(6, 9)).AddMinutes(new Random().Next(0, 60));
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddDays(1).AddHours(new Random().Next(15, 23)).AddMinutes(new Random().Next(0, 60)).AddMinutes(30);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $40.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_8_30AM_On_A_Weekday_And_The_Exit_Date_And_Time_Is_9_15AM_On_The_Same_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(8).AddMinutes(30);
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(9).AddMinutes(15);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $5.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_8_30AM_On_A_Weekday_And_The_Exit_Date_And_Time_Is_10_15AM_On_The_Same_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(8).AddMinutes(30);
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(10).AddMinutes(15);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $10.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_8_30AM_On_A_Weekday_And_The_Exit_Date_And_Time_Is_11_15AM_On_The_Same_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(8).AddMinutes(30);
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(11).AddMinutes(15);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $15.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_8_30AM_On_A_Weekday_And_The_Exit_Date_And_Time_Is_1_15PM_On_The_Same_Day
                        {
                            [Test]
                            [TestCase(DayOfWeek.Monday)]
                            [TestCase(DayOfWeek.Tuesday)]
                            [TestCase(DayOfWeek.Wednesday)]
                            [TestCase(DayOfWeek.Thursday)]
                            [TestCase(DayOfWeek.Friday)]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed(DayOfWeek dayOfWeek)
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(8).AddMinutes(30);
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(dayOfWeek).AddHours(13).AddMinutes(15);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $20.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_Is_11_30PM_On_A_Sunday_And_The_Exit_Date_And_Time_Is_1_15AM_On_The_Next_Day
                        {
                            [Test]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Sunday).AddHours(23).AddMinutes(30);
                                var exitDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Sunday).AddDays(1).AddHours(1).AddMinutes(15);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), exitDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $10.00.{Environment.NewLine}", output);
                                }
                            }
                        }

                        class And_The_Entry_Date_And_Time_And_The_Exit_Date_And_Time_Are_The_Same
                        {
                            [Test]
                            public void Then_The_Standard_Rate_Name_And_Price_Should_Be_Displayed()
                            {
                                var entryDateTime = TestUtilities.MapDayOfWeekToDate(DayOfWeek.Monday).AddHours(11).AddMinutes(30);

                                using (var stringWriter = new StringWriter())
                                {
                                    Console.SetOut(stringWriter);

                                    CarparkRateCalculationEngine.Main(new string[] { entryDateTime.ToString(), entryDateTime.ToString() });

                                    var output = stringWriter.ToString();

                                    Assert.AreEqual($"Rate: Standard Rate. Price: $5.00.{Environment.NewLine}", output);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    static class TestUtilities
    {
        internal static DateTime MapDayOfWeekToDate(DayOfWeek dayOfWeek)
        {
            return new DateTime(2019, 11, 10 + ((int)dayOfWeek)).Date;
        }
    }
}