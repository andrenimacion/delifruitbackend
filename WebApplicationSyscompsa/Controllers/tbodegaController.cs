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
using WebApplicationSyscompsa.Models.traspase_product;

namespace WebApplicationSyscompsa.Controllers.TransferenciaBodegasControl
{
    [Route("api/transferbode")]
    [ApiController]
    public class TransferBodegaController : ControllerBase
    {

        private readonly AppDbContext _context;
        public TransferBodegaController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("getBodegas/{filt}")]
        public ActionResult<DataTable> getBodegas([FromRoute] string filt)
        {

            string Sentencia = " declare @filter varchar(50) = @filt " +
                               " select ltrim(rtrim( codigo )) codigo, nombre from dbo.df_alptabla('i_bode')" +
                               " where nombre like @filter or codigo like @filter " +
                               " order by nombre ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@filt", "%" + filt + "%"));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpGet]
        [Route("getBodegasProducts/{bodega}/{fecha}")]
        public ActionResult<DataTable> getBodegasProducts([FromRoute] string bodega, [FromRoute] DateTime fecha)
        {

            string Sentencia = " Declare @noParte varchar(15) = '', " +
                               " @Bodega char(6) = @bod, " +
                               " @fecha datetime = @fech " +
                               " Declare @mes int = month(@fecha) " +
                               " declare @querySum decimal = (select sum(difer_stcok) from traspase_product where codigo = @noParte ) " +
                               " select xx.*,( case  when xx.stock <= 0 then '#AE3413' " +
                               "         when xx.stock <= 100 then '#B87822'  else '#838F2E' end ) color " +
                               "         from " +
                               "         ( " +
                               " select a.bodega, a.no_parte, b.nombre, c.nombre presentacion, " +
                               " iif(@mes >= 0, coalesce(a.saldo00, 0), 0) + iif(@mes >= 1, coalesce(a.saldo01, 0), 0) + iif(@mes >= 2, coalesce(a.saldo02, 0), 0) + " +
                               " iif(@mes >= 3, coalesce(a.saldo03, 0), 0) + iif(@mes >= 4, coalesce(a.saldo04, 0), 0) + iif(@mes >= 5, coalesce(a.saldo05, 0), 0) + " +
                               " iif(@mes >= 6, coalesce(a.saldo06, 0), 0) + iif(@mes >= 7, coalesce(a.saldo07, 0), 0) + iif(@mes >= 8, coalesce(a.saldo08, 0), 0) + " +
                               " iif(@mes >= 9, coalesce(a.saldo09, 0), 0) + iif(@mes >= 10, coalesce(a.saldo10, 0), 0) + iif(@mes >= 11, coalesce(a.saldo11, 0), 0) + " +
                               " iif(@mes >= 12, coalesce(a.saldo12, 0), 0) - coalesce(@querySum, 0.00) stock_init, " +
                               " iif(@mes >= 0, coalesce(a.saldo00, 0), 0) + iif(@mes >= 1, coalesce(a.saldo01, 0), 0) + iif(@mes >= 2, coalesce(a.saldo02, 0), 0) + " +
                               " iif(@mes >= 3, coalesce(a.saldo03, 0), 0) + iif(@mes >= 4, coalesce(a.saldo04, 0), 0) + iif(@mes >= 5, coalesce(a.saldo05, 0), 0) + " +
                               " iif(@mes >= 6, coalesce(a.saldo06, 0), 0) + iif(@mes >= 7, coalesce(a.saldo07, 0), 0) + iif(@mes >= 8, coalesce(a.saldo08, 0), 0) + " +
                               " iif(@mes >= 9, coalesce(a.saldo09, 0), 0) + iif(@mes >= 10, coalesce(a.saldo10, 0), 0) + iif(@mes >= 11, coalesce(a.saldo11, 0), 0) + " +
                               " iif(@mes >= 12, coalesce(a.saldo12, 0), 0) stock, " +
                               " coalesce(d.cantidad, 0) cant " +
                               " from dp03a110 b with(nolock) " +
                               " left join dp03asal a with(nolock) on b.no_parte = a.no_parte " +
                               " left join dbo.df_alptabla('i_tpuni') c on c.codigo = b.desunidad " +
                               " left join traspase_product d on d.codigo = b.no_parte and a.bodega = d.bodega_ing " +
                               " where a.periodo = year(@fecha) " +
                               " and(len(@noParte) = 0 or b.no_parte = @noParte)  " +
                               " and(len(@Bodega) = 0 or a.bodega = @Bodega) " +
                               " and b.estado = 'A' " +
                               " ) xx " +
                               " order by xx.nombre ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@bod", bodega));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@fech", fecha));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpPost]
        [Route("save_transfer")]
        public async Task<IActionResult> save_transfer([FromBody] Traspase_product model)
        {

            var result = await _context.traspase_product.FirstOrDefaultAsync(x => x.codigo == model.codigo && x.descripcion == model.descripcion);

            if (result != null)
            {
                return Ok("Ya existe");
            }

            else
            {
                if (ModelState.IsValid)
                {

                    _context.traspase_product.Add(model);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Ok(model);
                    }

                    else
                    {
                        return BadRequest("no coincide");
                    }

                }

                else
                {
                    return BadRequest(ModelState);
                }


            }
        }


        [HttpGet]
        [Route("gettransprod")]
        public ActionResult<DataTable> gettransprod()
        {

            string Sentencia = "select * from traspase_product";

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
        [Route("deltransprod/{id}")]
        public ActionResult<DataTable> deltransprod([FromRoute] int id)
        {

            string Sentencia = "delete from traspase_product where id =  " + id;

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
        [Route("deltransprods")]
        public ActionResult<DataTable> deltransprods()
        {

            string Sentencia = "delete from traspase_product";

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


        [HttpPut]
        [Route("puttransprod/{Id}")]
        public async Task<IActionResult> puttransprod([FromRoute] int Id, [FromBody] Traspase_product model)
        {

            if (Id != model.id)
            {
                return BadRequest("El ID del producto no es compatible, o no existe");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }


        //Obtener transacción
        [HttpGet]
        [Route("getTransacProd")]
        public ActionResult<DataTable> getTransacProd()
        {

            string Sentencia = " select codigo,nombre from dp03acom where grupo='T' and" +
                               " coalesce(codigo_i,'')!='' and coalesce(codigo_e,'')!=''";

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

        [HttpPost]
        [Route("save_transfer_cab")]
        public async Task<IActionResult> save_transfer_cab([FromBody] t_invcabg model)
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

        [HttpPost]
        [Route("save_transfer_det")]
        public async Task<IActionResult> save_transfer_det([FromBody] t_invdetg model)
        {

            string Sentencia = "delete from traspase_product";

            if (ModelState.IsValid)
            {

                _context.t_invdetg.Add(model);
                if (await _context.SaveChangesAsync() > 0)
                {
                    DataTable dt = new DataTable();
                    using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                    {
                        using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.SelectCommand.CommandType = CommandType.Text;
                        adapter.Fill(dt);
                    }
                    return Ok(model);
                }

                else
                {
                    return BadRequest("Lago ha ocurrido, revisa el código");
                }

            }

            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet]
        [Route("getCodectransfer/{key}/{tempo}")]
        public ActionResult<DataTable> getCodectransfer([FromRoute] string key, [FromRoute] string tempo)
        {
            //'2MyVVu4rZd'
            //'transferencia'
            string Sentencia = "exec sp_tranferbode @Key, @tempo ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@Key", key));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@tempo", tempo));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpGet]
        [Route("getGrafic/{nparteProd}")]
        public ActionResult<DataTable> getGrafic([FromRoute] string nparteProd)
        {
            //'2MyVVu4rZd'
            //'transferencia'
            ///cuando es c-devolucion solo enviar fecha y bodega
            string Sentencia = " Declare @noParte varchar(15) = @nparteProd,		@Bodega char(6) = '',		@fecha datetime = getdate() " +
                   " Declare @mes int = month(@fecha) " +
                   " select b.no_parte, b.nombre, a.bodega,coalesce(c.nombre, 'No registrado..') nomBodega,sum( " +
                   "  iif(@mes >= 0, coalesce(a.saldo00, 0), 0) + iif(@mes >= 1, coalesce(a.saldo01, 0), 0) + iif(@mes >= 2, coalesce(a.saldo02, 0), 0) +  " +
                   "  iif(@mes >= 3, coalesce(a.saldo03, 0), 0) + iif(@mes >= 4, coalesce(a.saldo04, 0), 0) + iif(@mes >= 5, coalesce(a.saldo05, 0), 0) +  " +
                   "  iif(@mes >= 6, coalesce(a.saldo06, 0), 0) + iif(@mes >= 7, coalesce(a.saldo07, 0), 0) + iif(@mes >= 8, coalesce(a.saldo08, 0), 0) +  " +
                   "  iif(@mes >= 9, coalesce(a.saldo09, 0), 0) + iif(@mes >= 10, coalesce(a.saldo10, 0), 0) + iif(@mes >= 11, coalesce(a.saldo11, 0), 0) +" +
                   "  iif(@mes >= 12, coalesce(a.saldo12, 0), 0)) stock " +
                   "  from dp03a110 b with(nolock) " +
                   " left join dp03asal a with(nolock) on b.no_parte = a.no_parte" +
                   " left join dbo.df_alptabla('i_bode') c on c.codigo = a.bodega" +
                   " where a.periodo = year(@fecha)" +
                   " and(len(@noParte) = 0 or b.no_parte = @noParte) " +
                   " and(len(@Bodega) = 0 or a.bodega = @Bodega) " +
                   " and b.estado = 'A' " +
                   " group by b.no_parte, b.nombre, a.bodega,coalesce(c.nombre, 'No registrado..') " +
                   " order by coalesce(c.nombre, 'No registrado..')";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@OP", OP));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@nparteProd", nparteProd));
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@bodeg", bodeg));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpGet]
        [Route("getGraficVarsnparteProd/{bode}/{dtime}")]
        public ActionResult<DataTable> getGraficVars([FromRoute] string nparteProd, [FromRoute] string bode, [FromRoute] DateTime date)
        {
            //'2MyVVu4rZd'
            //'transferencia'
            ///cuando es c-devolucion solo enviar fecha y bodega
            string Sentencia = " Declare @noParte varchar(15) = @nparteProd,		@Bodega char(6) = @bod,		@fecha datetime = @dat " +
                   " Declare @mes int = month(@fecha) " +
                   " select b.no_parte, b.nombre, a.bodega,coalesce(c.nombre, 'No registrado..') nomBodega,sum( " +
                   "  iif(@mes >= 0, coalesce(a.saldo00, 0), 0) + iif(@mes >= 1, coalesce(a.saldo01, 0), 0) + iif(@mes >= 2, coalesce(a.saldo02, 0), 0) +  " +
                   "  iif(@mes >= 3, coalesce(a.saldo03, 0), 0) + iif(@mes >= 4, coalesce(a.saldo04, 0), 0) + iif(@mes >= 5, coalesce(a.saldo05, 0), 0) +  " +
                   "  iif(@mes >= 6, coalesce(a.saldo06, 0), 0) + iif(@mes >= 7, coalesce(a.saldo07, 0), 0) + iif(@mes >= 8, coalesce(a.saldo08, 0), 0) +  " +
                   "  iif(@mes >= 9, coalesce(a.saldo09, 0), 0) + iif(@mes >= 10, coalesce(a.saldo10, 0), 0) + iif(@mes >= 11, coalesce(a.saldo11, 0), 0) +" +
                   "  iif(@mes >= 12, coalesce(a.saldo12, 0), 0)) stock " +
                   "  from dp03a110 b with(nolock) " +
                   " left join dp03asal a with(nolock) on b.no_parte = a.no_parte" +
                   " left join dbo.df_alptabla('i_bode') c on c.codigo = a.bodega" +
                   " where a.periodo = year(@fecha)" +
                   " and(len(@noParte) = 0 or b.no_parte = @noParte) " +
                   " and(len(@Bodega) = 0 or a.bodega = @Bodega) " +
                   " and b.estado = 'A' " +
                   " group by b.no_parte, b.nombre, a.bodega,coalesce(c.nombre, 'No registrado..') " +
                   " order by coalesce(c.nombre, 'No registrado..')";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@OP", OP));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@nparteProd", nparteProd));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@bod", bode));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@fecha", date));
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@bodeg", bodeg));
                adapter.Fill(dt);
            }

            if (dt == null) {
                return NotFound("");
            }

            return Ok(dt);

        }





    }

}
