using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models.cosecha_control
{
    public class Control_cosecha
    {

      public int    id           { get; set; }
      public string code_user    { get; set; }
      public int    cortes       { get; set; }
      public string semana       { get; set; }
      public string color        { get; set; }
      public string hacienda     { get; set; }
      public DateTime f_cosecha  { get; set; }
      public string cod_hacienda { get; set; }

    }
}
