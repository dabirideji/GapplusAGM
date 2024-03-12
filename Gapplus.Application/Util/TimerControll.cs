using BarcodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class TimerControll 
    {
        //public int time;
        //public static Boolean Running;
        //public static TimeSpan timeElapsed;
        //public static Stopwatch stopwatch;
        public static Dictionary<string ,Stopwatch> TimerStore = new Dictionary<string ,Stopwatch>();
        public static Dictionary<string, int> CountStore = new Dictionary<string, int>();

 

        public TimerControll()
        {
            
        }


        public static async Task<bool> setTime(string company,int agmid ,int CountTime)
        {

            //public static Boolean Running;
            //public static TimeSpan timeElapsed;
            Stopwatch stopwatch;
            int countTime ;
            var companyinfo = company.ToUpper();
            //check if stopwatch exist
            if (CountStore.TryGetValue(companyinfo, out countTime))
            {

                CountStore[companyinfo] = CountTime + 1;
               

            }
            else
            {
                countTime = CountTime + 1;
                CountStore.Add(companyinfo, countTime);

            }

            //check if stopwatch exist
            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {             
                stopwatch.Start();
                //await GetTimeAsync(company, agmid);
                return true;
            }
            else
            {
                stopwatch = new Stopwatch();
                TimerStore.Add(companyinfo, stopwatch);
                TimerStore[companyinfo].Start();
                //await GetTimeAsync(company, agmid);
                return true;
            }



            //if (stopwatch.Elapsed.Minutes == time.Minutes)
            //{
            //    stopwatch.Stop();
            //}
        }

        public static void ClearTimerStore()
        {
            TimerStore.Clear();
            CountStore.Clear();
        }

        public static void stopTime(string company,int agmid)
        {
            //public static Boolean Running;
            //public static TimeSpan timeElapsed;
             Stopwatch stopwatch;
            int countTime;
            var companyinfo = company.ToUpper();
            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {
                stopwatch.Stop();
                if (CountStore.TryGetValue(companyinfo, out countTime))
                {
                    CountStore[companyinfo] = 0;
                    stopwatch.Stop();

                }
                //Functions.StopTimer(companyinfo);
                //var timeElapsed = new TimeSpan();
                //Functions.StopTimer(companyinfo);
                //var interval = new TimeSpan(timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);
                //Functions.TimerDisplay(agmid, companyinfo, string.Format("{0}", interval.ToString("c")));

            }

            //stopwatch.Stop();

        }
        public static void ResetTime(string company,int agmid)
        {
            Stopwatch stopwatch;
            int countTime;
            var companyinfo = company.ToUpper();
            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {

                stopwatch.Reset();
                if (CountStore.TryGetValue(companyinfo, out countTime))
                {
                    CountStore[companyinfo] = 0;
                    stopwatch.Reset();
                    //var timeElapsed = new TimeSpan();
                    //Functions.StopTimer(companyinfo);
                    //var interval = new TimeSpan(timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);
                    //Functions.TimerDisplay(agmid, companyinfo, string.Format("{0}", interval.ToString("c")));

                }
                //GetTimeAsync(company, agmid);
            }

            //TimerStore[companyinfo].Reset();

        }

        public static bool GetTimeStatus(string company)
        {
            bool Running = false;
            var companyinfo = company.ToUpper();
            //public static TimeSpan timeElapsed;
            Stopwatch stopwatch;
            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {
                Running = stopwatch.IsRunning;
            }
                //Running = TimerStore[companyinfo].IsRunning;
            return Running;
        }

        public static TimeSpan GetTime(string company)
        {
            TimeSpan timeElapsed = new TimeSpan();
            Stopwatch stopwatch;
            int countTime;
            var companyinfo = company.ToUpper();
            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {
                if (CountStore.TryGetValue(companyinfo, out countTime))
                {
                    if(!((TimeSpan.FromSeconds(countTime) - TimerStore[companyinfo].Elapsed) <= TimeSpan.FromSeconds(0)) && stopwatch.IsRunning)
                    {

                            timeElapsed = TimeSpan.FromSeconds(countTime) - TimerStore[companyinfo].Elapsed;
                            //interval = new TimeSpan(timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);
                            //Functions.TimerDisplay(id, companyinfo, string.Format("{0}", interval.ToString("c")));
                            //Thread.Sleep(1000);

                    }
                    else
                    {
                        countTime = 0;
                        stopwatch.Stop();
                        CountStore[companyinfo] = 0;
                        timeElapsed = new TimeSpan();
                        Functions.StopTimer(companyinfo);
                    }



                    //interval = new TimeSpan(timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);
                    //Functions.TimerDisplay(id, companyinfo, string.Format("{0}", interval.ToString("c")));
                }
            }
            //if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            //{

            //    if (CountStore.TryGetValue(companyinfo, out countTime))
            //    {
            //        if ((TimeSpan.FromSeconds(countTime) - TimerStore[companyinfo].Elapsed) <= TimeSpan.FromSeconds(0))
            //        {
            //            stopwatch.Stop();

            //            CountStore[companyinfo] = 0;


            //            timeElapsed = new TimeSpan();
            //            Functions.StopTimer(companyinfo);
            //        }
            //        else
            //        {
            //            timeElapsed = TimeSpan.FromSeconds(countTime) - TimerStore[companyinfo].Elapsed;
            //        }
            //    }
            //}

            return timeElapsed;
        }

        private static Task<TimeSpan> GetTimeAsync(string company, int id)
        {
            TimeSpan timeElapsed = new TimeSpan();
            Stopwatch stopwatch;
            TimeSpan interval;
            int countTime;
            var companyinfo = company.ToUpper();

            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {
                if (CountStore.TryGetValue(companyinfo, out countTime))
                {
                    while (!((TimeSpan.FromSeconds(countTime) - TimerStore[companyinfo].Elapsed) <= TimeSpan.FromSeconds(0)))
                    {
                        if (stopwatch.IsRunning)
                        {
                            timeElapsed = TimeSpan.FromSeconds(countTime) - TimerStore[companyinfo].Elapsed;
                            interval = new TimeSpan(timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);
                            Functions.TimerDisplay(id, companyinfo, string.Format("{0}", interval.ToString("c")));
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            countTime = 0;

                        }
                    }
                    stopwatch.Stop();

                    CountStore[companyinfo] = 0;
                    timeElapsed = new TimeSpan();
                    Functions.StopTimer(companyinfo);
                     interval = new TimeSpan(timeElapsed.Hours, timeElapsed.Minutes, timeElapsed.Seconds);
                    Functions.TimerDisplay(id, companyinfo, string.Format("{0}", interval.ToString("c")));
                }
            }
            //timeElapsed = TimerStore[companyinfo].Elapsed;
            return Task.FromResult(timeElapsed);
        }



        public static bool GetCompanyExist(string company)
        {
            Stopwatch stopwatch;
            var companyinfo = company.ToUpper();
            if (TimerStore.TryGetValue(companyinfo, out stopwatch))
            {
                return true;
            }
            return false;
        }
    }
}