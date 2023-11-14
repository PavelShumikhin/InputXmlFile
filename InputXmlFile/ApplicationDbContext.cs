using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputXmlFile
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Card> Clients { get; set; }

        public ApplicationDbContext(): base("Data Source=Shuma\\MSSQLSERVER02;Initial Catalog=Test;Integrated Security=True") { 
        }
    }
}
