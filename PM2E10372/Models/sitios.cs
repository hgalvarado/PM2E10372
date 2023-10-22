using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PM2E10372.Models
{
    public class sitios
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public double latitud { get; set; }

        public double longitud { get; set;}

        [MaxLength(100)]
        public string descripcion { get; set; }
        public byte[] foto { get; set; }

    }
}
