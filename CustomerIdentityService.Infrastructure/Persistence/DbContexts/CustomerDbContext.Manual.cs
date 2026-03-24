using CustomerIdentityService.Core.Models.Settings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerIdentityService.Infrastructure.Persistence.DbContexts
{
    public partial class CustomerDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Đưa logic dùng biến hằng của bạn vào đây
                optionsBuilder.UseSqlServer(ConnectionStrings.CustomerAppLocal);
            }
        }
    }
}
