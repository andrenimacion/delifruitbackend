using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models.ar_loscann
{
    public class Ar_loscann
    {
        public int id             { get; set; }
        public string descripcion { get; set; }
        public string date_toma   { get; set; }
        public string lote        { get; set; }
        public string n_parte     { get; set; }
    }
}
