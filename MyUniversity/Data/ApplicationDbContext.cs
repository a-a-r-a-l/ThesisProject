using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyUniversity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyUniversity.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<MyIdentityUser, MyIdentityRole, Guid>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculty { get; set; }



        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
    }
}
