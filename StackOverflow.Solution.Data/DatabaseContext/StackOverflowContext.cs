using Microsoft.EntityFrameworkCore;
using StackOverflow.Solution.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverflow.Solution.Data.DatabaseContext
{
    public class StackOverflowContext:DbContext
    {
        public StackOverflowContext(DbContextOptions options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


        }
        public DbSet<User> Users { get; set; }
    }
}
