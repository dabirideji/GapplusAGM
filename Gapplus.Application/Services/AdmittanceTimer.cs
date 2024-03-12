using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Timers;

namespace BarcodeGenerator.Service
{
    public class AdmittanceTimer
    {
        //private static System.Timers.System.Timers.Timer aTimer;
        //private string company;
        private static Dictionary<string, System.Timers.Timer> aTimer = new Dictionary<string, System.Timers.Timer>();


        //public static void Main()
        //{
        //    SetTimer();

        //    //Console.WriteLine("\nPress the Enter key to exit the application...\n");
        //    //Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
        //    //Console.ReadLine();
        //    aTimer.Stop();
        //    aTimer.Dispose();

        //    Console.WriteLine("Terminating the application...");
        //}

        public static void SetTimer(string company)
        {
            System.Timers.Timer timer;
            var companyinfo = company.ToUpper();
            // Create a timer with a two second interval.
            if (aTimer.TryGetValue(companyinfo, out timer))
            {
                timer = new System.Timers.Timer(1000);
                //timer.Elapsed += OnTimedEvent;
                timer.Elapsed += (sender, e) => CheckDateEvent(sender, e, companyinfo);
                timer.AutoReset = true;
                timer.Enabled = true;
            }
            else
            {

                timer = new System.Timers.Timer(1000);
                aTimer.Add(companyinfo, timer);
                // Hook up the Elapsed event for the timer. 
                timer.Elapsed += (sender, e) => CheckDateEvent(sender, e, companyinfo);
                //timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
                timer.Enabled = true;
            }
            //aTimer = new System.Timers.Timer(1000);

        }

        public static void StopTimer(string company)
        {

            System.Timers.Timer timer;
            var companyinfo = company.ToUpper();
            if (aTimer.TryGetValue(companyinfo, out timer))
            {
                timer.Stop();
                timer.Dispose();

            }

        }

        public static bool IsRunning(string company)
        {
            System.Timers.Timer timer;
            var companyinfo = company.ToUpper();
            if (aTimer.TryGetValue(companyinfo, out timer))
            {
                if (timer.Enabled)
                {
                    return true;
                }


            }
            return false;
            //SetTimer(company);

        }


        public static bool CheckExist(string company)
        {
            System.Timers.Timer timer;
            var companyinfo = company.ToUpper();
            if (aTimer.TryGetValue(companyinfo, out timer))
            {

                return true;
            }
            return false;
            //SetTimer(company);
        }

        public static void ClearTimerStore(string company)
        {

            aTimer.Clear();
            //SetTimer(company);

        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            //                  e.SignalTime);
        }

        private static void CheckDateEvent(Object source, ElapsedEventArgs e, string company)
        {
            //AutoManager am = new AutoManager(company);
            var response = AutoManager.StartAGMAdmittance(company);
            if (response)
            {
                StopTimer(company);
                aTimer.Remove(company);
                //aTimer.Clear();
            }
            //Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}",
            //                  e.SignalTime);
        }
    
    }
}