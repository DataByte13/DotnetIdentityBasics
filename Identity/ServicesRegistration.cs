using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Identity.Features;
using Identity.Interfaces;
using Microsoft.AspNetCore.Identity;
using Identity.Models;
using Identity.Data.Role;
using Identity.Data.Dbcontext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Identity
{
    public static class ServicesRegistrations
    {
        public static void AddServicesRegistrations(IServiceCollection services, IConfiguration configuration)

        {
            // mysql connection string 
            var Configuration = configuration;
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("DefaultConnection"),
                  new MySqlServerVersion(new Version(8, 0, 32))
                  )
                );

            services.AddDbContext<AuthDbContext>(options =>
                options.UseMySql(configuration.GetConnectionString("AuthConnection"),
                  new MySqlServerVersion(new Version(8, 0, 32))
                  )
                );
            // identity configuration 
            services.AddIdentity<AuthUser, IdentityRole>(options =>
            {
                // Simplify password requirements
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedAccount = false;
            }).AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();
            //--jwt token configuration 
            var jwtOptionsSection =Configuration.GetSection("JwtOptions");
            services.Configure<JwtOptions>(jwtOptionsSection);
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtOptionsSection.GetValue<string>("Issuer"),
                        ValidAudience = jwtOptionsSection.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionsSection.GetValue<string>("SigningKey")!))
                    };
                }
            );
            services.AddAuthorization();
            //identity db context 
            services.AddScoped<DbContext, AppDbContext>();
            services.AddTransient<RoleSeeder>();
            services.AddScoped<AdminSeeder>();
            //DI
            services.AddTransient<IUserAuthRepository, UserAuthRepository>();
            services.AddScoped<ITokenGeneration, TokenGeneration>();
            services.AddHostedService<DataSeederHostedService>();
            
        }
    }
}
