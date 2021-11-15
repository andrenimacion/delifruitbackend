using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationSyscompsa.Models;
using WebApplicationSyscompsa.Models.APCIAS_MODEL;

namespace WebApplicationSyscompsa.Controllers.APCIAS_CONTROLLERS
{
    [Route("api/webmail")]
    [ApiController]
    public class apcia_Contoller : ControllerBase
    {

        private readonly AppDbContext _context;

        public apcia_Contoller(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("UpdateMail/{email}")]
        public ActionResult<DataTable> UpdateMail([FromRoute] string email)
        {
            string Sentencia = "update apcias set email_despacho_web = @email";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@email", email));
                adapter.Fill(dt);

            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpGet]
        [Route("SelectMail")]
        public ActionResult<DataTable> SelectMail()
        {
            string Sentencia = "select * from apcias";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@email", email));
                adapter.Fill(dt);

            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

    }
}
