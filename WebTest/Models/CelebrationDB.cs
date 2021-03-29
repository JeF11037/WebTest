using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebTest.Models
{
    public class CelebrationDB : CreateDatabaseIfNotExists<CelebrationContext>
    {
        protected override void Seed(CelebrationContext context)
        {
            base.Seed(context);
        }
    }
}