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
using WebApplicationSyscompsa.Models.ar_loscann;

namespace WebApplicationSyscompsa.Controllers
{
    [Route("api/Ar_loscann")]
    [ApiController]
    public class Ar_loscannController : ControllerBase
    {
        private readonly AppDbContext _context;
        public Ar_loscannController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("getar_loscann/{order}")]
        public ActionResult<DataTable> getLoteFilter([FromRoute] string order)
        {

            string Sentencia = "select * from ar_loscann order by id " + order;
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@order", order));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }


        [HttpPost]
        [Route("save_ar_loscann")]
        public async Task<IActionResult> save_despacho_det([FromBody] Ar_loscann model)
        {

            if (ModelState.IsValid)
            {

                _context.ar_loscann.Add(model);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return Ok(model);
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

        [HttpGet]
        [Route("delar_loscann/{id}")]
        public ActionResult<DataTable> delar_loscann([FromRoute] int id)
        {

            string Sentencia = "delete from ar_loscann where id = @id ";
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@id", id));
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
