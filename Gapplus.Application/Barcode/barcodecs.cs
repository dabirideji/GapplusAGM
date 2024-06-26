﻿using System;
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
using Microsoft.AspNetCore.Hosting;

namespace BarcodeGenerator.Barcode
{
    public class barcodecs
    {
        private IWebHostEnvironment _webHostEnvironment;

        public barcodecs(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

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
                    if(length==0){
                return rs.ToString();

                    }
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



   public byte[] getBarcodeImage(string barcode, string fontFileName)
        {
            try
            {
                BarCode39 barcodeGenerator = new BarCode39();
                int barSize = 24;
                string fontFile = Path.Combine(_webHostEnvironment.WebRootPath, "fonts", fontFileName);

                return barcodeGenerator.Code39($"*{barcode}*", barSize, true, "", fontFile);
            }
            catch (Exception ex)
            {
                // Handle the exception or log it
                Console.WriteLine($"Error generating barcode: {ex.Message}");
                return null;
            }
        }







    }
}
