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
using WebApplicationSyscompsa.Models.code_color_lab;

namespace WebApplicationSyscompsa.Controllers.deli_ALPTABLA
{
    
    [Route("api/control_alp_master_tabla")]
    [ApiController]
    public class ALPTablaController: ControllerBase
    {


        private readonly AppDbContext _context;
        public ALPTablaController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("save_alp_master")]
        public async Task<IActionResult> save_despacho_det([FromBody] AlptablaModel model)
        {

            if (ModelState.IsValid)
            {

                _context.alptabla.Add(model);

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


        [HttpPost]
        [Route("save_color_code_lab")]
        public async Task<IActionResult> save_color_code_lab([FromBody] Code_color_lab model)
        {

            if (ModelState.IsValid)
            {

                _context.code_color_lab.Add(model);

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
        [Route("geMaster/{nomtag}/{Property}")]
        public ActionResult<DataTable> geMaster([FromRoute] string nomtag, [FromRoute] string Property)
        {

            string Sentencia = "select * from ALPTABLA where " + Property + " = @NomTag ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@NomTag", nomtag));
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
        [Route("delMasterData/{master}/{codec}")]
        public ActionResult<DataTable> delMasterData([FromRoute] string master, [FromRoute] string codec)
        {

            string Sentencia = "delete from ALPTABLA where master = @Master and codigo = @Codec";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection)) {

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Master", master ));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Codec" , codec  ));
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
        [Route("updateMasterData/{sgrupo}/{nomtag}/{newName}")]
        public ActionResult<DataTable> updateMasterData([FromRoute] string sgrupo, [FromRoute] string nomtag, [FromRoute] string newName)
        {
            //'HCIE_GR'
            string Sentencia = "update ALPTABLA set nombre = @Newname where sgrupo = @Sgrupo and nomtag like @Nomtag";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection)) {

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Sgrupo", sgrupo ));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Nomtag" , "%" + nomtag + "%"  ));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@Newname", newName  ));
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
        [Route("FilterDataModuleGBarCode/{data}/{opt}")]
        public ActionResult<DataTable> FilterDataModuleGBarCode([FromRoute] string data, [FromRoute] string opt)
        {
            //'HCIE_GR'
            string Sentencia = "exec AR_filter_hac @data, @opt";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection)) {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@data", data));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@opt",  opt ));
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
        [Route("gecolor/{CODEC}")]
        public ActionResult<DataTable> gecolor([FromRoute] string CODEC)
        {
            string Sentencia = "select * from code_color_lab where descrip_labor = @codec ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codec", CODEC));
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
        [Route("delcolor/{CODEC}/{np}")]
        public ActionResult<DataTable> delcolor([FromRoute] string CODEC, [FromRoute] string np)
        {
            string Sentencia = "delete from code_color_lab where descrip_labor = @codec and s_codec = @np ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(Sentencia, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.SelectCommand.CommandType = CommandType.Text;
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@codec", CODEC));
                    adapter.SelectCommand.Parameters.Add(new SqlParameter("@np",    np));
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
