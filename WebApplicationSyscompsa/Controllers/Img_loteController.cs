using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationSyscompsa.Models;
using WebApplicationSyscompsa.Models.lote_foto;

namespace WebApplicationSyscompsa.Controllers.LOTE_FOTO
{
    [Route("api/img_lote")]
    [ApiController]
    public class Img_loteController : ControllerBase
    {
        private readonly AppDbContext _context;
        public Img_loteController(AppDbContext context)
        {
            this._context = context;
        }

        [HttpPut]
        [Route("put_imge_lote/{pk}")]
        public async Task<IActionResult> put_imge_lote([FromRoute] string pk, [FromBody] Img_lote model)
        {
            if (pk != model.no_parte_i)
            {
                return BadRequest("El ID del producto no es compatible, o no existe");
            }

            _context.Entry(model).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(model);
        }

    }
}
