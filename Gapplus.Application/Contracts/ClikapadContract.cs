using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarcodeGenerator.Models;

namespace Gapplus.Application.Contracts
{

    public interface IClikapadContract
    {
        bool UpdateClikapad(PresentModel model, string clikapad);
    }
    public class ClikapadContract:IClikapadContract
    {

        private readonly UsersContext db;

        public ClikapadContract(UsersContext _db)
        {
            db = _db;

        }



        public bool UpdateClikapad(PresentModel model, string clikapad)
        {
            try
            {
                if (model != null)
                {
                    if (!string.IsNullOrEmpty(clikapad))
                    {
                        var checkOTherAccountsExist = db.Present.Where(b => b.Company == model.Company && b.emailAddress == model.emailAddress);
                        if (checkOTherAccountsExist != null)
                        {
                            if (checkOTherAccountsExist.Count() == 1)
                            {
                                checkOTherAccountsExist.First().Clikapad = clikapad;
                                checkOTherAccountsExist.First().GivenClikapad = true;
                                var shareholdernumber = checkOTherAccountsExist.First().ShareholderNum;
                                var company = checkOTherAccountsExist.First().Company;
                                var checkifpresent = db.BarcodeStore.FirstOrDefault(f => f.Company == company && f.ShareholderNum == shareholdernumber);
                                if (checkifpresent != null)
                                {

                                    checkifpresent.Selected = false;
                                    checkifpresent.Clikapad = clikapad;
                                }

                                db.SaveChanges();
                                return true;
                            }
                            else if (checkOTherAccountsExist.Count() > 1)
                            {
                                foreach (var item in checkOTherAccountsExist)
                                {
                                    item.Clikapad = clikapad;
                                    item.GivenClikapad = true;
                                    var shareholdernumber = item.ShareholderNum;
                                    var checkifpresent = db.BarcodeStore.FirstOrDefault(f => f.Company == item.Company && f.ShareholderNum == shareholdernumber);
                                    if (checkifpresent != null)
                                    {

                                        checkifpresent.Selected = false;
                                        checkifpresent.Clikapad = clikapad;


                                    }
                                }
                                db.SaveChanges();
                                return true;
                            }
                            return false;
                        }
                        return false;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                throw;
            }

        }


    }
}