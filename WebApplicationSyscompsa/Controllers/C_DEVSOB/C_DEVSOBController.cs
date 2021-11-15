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

    [Route("api/c_devsob")]
    [ApiController]
    public class C_DEVSOBController : ControllerBase
    {

        private readonly AppDbContext _context;
        public C_DEVSOBController(AppDbContext context)
        {
            this._context = context;
        }


        [HttpPost]
        [Route("SaveDevSob")]
        public async Task<IActionResult> SaveDevSob([FromBody] C_DEVSOB model)
        {
            var result = await _context.C_DEVSOB.FirstOrDefaultAsync( x => x.lote_prod == model.lote_prod );



            if( result == null )
            {
                if (ModelState.IsValid)
                {
                    _context.C_DEVSOB.Add(model);
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
                    return BadRequest("ERROR");
                }
            }

            else
            {
                return Ok("Lote esta repetido, acceso denegado");
            }

        }
       

        [HttpGet]
        [Route("getC_DEVSOB/{tokenUser}/{codecLote}")]
        public ActionResult<DataTable> getC_DEVSOB([FromRoute] string tokenUser, [FromRoute] string codecLote)
        {

            //string Sentencia = "select * from C_DEVSOB where campoA = @TokenUser ";
            string Sentencia = " declare @filter varchar(50) = @CodLote" +
                               " declare @codec varchar(100) = (select coalesce(rtrim(ltrim(codigo)), '----') nombre from ALPTABLA where nomtag like @filter) " +
                               " select cod_hacienda = @codec, b.codec_lotes, c.nombre nom_lote, c.valor hec_lote, a.*from C_DEVSOB a " +
                               " left join audit_print_lote b on b.hacienda_tag = a.lote_prod " +
                               " left join ALPTABLA c on c.codigo = b.codec_lotes " +
                               " where c.grupo = 'd-000' " +
                               " and master = @codec and a.campoA = @TokenUser ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add( new SqlParameter("@TokenUser", tokenUser) );
                adapter.SelectCommand.Parameters.Add( new SqlParameter("@CodLote", "%" + codecLote + "%" ) );
                adapter.Fill(dt);
            
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpPut]
        [Route("puttransprod/{LoteProd}")]
        public async Task<IActionResult> puttransprod([FromRoute] string LoteProd, [FromBody] C_DEVSOB model)
        {

            if (LoteProd != model.lote_prod)
            {
                return BadRequest("El Lote del producto no es compatible, o no existe");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

    }
}
