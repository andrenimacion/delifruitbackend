using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationSyscompsa.Models.Mail_Send
{
    public class mensajeMod
    {
        public int Id { get; set; }
        public string txtPara { get; set; }
        public string txtAsunto { get; set; }
        public string txtCopia { get; set; }
        public string txtMensaje { get; set; }
        public string MailAddress { get; set; }
        public string passwordMail { get; set; }
        public string date_send_mail { get; set; }
    }
}
