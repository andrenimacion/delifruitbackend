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
using WebApplicationSyscompsa.Models.despacho_save;

namespace WebApplicationSyscompsa.Controllers
{
    [Route("api/despachos_control_det")]
    [ApiController]
    public class t_invdetgController : ControllerBase
    {

        private readonly AppDbContext _context;
        public t_invdetgController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("save_despacho_det")]
        public async Task<IActionResult> save_despacho_det([FromBody] t_invdetg model)
        {

            if (ModelState.IsValid)
            {

                _context.t_invdetg.Add(model);

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
        [Route("getDetail")]
        public ActionResult<DataTable> getDetail()
        {

            string Sentencia = "select * from t_invdetg";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
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
        [Route("getCab")]
        public ActionResult<DataTable> getCab()
        {

            string Sentencia = "select * from t_invcabg";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
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
        [Route("delDetail")]
        public ActionResult<DataTable> delDetail()
        {

            string Sentencia = "delete from t_invdetg";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
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
        [Route("delCab")]
        public ActionResult<DataTable> delCab()
        {

            string Sentencia = "delete from t_invcabg";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
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


    }
}
