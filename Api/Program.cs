using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RSAHyundai.AutoMapper;
using RSAHyundai.Api.Extensions;
using RSAHyundai.Api.Middleware;
using RSAHyundai.Authentication;
using RSAHyundai.Data;
using RSAHyundai.DTOs.Projects;
using RSAHyundai.Encryption;
using RSAHyundai.Filtering;
using RSAHyundai.Interfaces;
using RSAHyundai.Services;

namespace RSAHyundai.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration
            var configuration = builder.Configuration;

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddApiVersioning();

            // Swagger Documentation
            builder.Services.AddSwaggerDocumentation();

            // Encryption & Token
            builder.Services.AddSingleton<IPasswordHasher, PBKDF2Hasher>();
            builder.Services.AddSingleton<ITokenHelper, JWTHelper>();

            // Paging & Sorting on Web-Request
            builder.Services.AddScoped<IFilterHelper<ProjectDetailsDTO>, FilterHelper<ProjectDetailsDTO>>();

            // Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IProjectService, ProjectService>();
            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddScoped<ITagService, TagService>();

            // Automapper
            builder.Services.AddSingleton(provider => new MapperConfiguration(options =>
            {
                options.AddProfile(new MappingProfile(provider.GetService<IPasswordHasher>()));
            }).CreateMapper());

            // Authentication
            var tokenConfiguration = configuration.GetSection("TokenSettings");
            builder.Services.Configure<TokenSettings>(tokenConfiguration);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var tokenSettings = tokenConfiguration.Get<TokenSettings>();
                var symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings.Secret));
                var keyExpiration = tokenSettings.AccessExpirationInMinutes;

                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricKey,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                };
            });

            // Authorization
            builder.Services.AddAuthorizationBuilder()
            // Authorization
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build());

            // Entity Framework - Database Connection
            var connectionString = builder.Configuration.GetConnectionString("DbConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseGlobalExceptionMiddleware();
            }

            app.UseSwaggerDocumentation();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();


        }
    }
}
