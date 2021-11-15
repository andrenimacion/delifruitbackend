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

namespace WebApplicationSyscompsa.Controllers.audit_
{
    [Route("api/Taudit")]
    [ApiController]
    public class Taudit : ControllerBase
    {

        private readonly AppDbContext _context;

        public Taudit(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("geT_Audit")]
        public ActionResult<DataTable> geT_Audit()
        {

            string Sentencia = " select * from t_audit ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(Sentencia, connection)) 
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.Fill(dt);
                }

            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }



    }
}
