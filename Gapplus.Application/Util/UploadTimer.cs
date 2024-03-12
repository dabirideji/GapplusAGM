using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class UploadTimer
    {
        public int time;
        public static Boolean Running;
        public static TimeSpan timeElapsed;
        public static Stopwatch stopwatch = new Stopwatch();

        public static void setTime()
        {
            stopwatch.Start();
            //if (stopwatch.Elapsed.Minutes == time.Minutes)
            //{
            //    stopwatch.Stop();
            //}
        }

        public static void stopTime()
        {
            stopwatch.Stop();

        }
        public static void ResetTime()
        {
            stopwatch.Reset();

        }

        public static Boolean GetTimeStatus()
        {
            Running = stopwatch.IsRunning;
            return Running;
        }

        public static TimeSpan GetTime()
        {
            timeElapsed = stopwatch.Elapsed;
            return timeElapsed;
        }
    }
}