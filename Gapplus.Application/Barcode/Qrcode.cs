using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using ZXing;
using System.Configuration;
using BarcodeGenerator.Service;
using Microsoft.Extensions.DependencyInjection;
using BarcodeGenerator.Models;
using Microsoft.AspNetCore.Hosting;

namespace BarcodeGenerator.Barcode
{
    public class Qrcode
    {


        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UsersContext _context;

        public Qrcode()
        {
             // Create a service collection
        var serviceCollection = new ServiceCollection();

       
        // Build a service provider from the service collection
        var serviceProvider = serviceCollection.BuildServiceProvider();

        // Resolve the required services
        _webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
        _context = serviceProvider.GetRequiredService<UsersContext>();
        }

        //public byte[] GenerateMyQCCode(string QCText, string name)
        //{
        //    var QCwriter = new BarcodeWriter();
        //    QCwriter.Format = BarcodeFormat.QR_CODE;
        //    var result = QCwriter.Write(QCText);
        //    var imageurl = "~/Images/QRC/" + name + ".jpg";
        //    string path = HttpContext.Current.Server.MapPath(imageurl);
        //    var barcodeBitmap = new Bitmap(result);

        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        using (FileStream fs = new FileStream(path,
        //           FileMode.Create, FileAccess.ReadWrite))
        //        {
        //            barcodeBitmap.Save(memory, ImageFormat.Jpeg);
        //            byte[] bytes = memory.ToArray();
        //            fs.Write(bytes, 0, bytes.Length);
        //        }
        //    }
        //    return ImageToByteArray(name);
        //    //imgageQRCode.Visible = true;
        //    //imgageQRCode.ImageUrl = "~/images/MyQRImage.jpg";

        //}

        // public string GenerateMyQCCode(string QCText, string name)
        // {



        //     // Resolve the IWebHostEnvironment service
        //     UserAdmin ua = new UserAdmin(_context);
        //     // var QCwriter = new BarcodeWriter();
        //     var QCwriter = new BarcodeWriter<string>(); //I AM TRYNING SOMETHING NEW , I THINK I INSTALLED A DIFFERENT PACKAGE SO I WILL TRY THIS ONNE OUT 
        //     string namePart = ua.GetAccessCode();
        //     string filename = name + namePart;
        //     QCwriter.Format = BarcodeFormat.QR_CODE;
        //     var result = QCwriter.Write(QCText);
        //     var imageurl = "~/Images/QRC/" + filename + ".jpg";
        //     // string path = HttpContext.Current.Server.MapPath(imageurl);
        //     string path = Path.Combine(_webHostEnvironment.WebRootPath, imageurl);
        //     var barcodeBitmap = new Bitmap(result);

        //     using (MemoryStream memory = new MemoryStream())
        //     {
        //         using (FileStream fs = new FileStream(path,
        //            FileMode.Create, FileAccess.ReadWrite))
        //         {
        //             barcodeBitmap.Save(memory, ImageFormat.Jpeg);
        //             byte[] bytes = memory.ToArray();
        //             fs.Write(bytes, 0, bytes.Length);
        //         }
        //     }
        //     return ImageUrl(filename);
        //     //imgageQRCode.Visible = true;
        //     //imgageQRCode.ImageUrl = "~/images/MyQRImage.jpg";

        // }


        public string GenerateMyQCCode(string QCText, string name)
        {
            UserAdmin ua = new UserAdmin(_context);
            var QCwriter = new BarcodeWriter<Bitmap>(); // Adjust the type based on the actual BarcodeWriter you are using
            string namePart = ua.GetAccessCode();
            string filename = name + namePart;
            QCwriter.Format = BarcodeFormat.QR_CODE;
            var result = QCwriter.Write(QCText);
            var imageRelativePath = $"~/Images/QRC/{filename}.jpg";
            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageRelativePath);
            var barcodeBitmap = result;

            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(imagePath, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);
                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return ImageUrl(filename);
        }





        //HTTP CONTEXT WILL NOT WORK PROPERLY IN .NETCORE
        // public string ImageToBase64(string name)
        // {
        //     string base64String = null;
        //     var imageurl = "~/Images/QRC/" + name + ".jpg";
        //     string path = HttpContext.Current.Server.MapPath(imageurl);
        //     using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
        //     {
        //         using (MemoryStream m = new MemoryStream())
        //         {
        //             image.Save(m, image.RawFormat);
        //             byte[] imageBytes = m.ToArray();
        //             base64String = Convert.ToBase64String(imageBytes);
        //             return base64String;
        //         }
        //     }
        // }

        public string ImageToBase64(string name)
        {
            string base64String = null;
            var imageRelativePath = $"~/Images/QRC/{name}.jpg";
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageRelativePath);

            using (var image = System.Drawing.Image.FromFile(imagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, ImageFormat.Jpeg);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }

            return base64String;
        }



        public string ImageUrl(string name)
        {
            //string base64String = null;
            var imageurl = "/Images/QRC/" + name + ".jpg";
            var baseUri = $"{Convert.ToString(ConfigurationManager.AppSettings["AGMBaseAddress"])}";
            //string path = HttpContext.Current.Server.MapPath(imageurl);
            //using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            //{
            //    using (MemoryStream m = new MemoryStream())
            //    {
            //        image.Save(m, image.RawFormat);
            //        byte[] imageBytes = m.ToArray();
            //        base64String = Convert.ToBase64String(imageBytes);
            //        return base64String;
            //    }
            //}
            return baseUri + imageurl;
        }
        // public byte[] ImageToByteArray(string name)
        // {

        //     var imageurl = "~/Images/QRC/" + name + ".jpg";
        //     string path = HttpContext.Current.Server.MapPath(imageurl);
        //     using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
        //     {
        //         using (MemoryStream m = new MemoryStream())
        //         {
        //             image.Save(m, image.RawFormat);
        //             byte[] imageBytes = m.ToArray();
        //             return imageBytes;
        //         }
        //     }
        // }

        public byte[] ImageToByteArray(string name)
        {
            var imageRelativePath = $"~/Images/QRC/{name}.jpg";
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageRelativePath);

            using (var image = Image.FromFile(imagePath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    return m.ToArray();
                }
            }
        }

        // public byte[] ReadQRCode(string name)
        // {
        //     var imagename = name + ".jpg";
        //     var QCreader = new BarcodeReader();
        //     string QCfilename = Path.Combine(HttpContext.Current.Request.MapPath("~/Images/QRC/"), imagename);
        //     var QCresult = QCreader.Decode(new Bitmap(QCfilename));
        //     var rawQR = QCresult.RawBytes;
        //     if (QCresult != null)
        //     {
        //         var qrcode = "My QR Code: " + QCresult.Text;
        //     }
        //     return rawQR;
        // }



       
public byte[] ReadQRCode(string name)
{
    // var imageName = name + ".jpg";
    // var QCreader = new BarcodeReader<Bitmap>(bitmap => new BitmapLuminanceSource(bitmap));
    // var imagePath = Path.Combine("wwwroot", "Images", "QRC", imageName); // Assuming the image is located in the wwwroot/Images/QRC directory
    // var QCresult = QCreader.Decode(new Bitmap(imagePath));

    // if (QCresult != null)
    // {
    //     var qrcode = "My QR Code: " + QCresult.Text;
    // }

    // return QCresult?.RawBytes; // Return the raw bytes of the decoded QR code
    return new byte[]{};
}













    }
}