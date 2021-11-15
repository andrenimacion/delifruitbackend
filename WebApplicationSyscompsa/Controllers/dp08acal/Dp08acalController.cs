using GemBox.Spreadsheet;
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

namespace WebApplicationSyscompsa.Controllers.dp08acal
{
    [Route("api/dp08acal")]
    [ApiController]
    public class Dp08acalController : ControllerBase
    {

        private readonly AppDbContext _context;

        public Dp08acalController(AppDbContext context)
        {
            this._context = context;
        }


        [HttpPost]
        [Route("save_dp08acal")]
        public async Task<IActionResult> save_dp08acal([FromBody] Dp08acal model)
        {

            if (ModelState.IsValid)
            {

                _context.dp08acal.Add(model);

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
        [Route("getDp08acal/{order}")]
        public ActionResult<DataTable> getDp08acal([FromRoute] string order)
        {

            string Sentencia = " select coalesce(color_asign, 'whitesmoke') color_asign, anio, peri," +
                               " sema, finicio, ffin from dp08acal " +
                               " where YEAR(GETDATE()) = anio " +
                               " and color_asign != '' order by ffin  " + order;

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    //adapter.SelectCommand.Parameters.Add(new SqlParameter("@User", User));
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
        [Route("DelDp08acal/{anio}/{peri}/{sema}")]
        public ActionResult<DataTable> DelDp08acal([FromRoute] string anio, [FromRoute] string peri, [FromRoute] string sema)
        {
            string Sentencia = " delete from dp08acal where anio = @Anio and peri = @Peri and sema = @Sema ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@Anio", anio));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@Peri", peri));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@Sema", sema));
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
        [Route("dp03amov/{top}/{table}/{dbase}")]
        public ActionResult<DataTable> dp03amov([FromRoute] int top, [FromRoute] string table, [FromRoute] string dbase)
        {
            // SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            string Sentencia = " use " + dbase + " select top("+ top +") * from " + table; 
             DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    //adapter.SelectCommand.Parameters.Add(new SqlParameter("@Anio", anio));
                    //adapter.SelectCommand.Parameters.Add(new SqlParameter("@Peri", peri));
                    //adapter.SelectCommand.Parameters.Add(new SqlParameter("@Sema", sema));
                    adapter.Fill(dt);

                    // Deserialize JSON string
                    // Dictionary<string, User> users = JsonConvert.DeserializeObject<Dictionary<string, User>>(dt);

                    // Create empty excel file with a sheet
                    //ExcelFile workbook = new ExcelFile();
                    //ExcelWorksheet worksheet = workbook.Worksheets.Add("dp03amov");

                    //worksheet.Cells[0, 0].Value = "";
                    //worksheet.Cells[0, 1].Value = "Last name";
                    //worksheet.Cells[0, 2].Value = "Age";
                    //worksheet.Cells[0, 3].Value = "Email";

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
