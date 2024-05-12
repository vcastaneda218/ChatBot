using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChatBotWS.Data;
using ChatBotWS.Models.WhatsAppAdmin;
using Microsoft.AspNetCore.Cors;

namespace ChatBotWS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactosController : ControllerBase
    {
         TestContext _context = new TestContext();


        // GET: api/Contactos
        [HttpGet]
        [EnableCors("AllowAny")]
        public async Task<ActionResult<IEnumerable<Contacto>>> GetContactos()
        {
            return await _context.Contactos.ToListAsync();
        }

        // GET: api/Contactos/5
        [HttpGet("{id}")]
        [EnableCors("AllowAny")]
        public async Task<ActionResult<Contacto>> GetContacto(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);

            if (contacto == null)
            {
                return NotFound();
            }

            return contacto;
        }

        // PUT: api/Contactos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [EnableCors("AllowAny")]
        public async Task<IActionResult> PutContacto(int id, Contacto contacto)
        {
            if (id != contacto.ContactoId)
            {
                return BadRequest();
            }

            _context.Entry(contacto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Contactos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableCors("AllowAny")]
        public async Task<ActionResult<Contacto>> PostContacto([FromBody] Contacto contacto)
        {
            _context.Contactos.Add(contacto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContacto", new { id = contacto.ContactoId }, contacto);
        }

        // DELETE: api/Contactos/5
        [HttpDelete("{id}")]
        [EnableCors("AllowAny")]
        public async Task<IActionResult> DeleteContacto(int id)
        {
            var contacto = await _context.Contactos.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }

            _context.Contactos.Remove(contacto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContactoExists(int id)
        {
            return _context.Contactos.Any(e => e.ContactoId == id);
        }
    }
}
