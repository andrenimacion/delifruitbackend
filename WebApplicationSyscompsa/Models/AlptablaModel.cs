using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models
{
    public class AlptablaModel
    {
        public int id { get; set; }
        public string   master  {get; set;}
        public string   codigo  {get; set;}
        public string   nombre  {get; set;}
        public decimal  valor   {get; set;}
        public string   nomtag  {get; set;}
        public string   gestion {get; set;}
        public int      pideval {get; set;}
        public string   campo1  {get; set;}
        public string   grupo   {get; set;}
        public string   sgrupo  {get; set;}
        public string   campo2  {get; set;}
        public decimal  lencod  {get; set;}
    }
}
