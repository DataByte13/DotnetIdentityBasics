using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using AuthenticationApi;
using Microsoft.IdentityModel.Tokens;
using Identity;
var builder = WebApplication.CreateBuilder(args);
var jwtOptions = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

// Add services to the container.
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var configuration = builder.Configuration;
Identity.ServicesRegistrations.AddServicesRegistrations(builder.Services, configuration);

//-----------------jwt stuff 
// var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");
// builder.Services.AddAuthentication(opt =>
// {
//     opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//     opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//     opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//
// }).AddJwtBearer(x =>
//     {
//         x.TokenValidationParameters = new TokenValidationParameters
//         {
//             ValidateIssuer = true,
//             ValidateAudience = true,
//             ValidateLifetime = true,
//             ValidateIssuerSigningKey = true,
//             ValidIssuer = jwtOptionsSection.GetValue<string>("Issuer"),
//             ValidAudience = jwtOptionsSection.GetValue<string>("Audience"),
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptionsSection.GetValue<string>("SigningKey")!))
//         };
//     }
//);
builder.Services.AddAuthorization();
// --------------- service registration in identity  stuff 
//---------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
