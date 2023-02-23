using System.Text;
using Core.Lib.Authentication.Services;
using Core.Lib.Database.Contexts;
using Core.Lib.Database.DbClients;
using Core.Lib.Database.Interfaces;
using Core.Lib.Ioc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Core.WebApi
{
    public class Startup
    {
        public static IConfiguration Configuration { get; set; }
        public static IWebHostEnvironment WebHostEnvironment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = "issuer",
                    ValidAudience = "audience",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes("SecretKey"))
                };
            });
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddSingleton<IRepositoryContext, MongoDbContext>();
            services.AddSingleton<IMongoDbClient, MongoDbClient>();
            services.AddSingleton<UserService>();
            IocContainer.Instance.ServiceProvider = services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (WebHostEnvironment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
