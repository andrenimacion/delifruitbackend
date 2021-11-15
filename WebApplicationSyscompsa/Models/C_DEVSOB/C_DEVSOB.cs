using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models
{
    public class C_DEVSOB
    {
        public string lote_prod {get; set;}
        public DateTime finit { get; set; }
        public DateTime ffin { get; set; }
        public string observ_dev { get; set; }
        public decimal cant { get; set; }
        public decimal cant_dev { get; set; }
        public decimal total { get; set; }
        public string campo { get; set; }
        public string campoA { get; set; }
        public string campoB { get; set; }
    }
}
