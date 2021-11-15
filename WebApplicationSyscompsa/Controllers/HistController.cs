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
using WebApplicationSyscompsa.Models.history;

namespace WebApplicationSyscompsa.Controllers.HistoryComproba
{
    [Route("api/historyComproban")]
    [ApiController]
    public class HistController : ControllerBase
    {

        private readonly AppDbContext _context;

        public HistController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("save_history")]
        public async Task<IActionResult> save_despacho_cab([FromBody] Hist_comp_emp model)
        {

            if (ModelState.IsValid)
            {

                _context.hist_comp_emp.Add(model);

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
        [Route("getComprobanHist/{top}")]
        public ActionResult<DataTable> getComprobanHist([FromRoute] int top)
        {

            string Sentencia = "select top(" + top + ") * from hist_comp_emp order by id desc ";

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
        [Route("delComprobanHist/{id}")]
        public ActionResult<DataTable> delComprobanHist([FromRoute] int id)
        {

            string Sentencia = "delete from hist_comp_emp where id = " + id;

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
