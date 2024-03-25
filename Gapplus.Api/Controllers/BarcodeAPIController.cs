using BarcodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;


namespace BarcodeGenerator.Controllers
{
    // [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BarcodeAPIController : ControllerBase
    {
        UsersContext db ;
        public BarcodeAPIController(UsersContext _db)
        {
            db=_db;
        }
        
        // GET api/barcodeapi
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //GET api/barcodeapi/5
        [HttpGet("GetBarcode/{id}")]
        public HttpResponseMessage GetBarcode(string id)
        {
            HttpResponseMessage response;
            var shareholdernum = Int64.Parse(id);
            var data = db.BarcodeStore.SingleOrDefault(b => b.ShareholderNum == shareholdernum);
            if(data!=null)
            {
                byte[] imgData = data.BarcodeImage;
                MemoryStream ms = new MemoryStream(imgData);
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                return response;
            }
            response = new HttpResponseMessage(HttpStatusCode.NotFound);
            return response;
        }

        //POST api/barcodeapi
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ReturnValue post)
        {
            HttpResponseMessage response;
            var identity = Int64.Parse(post.Identity);
            var data = db.BarcodeStore.SingleOrDefault(b => b.ShareholderNum == identity);
            if(data!=null)
            {
                byte[] imgData = data.BarcodeImage;
                MemoryStream ms = new MemoryStream(imgData);
                response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);

                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

                return response;
            }
            response = new HttpResponseMessage(HttpStatusCode.OK);
            return response;
        }

        // PUT api/barcodeapi/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/barcodeapi/5
        //public void Delete(int id)
        //{
        //}
    }
}
