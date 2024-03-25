// using System;
// using System.Threading;
// using System.Web.Mvc;
// using WebMatrix.WebData;
// using BarcodeGenerator.Models;

// namespace BarcodeGenerator.Filters
// {
//     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//     public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
//     {
//         private static SimpleMembershipInitializer _initializer;
//         private static object _initializerLock = new object();
//         private static bool _isInitialized;

//         public override void OnActionExecuting(ActionExecutingContext filterContext)
//         {
//             // Ensure ASP.NET Simple Membership is initialized only once per app start
//             LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
//         }

//         private class SimpleMembershipInitializer
//         {

//             public SimpleMembershipInitializer()
//             {
//                 Database.SetInitializer<UsersContext>(null);

//                 try
//                 {
//                     using (var context = new UsersContext())
//                     { }
//                     if (!context.Database.Exists())
//                     {
//                         // Create the SimpleMembership database without Entity Framework migration schema
//                         ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
//                     }
//                 }

//                     if (!WebSecurity.Initialized)
//                 {
//                     WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
//                 }
//             }
//                 catch (Exception ex)
//                 {
//                     throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
//         }
//     }
// }
// }







// using System;
// using Microsoft.AspNetCore.Mvc.Filters;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.AspNetCore.Identity;
// using BarcodeGenerator.Models;
// using Gapplus.Domain;

// namespace YourNamespace.Filters
// {
//     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
//     public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
//     {
//         private static SimpleMembershipInitializer _initializer;
//         private static object _initializerLock = new object();
//         private static bool _isInitialized;

//         public override void OnActionExecuting(ActionExecutingContext filterContext)
//         {
//             // Ensure ASP.NET Core Identity is initialized only once per app start
//             if (!_isInitialized)
//             {
//                 lock (_initializerLock)
//                 {
//                     if (!_isInitialized)
//                     {
//                         _initializer = new SimpleMembershipInitializer(filterContext.HttpContext.RequestServices);
//                         _isInitialized = true;
//                     }
//                 }
//             }
//         }

//         private class SimpleMembershipInitializer
//         {
//             private readonly IServiceProvider _serviceProvider;

//             public SimpleMembershipInitializer(IServiceProvider serviceProvider)
//             {
//                 _serviceProvider = serviceProvider;
//                 Initialize();
//             }

//             private void Initialize()
//             {
//                 using (var scope = _serviceProvider.CreateScope())
//                 {
//                     var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
//                     var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

//                     // Ensure any pending migrations are applied to the database
//                     var context = scope.ServiceProvider.GetRequiredService<UsersContext>();
//                     context.Database.Migrate();

//                     // Ensure roles are created
//                     string[] roles = { "Admin", "User" };
//                     foreach (var roleName in roles)
//                     {
//                         if (!roleManager.RoleExistsAsync(roleName).Result)
//                         {
//                             roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
//                         }
//                     }

//                     // Ensure an admin user is created
//                     var adminUser = new User { FullName = "admin@example.com", EmailId = "admin@example.com" };
//                     var result = userManager.CreateAsync(adminUser, "Admin@123").Result;
//                     if (result.Succeeded)
//                     {
//                         userManager.AddToRoleAsync(adminUser, "Admin").Wait();
//                     }
//                 }
//             }
//         }
//     }
// }
