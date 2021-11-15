using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models
{
    public class Audit_print_lote
    {
        public int id { get; set; }
        public string user_name { get; set; }
        public DateTime? finit { get; set; }
        public DateTime? ffin { get; set; }
        public string codec_lotes { get; set; }
        public string hacienda_tag { get; set; }
        public string codec_lotes_master { get; set; }
        public int cantidad { get; set; }
        //public string token_user { get; set; }
    }
}
