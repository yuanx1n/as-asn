﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Model
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
		public DbSet<Auditlog> Auditlogs { get; set; }


		private readonly IConfiguration _configuration;
		//public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
		public AuthDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
