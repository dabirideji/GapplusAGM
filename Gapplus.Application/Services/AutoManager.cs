using BarcodeGenerator.Models;
using BarcodeGenerator.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BarcodeGenerator.Service
{
    public class AutoManager
    {
        //private  string company;

        private static UsersContext db;
        private static UserAdmin ua;

        public AutoManager(UsersContext _db)
        {
            db = _db;
            ua = new UserAdmin(db);

        }



        public static bool CheckAGMStart(string company)
        {



            var companyinfo = company;
            var currentTime = DateTime.Now;
            //TimeSpan agmCountdown = new TimeSpan();
            if (!string.IsNullOrEmpty(company))
            {
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId != -1)
                {
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent != null)
                    {
                        return AgmEvent.AgmStart;
                    }

                }


            }
            return false;
        }

        public static bool StartAGMEvent(string company)
        {
            var companyinfo = company;
            var currentTime = DateTime.Now;
            UsersContext db = AutoManager.db;
            UserAdmin ua = new UserAdmin(db);
            //TimeSpan agmCountdown = new TimeSpan();
            if (!string.IsNullOrEmpty(company))
            {
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId != -1)
                {
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent.AgmDateTime != null && AgmEvent.AgmStart == false)
                    {
                        if (AgmEvent.AgmDateTime <= currentTime)
                        {
                            AgmEvent.AgmStart = true;

                            Functions.RefreshPages(company, "AGM has started");
                            db.SaveChanges();


                            return true;
                        }
                        //agmCountdown = (TimeSpan)(AgmEvent.AgmDateTime - currentTime);
                    }

                }


            }

            return false;
        }

        public static bool StartAGMAdmittance(string company)
        {
            var companyinfo = company;
            var currentTime = DateTime.Now;
            UsersContext db = AutoManager.db;
            UserAdmin ua = new UserAdmin(db);
            //TimeSpan agmCountdown = new TimeSpan();
            if (!string.IsNullOrEmpty(company))
            {
                var UniqueAGMId = ua.RetrieveAGMUniqueID(company);
                if (UniqueAGMId != -1)
                {
                    var AgmEvent = db.Settings.SingleOrDefault(s => s.AGMID == UniqueAGMId);
                    if (AgmEvent.AdmittanceDateTime != null)
                    {
                        if (AgmEvent.AdmittanceDateTime <= currentTime)
                        {
                            AgmEvent.StartAdmittance = true;

                            Functions.RequestBrowserHeartbeats(company, "AGM admittance starts.");
                            db.SaveChanges();


                            return true;
                        }
                        //agmCountdown = (TimeSpan)(AgmEvent.AgmDateTime - currentTime);
                    }

                }


            }

            return false;
        }
    }
}