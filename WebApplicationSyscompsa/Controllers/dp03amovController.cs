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

namespace WebApplicationSyscompsa.Controllers.EXPOFIDEL_CONTROLLERS
{

    //Control de productos despachados - vendidos - empacados
    [Route("api/controlprod")]
    [ApiController]
    public class dp03amovController : ControllerBase
    {

        private readonly AppDbContext _context;

        public dp03amovController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("getLoteStockProducts/{filter}/{npart}/{tp}")]
        public ActionResult<DataTable> getLoteStockProducts( [FromRoute] string filter, [FromRoute] string npart, [FromRoute] int tp)
        {

            string Sentencia = "exec AR_getLoteStockEmpaque @Filt, @Npart, @Tp";
            //exec AR_getLoteStockEmpaque '%EM00001900%', '%086%'

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Filt",  "%" + filter + "%" ));
                adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Npart", "%" + npart  + "%" ));
                adapter.SelectCommand.Parameters.Add(new SqlParameter( "@Tp", tp ));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("No se encontro: " + filter);
            }
            return Ok(dt);

        }

        [HttpGet]
        [Route("getFACTS/{opt}/{tipo}/{num}/{grupo}")]
        public ActionResult<DataTable> getFACTS([FromRoute] string opt,
                                                [FromRoute] string tipo,
                                                [FromRoute] string num,
                                                [FromRoute] string grupo)
        {

            string Sentencia = "exec AR_controlprod @opt, @tipo, @num, @grupo";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {

                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@opt", opt));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@tipo", tipo));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@num", num));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@grupo", grupo));
                adapter.Fill(dt);

            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }


        [HttpGet]
        [Route("getFactsType/{type}/{top}")]
        public ActionResult<DataTable> getFactsType([FromRoute] string type, [FromRoute] string top)
        {

            string Sentencia = "Select top " + top + " a.tipo+a.numero comproba, b.empcli nomCliente " +
                                " from dpinvcab a with(nolock) " +
                                " left " +
                                " join dp05a110 b with(nolock) on b.codigo = a.prov_cli " +
                                " where grupo = 'V' and coalesce(a.referencia, '')='' " +
                                " and( a.tipo + a.numero like @type " +
                                " or b.empcli like @type) " +
                                " order by a.fecha_tra desc, b.empcli, a.tipo, a.numero";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", "%" + type + "%"));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }
            return Ok(dt);

        }

        [HttpGet]
        [Route("getFactsGen/{type}")]
        public ActionResult<DataTable> getFactsTypeGen([FromRoute] string type)
        {

            string Sentencia = "Select a.tipo+a.numero comproba, b.empcli nomCliente " +
                                " from dpinvcab a with(nolock) " +
                                " left " +
                                " join dp05a110 b with(nolock) on b.codigo = a.prov_cli " +
                                " where grupo = 'V' " +
                                " and( a.tipo + a.numero like @type " +
                                " or b.empcli like @type) " +
                                " order by a.fecha_tra desc, b.empcli, a.tipo, a.numero";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@type", "%" + type + "%"));
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
