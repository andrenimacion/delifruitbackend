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
using WebApplicationSyscompsa.Models.cosecha_control;

namespace WebApplicationSyscompsa.Controllers
{
    [Route("api/cosecha")]
    [ApiController]
    public class cosechaController : ControllerBase
    {

        private readonly AppDbContext _context;

        public cosechaController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("saveCosecha")]
        public async Task<IActionResult> saveCosecha([FromBody] Control_cosecha model)
        {

            if (ModelState.IsValid)
            {

                _context.control_cosecha.Add(model);

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
        [Route("getCosecha/{top}/{order}")]
        public ActionResult<DataTable> getCosecha([FromRoute] int top, [FromRoute] string order)
        {

            string Sentencia = "select top("+top+") * from Control_cosecha order by id " + order;

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection( _context.Database.GetDbConnection().ConnectionString) )
            {                
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }

        [HttpGet]
        [Route("delCosecha/{id}")]
        public ActionResult<DataTable> delCosecha([FromRoute] int id)
        {

            string Sentencia = "delete from Control_cosecha where id = @ID";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@ID", id));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }

        [HttpPut]
        [Route("putCosecha/{id}")]
        public async Task<IActionResult> putCosecha([FromRoute] int id, [FromBody] Control_cosecha model)
        {

            if (id != model.id)
            {
                return BadRequest("El id del producto no es compatible, o no existe");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }


    }

        
}
