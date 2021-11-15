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

namespace WebApplicationSyscompsa.Controllers.CONTROL_PRODUCTOS
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("getLote/{filter}/{nopart}/{tp}")]
        public ActionResult<DataTable> getLote([FromRoute] string filter, [FromRoute] string nopart, [FromRoute] int tp) {
            // and coalesce(a.crtempca_web,'')= 'T' "+
            string Sentencia = "declare @tipo varchar(60) = @filter " +
                " declare @nopart varchar(10) = @npart " +
                " declare @top int = " + tp +
                " if (@top = 0) " +
                    "   begin " +
                    "   print 'opcion1' " +
                    "   select a.tipo + a.numero comproba, f.img_no_parte imagen," +
                    "   mensaje_query = 'esta es la opcion 1 del query', ltrim(rtrim(a.lote)) lote, " +
                	"	cast(a.fecha_tra AS date) fecha_tra, c.no_parte, cantidad_item = (select count(*)" +
                    "   from dp03amov where tipo = 'EM'), " +
                	"	coalesce(d.nombre, '') nomParte,  " +
                	"	(coalesce(c.cantidad, 0) - coalesce(c.cantidad2, 0)) cantidad, c.bodega, " +
                	"	coalesce(e.nombre, '') nomBodega, coalesce(a.crtempca_web, '') estado_lote,  " +
                	"	(case coalesce(a.crtempca_web, '') when 'E' then '#7DB24C' when 'C' then '#E7632A' else '#BC9D4C' end)  colorTable " +
                    "   from dp03amov c with(nolock) " +
                    "   left  join dpinvcab a with(nolock) on a.tipo = c.tipo and a.numero = c.numero " +
                    "   inner join dp03acom b with(nolock) on b.codigo = a.tipo and b.controlemp_web = 'M' "+
                    "   left  join dp03a110 d with(nolock) on d.no_parte = c.no_parte "   +
                    "   left  join dbo.df_alptabla('i_bode') e on e.codigo = c.bodega "   +
                    "   left  join img_lote f with(nolock) on f.no_parte_i = c.no_parte " +
                    "   where coalesce(a.lote,'')!= ''  " +
                    "   and((a.lote like @tipo and c.no_parte like @nopart) or(d.nombre like" +
                    "   @tipo or a.tipo + a.numero like @tipo)) " + 
                    "   and(coalesce(c.cantidad, 0) - coalesce(c.cantidad2, 0)) > 0 " +
                    "   order by a.tipo,a.numero " +
                    "   end " +
                    "   else if (@top >= 1) " +
                    "   begin " +
                    "   print 'opcion2' " +
                    "   select top(@top) " +
                    "   a.tipo + a.numero comproba, f.img_no_parte imagen, " +
                    "   mensaje_query = 'esta es la opcion 2 del query', " +
                    "   ltrim(rtrim(a.lote)) lote, cast(a.fecha_tra AS date) fecha_tra, " +
                	"   c.no_parte, cantidad_item = (select count(*) from dp03amov where tipo = 'EM'), " +
                    "   coalesce(d.nombre, '') nomParte, (coalesce(c.cantidad, 0) - coalesce(c.cantidad2, 0)) cantidad,c.bodega, " + 
                    "   coalesce(e.nombre, '') nomBodega, coalesce(a.crtempca_web, '') estado_lote, " +
                    "   (case coalesce(a.crtempca_web, '') when 'E' then '#7DB24C' when 'C' then '#E7632A' else '#BC9D4C' end)  colorTable " +
                    "   from dp03amov c with(nolock) " +
                    "   left join dpinvcab a with(nolock) on a.tipo = c.tipo and a.numero = c.numero " +
                    "   inner join dp03acom b with(nolock) on b.codigo = a.tipo and b.controlemp_web = 'M' " +
                    "   left join dp03a110 d with(nolock) on d.no_parte = c.no_parte " +
                    "   left join dbo.df_alptabla('i_bode') e on e.codigo = c.bodega " +
                    "   left join img_lote f with(nolock) on f.no_parte_i = c.no_parte " +
                    "   where coalesce(a.lote,'')!= ''  " +
                    "   and((a.lote like @tipo and c.no_parte like @nopart) or(d.nombre like @tipo or a.tipo + a.numero like @tipo)) " + 
                    "   and(coalesce(c.cantidad, 0) - coalesce(c.cantidad2, 0)) > 0 " +
                    "   order by a.tipo,a.numero " +
                    "   end ";
            //and coalesce(a.crtempca_web,'')= 'T' or  coalesce(a.crtempca_web,'')= 'E'"
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString)) {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter( "@tp", tp ));
                adapter.SelectCommand.Parameters.Add(new SqlParameter( "@filter", "%" + filter + "%" ));
                adapter.SelectCommand.Parameters.Add(new SqlParameter( "@npart", "%" + nopart + "%" ));
                adapter.Fill(dt);
            }

            if (dt == null) {
                return NotFound("");
            }

            return Ok(dt);

        }



        [HttpGet]
        [Route("getLoteCodNpart/{filter}/{noparte}")]
        public ActionResult<DataTable> getLoteCodNpart([FromRoute] string filter, [FromRoute] string noparte)
        {

            string Sentencia = " Declare @resultado varchar(40)= @fltr " +
                               " declare @nopart varchar(15) = @npr " +
                               " Select a.tipo + a.numero comproba, ltrim(rtrim(a.lote)) lote, " +
                               " cast(a.fecha_tra AS date) fecha_tra, c.no_parte, " +
                               " coalesce(d.nombre, '') nomParte, c.cantidad, c.bodega, " +
                               " coalesce(e.nombre, '') nomBodega, coalesce(a.crtempca_web, '') estado_lote, " +
                               " (case coalesce(a.crtempca_web, '') when 'E' then '#7DB24C' when 'C' then '#E7632A' else '#BC9D4C' end)  colorTable " +
                               " from dp03amov c " +
                               " left join dpinvcab a on a.tipo = c.tipo and a.numero = c.numero " +
                               " inner join dp03acom b on b.codigo = a.tipo and b.controlemp_web = 'M' " +
                               " left join dp03a110 d on d.no_parte = c.no_parte " +
                               " left join dbo.df_alptabla('i_bode') e on e.codigo = c.bodega " +
                               " where coalesce(a.lote,'') != '' and coalesce(a.crtempca_web,'')!= 'T' " +
                               " and(a.lote like @resultado and c.no_parte = @nopart) ";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString)) {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@fltr", "%" + filter + "%"));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@npr", noparte));
                adapter.Fill(dt);
            }

            if (dt == null) {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpGet]
        [Route("getLoteFilter/{filter}/{optionsPA}")]
        public ActionResult<DataTable> getLoteFilter([FromRoute] string filter, [FromRoute] int optionsPA)
        {

            string Sentencia = "exec AR_getLotes @filt, @optionsPA";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@top",          top));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@filt",    filter));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@optionsPA", optionsPA));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }
        
        [HttpGet]
        [Route("getLoteGen/{tp}/{order}")]
        public ActionResult<DataTable> getLoteGen([FromRoute] int tp, [FromRoute] string order)
        {

            string Sentencia = " select top("+tp+") rtrim(ltrim(a.tipo)) + rtrim(ltrim(a.numero)) comproba, " +
                               " rtrim(ltrim(a.lote)) lote, rtrim(ltrim(c.no_parte)) no_parte, " +
	                           " coalesce(a.crtempca_web, '') estado_lote," +
	                           " (case coalesce(a.crtempca_web, '') when 'E' then '#7DB24C' when 'C' then '#E7632A' else '#BC9D4C' end)  colorTable " +
                               " from dp03amov c " +
                               " left join dpinvcab a on a.tipo = c.tipo and a.numero = c.numero " +
                               " left join dp03a110 d on d.no_parte = c.no_parte " +
                               " where coalesce(a.lote, '')      != ''  " +
                               " and coalesce(a.crtempca_web,'') != 'T' " +
                               " order by a.tipo " + order;

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter( "@tp",    tp    ));
                //adapter.SelectCommand.Parameters.Add(new SqlParameter( "@order", order ));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }
        
        [HttpGet]
        [Route("getLotesProdDespacho/{no_parte}")]
        public ActionResult<DataTable> getLotesProdDespacho([FromRoute] string no_parte)
        {

            string Sentencia = "  Declare @no_parte char(15) = @n_parte " + 
                                " select a.tipo + a.numero comprobante, b.no_parte, a.lote,  " +
                                " (b.cantidad - coalesce(b.cantidad2, 0)) disponible, b.bodega  " +
                                "    from dp03amov b left join dpinvcab a on a.tipo = b.tipo and a.numero = b.numero " +
                                " left join dp03a110 c on c.no_parte = b.no_parte " +
                                " inner join dp03acom d on d.codigo = b.tipo and d.esEnvasado = 'S' " +
                                " where(rtrim(ltrim(@no_parte)) = 0 or b.no_parte = @no_parte)";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@n_parte", no_parte));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }


        [HttpGet]
        [Route("getLoteimg/{nparte}")]
        public ActionResult<DataTable> getLoteimg([FromRoute] string nparte) {

            string Sentencia = "select * from img_lote where no_parte_i = @noparte";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@noparte", nparte));
                adapter.Fill(dt);
            }

            if (dt == null) {

                return NotFound("");
            
            }

            return Ok(dt);

        }



        [HttpGet]
        [Route("estadoLote/{data}/{tipo}/{numero}")]
        public ActionResult<DataTable> estadoLote([FromRoute] string data, [FromRoute] string tipo, [FromRoute] string numero)
        {
            // string Sentencia = " update dpinvcab set crtempca_web = @data where tipo = 'EM' and numero = '00000004' ";
         
            string Sentencia = " update dpinvcab set crtempca_web = @data where tipo = @tipo and numero = @numero ";

            //-- E: ESCANEANDO
            //-- T: TERMINADO

          DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);              
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);                          
                adapter.SelectCommand.CommandType = CommandType.Text;                      
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@data",   data));   
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@tipo",   tipo));   
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@numero", numero)); 
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
