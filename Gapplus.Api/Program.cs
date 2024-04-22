using BarcodeGenerator.Models;
using BarcodeGenerator.Service;
using Gapplus.Application.Contracts;
using Gapplus.Application.Helpers;
using Gapplus.Application.Interfaces;
using Gapplus.Application.Interfaces.Contracts;
using Gapplus.Application.Interfaces.IContracts;
using Gapplus.Application.Services;
using Gapplus.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
var builder = WebApplication.CreateBuilder(args);



#region EXTERNAL LIBRARIES AND PACKAGES
    //INJECTING AUTOMAPPER
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    
#endregion

#region ADDING AND REGISTERING OF SERVICES
//ADDING AND INJECTING SERVICES NEEDED 
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSignalR();

builder.Services.AddDistributedMemoryCache(); // Example: In-memory distributed cache


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(
     options =>
     {
         options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
         {
             In = ParameterLocation.Header,
             Name = "Authorization",
             Type = SecuritySchemeType.ApiKey
         });
         options.OperationFilter<SecurityRequirementsOperationFilter>();
     });


    //  CORS FOR CROSS origin  RESOURCE SHARING
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

#endregion

#region DATABASE CONNECTIONS AND CONFIGURATIONS

//REGISTERING THE DB CONTEXT AND THE DATAABASES 

// ##DEFAULT CONNECTION

// builder.Services.AddDbContext<UsersContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
// b => b.MigrationsAssembly("Gapplus.Api")));



// ##OFFLINE SQLITE DATABASE {Deji test database }
builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Offline"),
b => b.MigrationsAssembly("Gapplus.Api")));




// ##ONLINE SQL SERVER DATABASE
// MAIN DATABASE FOR LIVE UPDATES ANY SYNC BETWEEN DEVELOPERS {ONLINE SQL SERVER DATABASE }

// builder.Services.AddDbContext<UsersContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("Online"),
// b => b.MigrationsAssembly("Gapplus.Api")));

#endregion

#region DEPENDENCY INJECTION AND INTERFACE REGISTERATIONS

//REGISTER SERVICES AND INTERFACES
//UNIT OF WORK. // NOT CURRENTLY IN USE.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICompanyContract, CompanyContract>();
builder.Services.AddScoped<IUserContract, UserContract>();
builder.Services.AddScoped<IClikapadContract, ClikapadContract>();


//VITAL SERVICES
builder.Services.AddScoped<ITempDataManager, TempDataManager>();
builder.Services.AddScoped<IViewBagManager, ViewBagManager>();
builder.Services.AddScoped<ICacheService, CacheService>();


builder.Services.AddScoped<IUserAdmin, UserAdmin>();
builder.Services.AddScoped<IAGMManager, AGMManager>();
builder.Services.AddScoped<IBarcodeContract, BarcodeContract>();

#endregion

var app = builder.Build();

#region BUILD PIPELINE 
//INITIALIZING MY STATIC SESSION MANAGER
SessionInitializer
.Initialize(app
.Services
.GetRequiredService<IHttpContextAccessor>()
);


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors();
app.UseSession();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.Run();

#endregion