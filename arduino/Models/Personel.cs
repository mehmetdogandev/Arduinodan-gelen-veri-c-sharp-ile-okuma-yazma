using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arduino.Models
{
    internal class Personel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Tarih { get; set; }
        public int gazOrani { get; set; }

    }
}
