using BelajarCRUDWPF.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.MyContext
{
    public class MyContext : DbContext
    {
        public MyContext() : base("BelajarCRUDWPF") { }
        public DbSet<Supplier> Suppliers { get; set; } //mendaftafkan tabel mana yang akan dibuat
    }
}
