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

//INJECTING AUTOMAPPER
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Console.WriteLine("Start");
Console.WriteLine(DatabaseManager.GetConnectionString());
Console.WriteLine("End");

builder.Services.AddSignalR();

// builder.Services.AddScoped<IConfiguration>();
//REGISTERING THE DB CONTEXT AND THE DATAABASES 

//ONLINE SQL SERVER DATABASE
// builder.Services.AddDbContext<GapplusDbContext>(options =>
// options.UseSqlServer(builder.Configuration.GetConnectionString("Online"),
// b => b.MigrationsAssembly("Gapplus.Api")
// ));


//OFFLINE SQLITE DATABASE
// builder.Services.AddDbContext<GapplusDbContext>(options =>
//     options.UseSqlite(builder.Configuration.GetConnectionString("Offline"),
// b => b.MigrationsAssembly("Gapplus.Api")));


builder.Services.AddDbContext<UsersContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"),
b => b.MigrationsAssembly("Gapplus.Api")));



//REGISTER SERVICE
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICompanyContract, CompanyContract>();
builder.Services.AddScoped<IUserContract, UserContract>();
builder.Services.AddScoped<IClikapadContract, ClikapadContract>();




// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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



//REGISTERING CORS FOR COMPATIBILITY AND EASE IN CONSUMING

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});




builder.Services.AddScoped<IUserAdmin,UserAdmin>();
builder.Services.AddScoped<IAGMManager,AGMManager>();
builder.Services.AddScoped<IBarcodeContract,BarcodeContract>();





var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();

// WebRootHelper.Initialize(app.Services.GetRequiredService<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>());
//STARTING THE APPLICATION
app.Run();
