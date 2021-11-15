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

namespace WebApplicationSyscompsa.Controllers
{
    [Route("api/despachos_control_cab")]
    [ApiController]
    public class t_invcabgController : ControllerBase
    {
        private readonly AppDbContext _context;

        public t_invcabgController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("save_despacho_cab")]
        public async Task<IActionResult> save_despacho_cab([FromBody] t_invcabg model)
        {

            if (ModelState.IsValid)
            {

                _context.t_invcabg.Add(model);

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
        [Route("getExec/{token}/{pk2}")]
        public ActionResult<DataTable> getExec([FromRoute] string token, [FromRoute] string pk2)
        {

            string Sentencia = "exec sp_grabaDespacho_web @token, @pk2";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@token", token));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@pk2", pk2));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }

        [HttpGet]
        [Route("getExecReingreso/{token}/{pk2}")]
        public ActionResult<DataTable> getExecReingreso([FromRoute] string token, [FromRoute] string pk2)
        {

            string Sentencia = "exec sp_reingresoMateriaPrima_web @token, @pk2";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@token", token));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@pk2", pk2));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }

        [HttpGet]
        [Route("getExecEmpaque/{token}/{pk2}")]
        public ActionResult<DataTable> getExecEmpaque([FromRoute] string token, [FromRoute] string pk2)
        {
            string Sentencia = "exec sp_grabaEmpaque_web @token, @pk2";
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@token", token));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@pk2", pk2));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }

        [HttpGet]
        [Route("getExecReport/{type}/{number}/{optA}/{optB}")]

        public ActionResult<DataTable> getExecReport([FromRoute] string type, [FromRoute] string number, [FromRoute] string optA, [FromRoute] string optB )
        {
            /* Una vez obtenido el resultado de getExec[exec 
             * sp_grabaDespacho_web @token, @pk2], 
             * divido el codigo recibido en 2 tipo y numero*/
            string Sentencia = "exec sp_printer @type, @number, @opt1, @opt2";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", type));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@number", number));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@opt1", optA));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@opt2", optB));
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

