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
    [Route("api/AuditPrint")]
    [ApiController]
    public class AuditPrintController : ControllerBase
    {

        private readonly AppDbContext _context;
        public AuditPrintController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("Save_audit_print_lote")]
        public async Task<IActionResult> Save_audit_print_lote([FromBody] Audit_print_lote model)
        {

            if (ModelState.IsValid) {

                _context.audit_print_lote.Add(model);

                if (await _context.SaveChangesAsync() > 0) {
                    
                    return Ok(model);
                
                }

                else {

                    return BadRequest("Datos incorrectos");
                
                }

            }

            else {

                return BadRequest(ModelState);
            
            }

        }

        [HttpGet]
        [Route("getAudit/{User}/{Codec}")]
        public ActionResult<DataTable> getAudit([FromRoute] string User, [FromRoute] string Codec )
        {
            // exec AR_audit '_void_', '%DEL_H-001%'
            string Sentencia = " exec AR_audit @User, @codec ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using ( SqlCommand cmd = new SqlCommand(Sentencia, connection) )
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter( "@User",  User  ));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Codec", "%" + Codec + "%" ));
                    adapter.Fill(dt);
                }

            }

            if (dt == null)
            {

                return NotFound("");

            }

            return Ok(dt);

        }

        [HttpGet]
        [Route("GetLotes/{codec_lotes_master}/{codec_lotes}")]
        public ActionResult<DataTable> GetLotes([FromRoute] string codec_lotes_master, [FromRoute] string codec_lotes)
        {

            //string Sentencia = "select * from audit_print_lote where codec_lotes_master like @Codec_lotes_master and codec_lotes = @Codec_lotes";
            string Sentencia = "   select a.codec_lotes, a.codec_lotes_master, a.hacienda_tag, " +
                               "   b.cant, b.cant_dev, b.total totalStock from audit_print_lote a " +
                               "   left join C_DEVSOB b on b.lote_prod = a.hacienda_tag " +
                               "   where codec_lotes_master " +
                               "   like @Codec_lotes_master and codec_lotes = @Codec_lotes ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(Sentencia, connection)) {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@Codec_lotes_master", "%" + codec_lotes_master + "%"));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@Codec_lotes", codec_lotes ));
                    adapter.Fill(dt);
                }

            }

            if (dt == null) {
                return NotFound("");
            }

            //return Ok(codec_lotes_master + '/' + codec_lotes);
            return Ok(dt);

        }

    }
}
