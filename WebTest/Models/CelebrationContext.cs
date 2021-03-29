using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebTest.Models
{
    public class CelebrationContext : DbContext
    {
        public DbSet<Celebration> Celebrations { get; set; }
    }
}