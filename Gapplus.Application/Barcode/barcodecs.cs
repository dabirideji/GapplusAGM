using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;

namespace BarcodeGenerator.Barcode
{
    public class barcodecs
    {
        public string generateBarcode()
        {
            try
            {
                string[] charPool = "1-2-3-4-5-6".Split('-');
                StringBuilder rs = new StringBuilder();
                int length = 7;
                Random rnd = new Random();
                while (length-- > 0)
                {
                    int index = (int)(rnd.NextDouble() * charPool.Length);
                    if (charPool[index] != "-")
                    {
                        rs.Append(charPool[index]);
                        charPool[index] = "-";
                    }
                    else
                        length++;
                }
                
                return rs.ToString();
            }
            catch (Exception ex)
            {
                //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
            }
            return "";
        }

        //31 December 2012 Prapti

        // public Byte[] getBarcodeImage(string barcode, string file)
        // {
        //     try
        //     {
        //         BarCode39 _barcode = new BarCode39();
        //         int barSize = 24;
        //         string fontFile = HttpContext.Current.Server.MapPath("~/fonts/fre3of9x.ttf");
        //         return (_barcode.Code39( "*" + barcode + "*" , barSize, true, file, fontFile));
        //     }
        //     catch (Exception ex)
        //     {
        //         //ErrorLog.WriteErrorLog("Barcode", ex.ToString(), ex.Message);
        //     }
        //     return null;
        // }



    









    }
}
