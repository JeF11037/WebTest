using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebTest.Models
{
    public class GuestDB : CreateDatabaseIfNotExists<GuestContext>
    {
        protected override void Seed(GuestContext context)
        {
            base.Seed(context);
        }
    }
}