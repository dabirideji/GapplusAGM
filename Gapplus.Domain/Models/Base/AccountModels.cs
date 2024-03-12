using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gapplus.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarcodeGenerator.Models
{
    public class UsersContext : DbContext
    {
        private readonly DbContextOptions<UsersContext> options;


        public UsersContext(DbContextOptions<UsersContext> options) : base(options)
        {
        }

        ///////////////////////////////////////////////////////
        //  I RECENTLY JUST ADDED THE MODELS
        // public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }

        ///////////////////////////////////////////////////////
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<BarcodeModel> BarcodeStore { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Result> Result { get; set; }
        public DbSet<SettingsModel> Settings { get; set; }
        public DbSet<PresentModel> Present { get; set; }
        public DbSet<ProxyModel> Proxy { get; set; }
        public DbSet<MailSettings> mailsettings { get; set; }
        public DbSet<UploadDatabase> UploadDatabase { get; set; }
        public DbSet<ShareholderFeedback> ShareholderFeedback { get; set; }
        public DbSet<Messages> Messages { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<SMSDeliveryLog> SMSDeliveryLog { get; set; }
        public DbSet<KeypadResults> KeypadResults { get; set; }
        public DbSet<Proxylist> ProxyList { get; set; }
        public DbSet<PresentArchive> PresentArchive { get; set; }
        public DbSet<QuestionArchive> QuestionArchive { get; set; }
        public DbSet<ResultArchive> ResultArchive { get; set; }
        public DbSet<AppLog> AppLogs { get; set; }

        //public DbSet<APIMessageLog> APIMessageLogs { get; set; }
        public DbSet<AGMQuestion> AGMQuestions { get; set; }
        public object SettingsModels { get; internal set; }
        public DbSet<Facilitators> Facilitators { get; set; }
        public DbSet<FacilitatorsArchive> FacilitatorsArchive { get; set; }
        public DbSet<SettingsModelArchive> SettingsArchive { get; set; }
    }












    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }



        public string Identity { get; set; }
        public string EmailId { get; set; }
        public string CompanyInfo { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }






    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(
            100,
            ErrorMessage = "The {0} must be at least {2} characters long.",
            MinimumLength = 6
        )]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare(
            "NewPassword",
            ErrorMessage = "The new password and confirmation password do not match."
        )]
        public string ConfirmPassword { get; set; }
    }








    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[Required]
        //[DataType(DataType.Text)]
        //[Display(Name = "Login Company")]
        //public string CompanyInfo { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }













    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(
            100,
            ErrorMessage = "The {0} must be at least {2} characters long.",
            MinimumLength = 6
        )]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Identity { get; set; }
        public string EmailId { get; set; }
    }











    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }













    public class ExternalChange
    {
        public string rt { get; set; }
    }
}
