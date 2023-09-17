global using Projekat.Data;
global using Microsoft.EntityFrameworkCore;
using Projekat;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>().UseUrls("https://localhost:7293");
            });
}

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.



//builder.Services.AddAuthorization();





//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));





//app.MapControllers();

//app.Run();


