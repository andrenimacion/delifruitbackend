using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationSyscompsa.Models;

namespace WebApplicationSyscompsa.Controllers
{
    [Route("api/DevControl")]
    [ApiController]
    public class DevConController : ControllerBase
    {

        private readonly AppDbContext _context;
        public DevConController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("transaccion")]
        public ActionResult<DataTable> transaccion()
        {

            string Sentencia = " select opt='a', codigo, nombre from dp03acom where grupo = 'C' "; 

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
        [Route("ProductosTermin")]
        public ActionResult<DataTable> ProductosTermin() 
        {

            string Sentencia = " declare @fecha datetime = getdate() " + 
                " declare @mes int = month(@fecha) " +
                " select *, ( case  when x.stock <= 0 then '#AE3413' when x.stock <= 100 then '#B87822'  else '#838F2E' end ) color from( " +
                " select opt = 'b', b.no_parte, b.nombre, " +
                " sum( " + 
                "  iif(@mes >= 0,  coalesce(a.saldo00, 0), 0) + iif(@mes >= 1, coalesce(a.saldo01, 0), 0) + iif(@mes >= 2, coalesce(a.saldo02, 0), 0) + " +
                "  iif(@mes >= 3,  coalesce(a.saldo03, 0), 0) + iif(@mes >= 4, coalesce(a.saldo04, 0), 0) + iif(@mes >= 5, coalesce(a.saldo05, 0), 0) + " +
                "  iif(@mes >= 6,  coalesce(a.saldo06, 0), 0) + iif(@mes >= 7, coalesce(a.saldo07, 0), 0) + iif(@mes >= 8, coalesce(a.saldo08, 0), 0) + " +
                "  iif(@mes >= 9,  coalesce(a.saldo09, 0), 0) + iif(@mes >= 10, coalesce(a.saldo10, 0), 0) + iif(@mes >= 11, coalesce(a.saldo11, 0), 0) + " +
                "  iif(@mes >= 12, coalesce(a.saldo12, 0), 0)) stock from dp03a110 b with(nolock) " +
                "  left join dp03asal a with(nolock) on b.no_parte = a.no_parte " +
                "  left join dbo.df_alptabla('i_bode') c on c.codigo = a.bodega " +
                " where tipo = 'N' and tpmaterial = 'T' " +
                " group by b.no_parte, b.nombre " +
                " ) x where x.stock > 0 ";

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
        [Route("ProductosDifTermin/{opts}")]
        public ActionResult<DataTable> ProductosDifTermin( [FromRoute] string opts )
        {

            string Sentencia = " declare @opt varchar(200) = @Opts " +
                               " if (@opt = '%gen%') " +
                               "     begin " +
                               "        select no_parte, nombre from dp03a110  " +
                               "        where tpmaterial != 'T' and tipo = 'N' " +
                               "     end " +
                               " else " +
                               "     begin " +
                               "        select no_parte, nombre from dp03a110 " +
                               "        where tpmaterial != 'T' and tipo = 'N' and nombre like @opt or no_parte like @opt " + 
                               "     end ";
                               
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@Opts", "%" + opts + "%"));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }


        [HttpGet]
        [Route("Consumo")]
        public ActionResult<DataTable> Consumo()
             
        {

            string Sentencia = "select opt='c', * from dp03a130";

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