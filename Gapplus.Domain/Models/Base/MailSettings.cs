using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BarcodeGenerator.Models
{
    public class MailSettings
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public virtual int id { get; set; }
        public virtual string ServerName { get; set; }
        public virtual string smtpHost { get; set; }
        public virtual int smtpPort { get; set; }
        public virtual string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public virtual string SentFrom { get; set; }
    }
}