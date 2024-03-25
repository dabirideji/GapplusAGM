using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Text;
using System.IO;
using System.Drawing.Imaging;

namespace BarCodeGenerator.Barcode
{
    public class IDAtomationBarCode
    {
        public static string BarcodeImageGenerator(string Code)  
        {  
            byte[] BarCode;  
            string BarCodeImage;
            //byte[] BarCodeImage;
           // Bitmap objBitmap = new Bitmap(Code.Length * 28, 100);
            Bitmap objBitmap = new Bitmap(Code.Length * 40, 150); 
            using(Graphics graphic = Graphics.FromImage(objBitmap))  
            {  
                //Font newFont = new Font("IDAutomationHC39M Free Version", 18, FontStyle.Regular);
                Font newFont = new Font("IDAutomationHC39M Free Version", 20); 
                PointF point = new PointF(2f, 2f);  
                SolidBrush balck = new SolidBrush(Color.Black);  
                SolidBrush white = new SolidBrush(Color.White);  
                graphic.FillRectangle(white, 0, 0, objBitmap.Width, objBitmap.Height);  
                graphic.DrawString("*" + Code + "*", newFont, balck, point);  
            }  
            using(MemoryStream Mmst = new MemoryStream())  
            {  
                objBitmap.Save(Mmst, ImageFormat.Png);  
                BarCode = Mmst.GetBuffer();
                BarCodeImage = BarCode != null ? "data:image/jpg;base64," + Convert.ToBase64String((byte[]) BarCode) : "";
                return BarCodeImage;  
            }  
        }
    }
}