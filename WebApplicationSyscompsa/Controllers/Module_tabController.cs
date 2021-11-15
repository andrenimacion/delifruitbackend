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

namespace WebApplicationSyscompsa.Controllers.Mod_tab_control
{
    [Route("api/modcon")]
    [ApiController]
    public class Module_tabController : ControllerBase
    {
        private readonly AppDbContext _context;
        public Module_tabController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("get_module")]
        public ActionResult<DataTable> get_module([FromRoute] string order)
        {

            string Sentencia = " select * from module_tab ";
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@order", order));
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
