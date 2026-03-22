using CustomerIdentityService.Application.Common.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerIdentityService.Application.DependencyInjection
{
    public static class AutoMapperServiceRegistration
    {
        public static IServiceCollection AddAutoMapperServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CustomerProfile>();
            });

            return services;
        }
    }
}
