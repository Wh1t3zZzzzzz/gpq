using Microsoft.EntityFrameworkCore;
using OilSystem.Configuration;
using OilBlendSystem.Models.DataBaseModel;
using OilBlendSystem.Models.ConstructModel;
using OilBlendSystem.BLL.Interface;
using OilBlendSystem.BLL.Implementation;

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(new Appsettings(configuration));


var MySqlConnection = Appsettings.App(new string[] { "AppSettings", "MySqlConnection" });
builder.Services.AddDbContext<oilblendContext>(options =>
  options.UseMySql(MySqlConnection, new MySqlServerVersion(new Version(8, 0, 29))));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", opt => opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("X-Pagination"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

    // app.UseSwagger();
    // app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.UseCors("Policy");

app.Run();
