// using BarcodeGenerator.Models;
// using BarcodeGenerator.Service;
// using Newtonsoft.Json;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Web.Http;
// using System.Web.Http.Description;

// namespace BarcodeGenerator.Controllers
// {
//     [ApiExplorerSettings(IgnoreApi = true)]
//     public class VerifyAPIController : ApiController
//     {
//         UsersContext db = new UsersContext();
//         UserAdmin ua = new UserAdmin();
//         //private static string companyinfo = UserAdmin.GetUserCompanyInfo();

//         //private static int RetrieveAGMUniqueID()
//         //{
//         //    UsersContext adb = new UsersContext();
//         //    var AGMID = adb.Settings.Where(s => s.CompanyName == companyinfo).OrderByDescending(ai => ai.AGMID).First().AGMID;

//         //    return AGMID;
//         //}

//         //private static int UniqueAGMId = RetrieveAGMUniqueID();
//         // GET api/verifyapi
//         //public IEnumerable<string> Get()
//         //{
//         //    return new string[] { "value1", "value2" };

//         //}

//         //// GET api/verifyapi/5
//         //public string Get(int id)
//         //{
//         //    return "value";
//         //}



//         //[ApiExplorerSettings(IgnoreApi = true)]
//         // POST api/verifyapi
//         public HttpResponseMessage Post([FromBody]ReturnValue post)
//         {

//             try
//             {
//                 HttpResponseMessage response;
//                 var companyinfo = ua.GetUserCompanyInfo();
//                 if (post.Identity == null || string.IsNullOrEmpty(companyinfo))
//                 {
//                     return response = Request.CreateResponse(HttpStatusCode.BadRequest, "Missing shareholder number or Company information");

//                 }
//                     var voter = db.BarcodeStore.SingleOrDefault(q => q.Company==companyinfo && q.Barcode == post.Identity);

//                 if (voter != null)
//                 {
//                     //var question = db.Question.ToList();
//                     PresentModel model = new PresentModel();
//                     model.Id = voter.Id;
//                     model.Name = voter.Name;
//                     model.Holding = voter.Holding;
//                     model.Address = voter.Address;
//                     model.ShareholderNum = voter.ShareholderNum;
//                     model.PercentageHolding = voter.PercentageHolding;
//                     model.TakePoll = voter.TakePoll;

//                     dynamic collectionWrapper = new
//                     {
//                         myDetails = model,

//                     };
//                     var output = JsonConvert.SerializeObject(collectionWrapper);
//                     response = Request.CreateResponse(HttpStatusCode.OK, "Verified OK");
//                     response.Content = new StringContent(output, System.Text.Encoding.UTF8, "application/json");
//                     return response;

//                 }
//                 return Request.CreateResponse(HttpStatusCode.BadRequest, "Try Again");
//             }
//             catch
//             {
//                 return Request.CreateResponse(HttpStatusCode.NoContent, "Empty Post");
//             }

//         }

//         //// PUT api/verifyapi/5
//         //public void Put(int id, [FromBody]string value)
//         //{
//         //}

//         //// DELETE api/verifyapi/5
//         //public void Delete(int id)
//         //{
//         //}
//     }
// }
