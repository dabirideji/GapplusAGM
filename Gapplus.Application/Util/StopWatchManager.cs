using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class StopWatchManager
    {
        private static Dictionary<string, Stopwatch> EventTimeLocator = new Dictionary<string, Stopwatch>();
        public int time;
        private string TimerDescriptor;
        public static Boolean Running;
        public static TimeSpan timeElapsed;
        public static Stopwatch stopwatch = new Stopwatch();

        public StopWatchManager(string stopwatchDescriptor)
        {
            this.TimerDescriptor = stopwatchDescriptor;
            if (!EventTimeLocator.ContainsKey(TimerDescriptor))
            {
                EventTimeLocator.Add(TimerDescriptor, stopwatch);
            }
        }


        public static Stopwatch GetEventStopWatch(string descriptor)
        {
            Stopwatch stopwatch = EventTimeLocator[descriptor];

            return stopwatch;
        }
    }


    public class EventTimerControl
    {
        public int time;
        public static Boolean Running;
        public static TimeSpan timeElapsed;



        public static void setTime(string descriptor)
        {

            var stopwatch = StopWatchManager.GetEventStopWatch(descriptor);
            stopwatch.Start();
            //if (stopwatch.Elapsed.Minutes == time.Minutes)
            //{
            //    stopwatch.Stop();
            //}
        }

        public static void stopTime(string descriptor)
        {
            var stopwatch = StopWatchManager.GetEventStopWatch(descriptor);
            stopwatch.Stop();

        }
        public static void ResetTime(string descriptor)
        {
            var stopwatch = StopWatchManager.GetEventStopWatch(descriptor);
            stopwatch.Reset();

        }

        public static Boolean GetTimeStatus(string descriptor)
        {
            var stopwatch = StopWatchManager.GetEventStopWatch(descriptor);
            Running = stopwatch.IsRunning;
            return Running;
        }

        public static TimeSpan GetTime(string descriptor)
        {
            var stopwatch = StopWatchManager.GetEventStopWatch(descriptor);
            timeElapsed = stopwatch.Elapsed;
            return timeElapsed;
        }
    }
}