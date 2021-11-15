using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models.despacho_save
{
    public class t_invdetg
    {
        
        public string T_llave   { get; set; }
        public string tempo     { get; set; }
        public int linea        { get; set; }
        public string no_parte  { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio_u { get; set; }
        public decimal precio_t { get; set; }
        public decimal cant_tou { get; set; }
        public string trnVenta  { get; set; }
        public string lote      { get; set; }

    }
}
