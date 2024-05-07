using BarcodeGenerator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;

namespace BarcodeGenerator.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PasswordAPIController : ControllerBase
    {
        UsersContext db;

        public PasswordAPIController(UsersContext _db)
        {
            db=_db;
        }

        


        // GET api/passwordapi
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

     
        //// GET api/passwordapi/5
        //public string Get(int id)
        //{
        //    return "value";
        //}


        [HttpGet]
        public ActionResult post([FromBody]PostPasswordModel post)
        {
            try
            {
                var shareholder = db.BarcodeStore.SingleOrDefault(u => u.passwordToken == post.Token);
                if(shareholder!=null)
                {
                    shareholder.password = post.Password;
                    db.Entry(shareholder).State = EntityState.Modified;
                    db.SaveChanges();

                    var response =StatusCode((int)HttpStatusCode.OK, "Verified OK");

                    return response;
                }        
                
                return StatusCode((int)HttpStatusCode.BadRequest, "Try Again");

            }
            catch (Exception e)
            {
                return StatusCode((int)HttpStatusCode.NoContent, "Empty Post");
            }
        }


        //// PUT api/passwordapi/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/passwordapi/5
        //public void Delete(int id)
        //{
        //}
    }
}
