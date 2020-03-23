using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BelajarCRUDWPF.Model;

namespace BelajarCRUDWPF.MyContext
{
    public class myContext : DbContext
    {
        public myContext(): base("CRUDWPF") { }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Role> Roles { get; set; }


    }
}
