using CustomerIdentityService.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CustomerIdentityService.API.Configurations
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(
              configuration.GetSection("JwtSettings"));

            services.Configure<ExternalProviderOptions>(
               configuration.GetSection("ExternalProviders"));

            return services;
        }
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {


            //tạo cấu hình
            var jwtSettings = configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>()
                ?? throw new InvalidOperationException("JwtSettings missing");

            var _internalAuthIssuer = configuration["EcommerceApiGateWay:BaseUrl"];
            services.AddSingleton<IAuthorizationHandler, InternalOrPermissionHandler>();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = jwtSettings.Issuer;
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,

                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidateIssuerSigningKey = true,
                    };
                });



            services.AddAuthorization();
            return services;
        }
    }
}
