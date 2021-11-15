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
    [Route("api/devcont")]
    [ApiController]
    public class cdevController : ControllerBase
    {
        private readonly AppDbContext _context;
        public cdevController(AppDbContext context)
        {
            this._context = context;
        }

        //DP03ACOM GET
        [HttpGet]
        [Route("getDep03acom/{grupo}")]
        public ActionResult<DataTable> getDep03acom([FromRoute] string grupo)
        {
            string Sentencia = "select codigo,nombre from dp03acom where grupo=@grup";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@grup", grupo));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        //PRODUCTOS TERMINADOS DP03A110
        [HttpGet]
        [Route("prodTerminate/{estado}/{mat}")]
        public ActionResult<DataTable> prodTerminate([FromRoute] string estado, [FromRoute] string tpmaterial)
        {
            // estado = A, tpmaterial = T
            string Sentencia = "select no_parte, nombre from dp03a100 where estado= @st and tpmaterial=@tpm";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@st", estado));
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@tpm", tpmaterial));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        // consumo
        [HttpGet]
        [Route("consumo")]
        public ActionResult<DataTable> consumo()
        {

            string Sentencia = " select codigo, nombre from dp03a130";

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
