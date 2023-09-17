using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Projekat.Interfaces;
using Projekat.Repository;
using Projekat.Services;
using System.Text;
using Microsoft.OpenApi.Models;
using Projekat.Mapping;
using Microsoft.Extensions.FileProviders;

namespace Projekat
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Projekat", Version = "v1" });
                //Ovo dodajemo kako bi mogli da unesemo token u swagger prilikom testiranja
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type=ReferenceType.SecurityScheme,
                                            Id="Bearer"
                                        }
                                    },
                                    new string[]{}
                                }
                            });
            });

 
            //var provider = services.BuildServiceProvider();
            //var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters //Podesavamo parametre za validaciju pristiglih tokena
                {
                    ValidateIssuer = true, //Validira izdavaoca tokena
                    ValidateAudience = false, //Kazemo da ne validira primaoce tokena
                    ValidateLifetime = true,//Validira trajanje tokena
                    ValidateIssuerSigningKey = true, //validira potpis token, ovo je jako vazno!
                    ValidIssuer = "http://localhost:7293", //odredjujemo koji server je validni izdavalac
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Authentication:SecretKey")))//navodimo privatni kljuc kojim su potpisani nasi tokeni
                };
            });



            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IOrderService, OrderService>();


            services.AddScoped<UserRepository>();
            services.AddScoped<OrderRepository>();
            services.AddScoped<ItemRepository>();

            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();


            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

          

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:3000") // Allow only requests from this origin
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddDbContext<DataContext>(options =>
                                            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseSwagger();

            app.UseSwaggerUI();


            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, Configuration["ImageStoragePath"])),
                RequestPath = "/images" // This is the URL prefix for accessing the images
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
