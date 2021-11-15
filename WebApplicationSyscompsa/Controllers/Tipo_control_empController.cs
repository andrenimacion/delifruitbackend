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
using WebApplicationSyscompsa.Models.traspase_product;

namespace WebApplicationSyscompsa.Controllers.Tipo_control_emp
{
    [Route("api/tcontrolemp")]
    [ApiController]
    public class Tipo_control_empController : ControllerBase
    {
        private readonly AppDbContext _context;
        public Tipo_control_empController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Route("getTipoControlEmp")]
        public ActionResult<DataTable> getTipoControlEmp() 
        {

            string Sentencia = "select id, coalesce(tipo, '---') tipo, coalesce(consumo, '---') consumo from tipo_control_emp";

            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using SqlCommand cmd = new SqlCommand(Sentencia, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.SelectCommand.CommandType = CommandType.Text;
                //adapter.SelectCommand.Parameters.Add(new SqlParameter("@tipo", tipo));
                adapter.Fill(dt);
            }

            if (dt == null)
            {
                return NotFound("");
            }

            return Ok(dt);

        }

        [HttpPut]
        [Route("putTipoControlEmp/{Id}")]
        public async Task<IActionResult> puttransprod([FromRoute] int Id, [FromBody] Tipo_control_emps model)
        {

            if (Id != model.id) { return BadRequest("El ID del producto no es compatible, o no existe"); }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);

        }

        //NO DESCARTAMOS LA IDEA QUE LO PUEDA CREAR Y ELIMINAR POR AHORA SE MANEJARA CON UPDATE
        //[HttpPost]
        //[Route("saveTipoControlEmp")]
        //public async Task<IActionResult> saveTipoControlEmp([FromBody] Traspase_product model)
        //{

        //    if (ModelState.IsValid) {

        //        _context.traspase_product.Add(model);

        //        if (await _context.SaveChangesAsync() > 0) {
        //            return Ok(model);
        //        }

        //        else {
        //            return BadRequest("Datos incorrectos");
        //        }

        //    }

        //    else {
        //        return BadRequest(ModelState);
        //    }

        //}

        //[HttpGet]
        //[Route("DelTipoControlEmp/{tipo}")]
        //public ActionResult<DataTable> DelTipoControlEmp([FromRoute] string tipo)
        //{

        //    string Sentencia = "delete from tipo_control_emp where tipo = @tipo";

        //    DataTable dt = new DataTable();
        //    using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        //    {
        //        using SqlCommand cmd = new SqlCommand(Sentencia, connection);
        //        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //        adapter.SelectCommand.CommandType = CommandType.Text;
        //        adapter.SelectCommand.Parameters.Add( new SqlParameter("@tipo", tipo) );
        //        adapter.Fill(dt);
        //    }

        //    if (dt == null)
        //    {
        //        return NotFound("");
        //    }

        //    return Ok(dt);

        //}


    }
}
