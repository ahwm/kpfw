﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kpfw.DataModels
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Episode> Episodes { get; set; }

        public DbSet<Timeline> Timeline { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<BouncedEmail> BouncedEmails { get; set; }
        public DbSet<ComplainedEmail> ComplainedEmails { get; set; }
    }
}
