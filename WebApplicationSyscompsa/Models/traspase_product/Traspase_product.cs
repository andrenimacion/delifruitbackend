
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models.traspase_product
{
    public class Traspase_product
    {
        public int       id             { get; set; }
        public string    bodega_ing     { get; set; }
        public string    codigo         { get; set; }
        public string    descripcion    { get; set; }
        public string    presentacion   { get; set; }
        public decimal   cantidad       { get; set; }
        public decimal   difer_stcok    { get; set; }
        public decimal   stock          { get; set; }

        // public DateTime? fecha       { get; set; }
    }

    //public class t_invcabG_t
    //{
    //    //pk
    //    public string t_llave   { get; set; }
    //    //pk
    //    public string tempo     { get; set; }
    //    public string tipo      { get; set; }
    //    public DateTime? fecha_tra { get; set; }
    //    public string bodega    { get; set; }
    //    public string bodega_d  { get; set; }
    //    public string consumo   { get; set; }
    //    public string comenta   { get; set; }
    //    public string usercla   { get; set; }
    //    public string tipoCosto { get; set; }

    //}

    //public class t_invdetg_t
    //{
    //    //pk
    //    public string  t_llave   {get; set;}
    //    //pk
    //    public string  tempo     {get; set;}
    //    //pk
    //    public int     linea    {get; set;}
    //    public string  no_parte {get; set;}
    //    public decimal cantidad {get; set;}
    //    public decimal precio_u {get; set;}
    //    public decimal precio_t {get; set;}
        
    //}


}
