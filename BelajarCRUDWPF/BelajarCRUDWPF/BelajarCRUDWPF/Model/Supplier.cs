using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("tbl_m_supplier")]       //table supplier
    public class Supplier
    {
        [Key]       //definisi primary key dan auto increment
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Address { get; set; }
        public Supplier()
        { 

        }
        public Supplier(String nama, String address)
        {
            this.Nama = nama;
            this.Address = address;
        }
    }
}
