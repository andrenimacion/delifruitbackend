using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models.history
{
    public class Hist_comp_emp
    {
           public int    id              { get; set; }
           public string comprobante_num { get; set; }
           public DateTime? datetime_sal { get; set; }
           public decimal cant_total { get; set; }
           public decimal cant_desp  { get; set; }
           public string bodega  { get; set; }
           public string detalle { get; set; }

    }
}
