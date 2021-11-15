using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationSyscompsa.Models;
using WebApplicationSyscompsa.Models.Mail_Send;

namespace WebApplicationSyscompsa.Controllers.Email_Send
{
    [Route("api/mailing")]
    [ApiController]
    public class emailSends : ControllerBase
    {
        private readonly AppDbContext _context;

        public emailSends(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("MailSendSave")]
        public async Task<IActionResult> CanvaSave([FromBody] mensajeMod model)
        {

            if (ModelState.IsValid)
            {
                _context.mensajeMod.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {

                    MailMessage msg = new MailMessage();

                    //CABECERA DE EMAIL
                    msg.To.Add(model.txtPara);
                    msg.Subject = model.txtAsunto;
                    msg.SubjectEncoding = System.Text.Encoding.UTF8;
                    msg.Bcc.Add(model.txtCopia);

                    //CUERPO DEL EMAIL
                    msg.Body = model.txtMensaje;
                    msg.BodyEncoding = System.Text.Encoding.UTF8;
                    msg.IsBodyHtml = true;
                    msg.From = new MailAddress(model.MailAddress);

                    //CLIENTE CORREO
                    SmtpClient cliente = new SmtpClient();
                    cliente.Credentials = new NetworkCredential(model.MailAddress, model.passwordMail);

                    //ACCESO AL PUERTO DE GMAIL

                    //HACER VARIABLES NO TE OLVIDES PARA QUE EL GUTY LO CAMBIE
                    #region
                    cliente.Port = 587;
                    cliente.EnableSsl = true;
                    cliente.Host = "smtp.gmail.com";
                    #endregion

                    try
                    {

                        cliente.Send(msg);
                        return Ok("Email enviado: " + model.txtPara + " / " + model.txtAsunto);

                    }

                    catch
                    {

                        return BadRequest("Email no enviado: " + model.txtPara + " / " + model.txtAsunto);

                    }

                }

                else
                {

                    return BadRequest("Datos incorrectos");

                }
            }

            else
            {

                return BadRequest(ModelState);

            }
        }

    }
}
