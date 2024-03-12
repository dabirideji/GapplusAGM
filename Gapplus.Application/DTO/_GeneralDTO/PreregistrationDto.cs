using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class PreregistrationDto
    {
        public int Id { get; set; }
        public string SN { get; set; }
        [Required(ErrorMessage = "Company Name is required")]
        public string Company { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public float Holding { get; set; }
        public string FormattedHolding => string.Format("{0:0,0}", Holding);
        public string UnFormattedHolding => string.Format("{0:0}", Holding);
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }
        public decimal? PercentageHolding { get; set; }
        public string ShareholderNum { get; set; }
        public byte[] BarcodeImage { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string emailAddress { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }
        public string passwordToken { get; set; }
        public bool AccountVerified { get; set; }
        public bool IsLoggedIn { get; set; }
        public string OTP { get; set; }
        public bool IsValidOTP { get; set; }
        public string DeviceId { get; set; }
        public string Date { get; set; }
        public DateTime DateCreated { get; set; }
        public string CHN { get; set; }
        public string BVN { get; set; }
        public string BankAccountNo { get; set; }
        public string BankDescription { get; set; }
        public string State { get; set; }
        public decimal Price { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }


    public class PreRegistrationResponse
    {
        public string Status { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        //public BarcodeModelDto barcodemodeldto { get; set; }

    }
}