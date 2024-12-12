using EventManagement.API.Service;
using EventManagement.Data.Context;
using EventManagement.Data.MappingService;
using EventManagement.Data.Repository;
using EventManagement.Data.Repository.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace EventManagement.API
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
                
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Event Mgm API", Version = "v1" });

                // Add security definition for JWT
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter 'Bearer' [space] and then your token",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration["ConnectionStrings:default"]));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped<IEventRepo, EventRepository>();

            builder.Services.AddScoped<IRsvpRepo, RsvpRepository>();

            builder.Services.AddScoped<ISendService, EmailService>();

            builder.Services.AddScoped<EmailSender>();

            builder.Services.AddScoped<IAuthRepo, AuthRepository>();

            builder.Services.AddScoped<TokenService>();

            builder.Services.AddScoped<ISuperAdmin, SuperAdminRepository>();

            builder.Services.AddScoped<IUserRepo, UserRepository>();

            builder.Services.AddHostedService<NotificationSender>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters = new TokenValidationParameters()
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = builder.Configuration["Jwt:Issuer"],
                           ValidAudience = builder.Configuration["Jwt:Audience"],
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                       };
                   });

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

            //Enable Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });


            app.Run();
        }
    }
}