// using BarcodeGenerator.Models;
// using System;
// using System.Collections.Generic;
// using System.Data;
// using System.Data.Entity;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Web.Http;
// using System.Web.Http.Description;

// namespace BarcodeGenerator.Controllers
// {
//     [ApiExplorerSettings(IgnoreApi = true)]
//     public class PasswordAPIController : ApiController
//     {
//         UsersContext db = new UsersContext();
//         // GET api/passwordapi
//         //[HttpGet]
//         //public IEnumerable<string> Get()
//         //{
//         //    return new string[] { "value1", "value2" };
//         //}

     
//         //// GET api/passwordapi/5
//         //public string Get(int id)
//         //{
//         //    return "value";
//         //}
//         [HttpGet]
//         //[Route("GetHoldings")]
//         // POST api/passwordapi
//         public HttpResponseMessage post([FromBody]PostPasswordModel post)
//         {
//             try
//             {
//                 var shareholder = db.BarcodeStore.SingleOrDefault(u => u.passwordToken == post.Token);
//                 if(shareholder!=null)
//                 {
//                     shareholder.password = post.Password;
//                     db.Entry(shareholder).State = EntityState.Modified;
//                     db.SaveChanges();

//                     HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "Verified OK");

//                     return response;
//                 }        
                
//                 return Request.CreateResponse(HttpStatusCode.BadRequest, "Try Again");

//             }
//             catch (Exception e)
//             {
//                 return Request.CreateResponse(HttpStatusCode.NoContent, "Empty Post");
//             }
//         }


//         //// PUT api/passwordapi/5
//         //public void Put(int id, [FromBody]string value)
//         //{
//         //}

//         //// DELETE api/passwordapi/5
//         //public void Delete(int id)
//         //{
//         //}
//     }
// }
