using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BelajarCRUDWPF.Model
{
    [Table("Tb_M_Role")]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Nama { get; set; }


        public Role()
        {

        }

        public Role(string nama)
        {
            this.Nama = nama;
           
        }
    }
}
