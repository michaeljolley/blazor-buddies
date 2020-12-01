using System;
using Microsoft.EntityFrameworkCore;

using BlazorBuddies.Core.Common;

namespace BlazorBuddies.Core.Data
{
    public class BuddyDbContext: DbContext
    {
        public BuddyDbContext(DbContextOptions<BuddyDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public virtual DbSet<Donor> Donors { get; set; }
        public virtual DbSet<School> Schools { get; set; }
    }
}
