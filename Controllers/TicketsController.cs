using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Error;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Controllers
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : Controller
    {
        private ITicketRepository ticketRepository;

        public TicketsController(ITicketRepository repository)
        {
            ticketRepository = repository;
        }
        // GET: api/tickets/showtime/1
        [HttpGet("showtime/{showtimeId}")]
        public IActionResult GetByShowtime(int showtimeId)
        {
            // Temp. TODO: showtimeId => int
            var tickets = ticketRepository.GetAllTicketsByShowtime(showtimeId);
            return Ok(tickets);
        }

        private IActionResult StatusCode(int v, Error error)
        {
            throw new Exception(error.Message);
        }

        // GET: api/tickets/5
        [HttpGet("{id}", Name = "GetTicket")]
        public IActionResult Get(int id)
        {
            var ticket = ticketRepository.GetTicketById(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }

        // POST: api/tickets
        [HttpPost]
        public IActionResult Post([FromBody] TicketRequest ticketRequest)
        {
            if (ticketRequest == null) return StatusCode(400, ModelState);

            var statusCode = ValidateTicket(ticketRequest);


            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            var ticket = ticketRepository.BuyTicket(ticketRequest);
            if (ticket == null)
            {
                var error = new Error() { Message = "Something went wrong when save ticket" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetTicket", new { id = ticket.Id });
        }

        //// POST: api/tickets
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] TicketRequest ticketRequest)
        //{
        //    if (ticketRequest == null) return StatusCode(400, ModelState);

        //    var statusCode = ValidateTicket(ticketRequest);

        //    if (!ModelState.IsValid)
        //        return StatusCode(statusCode.StatusCode);

        //    var ticket = ticketRepository.UpdateTicket(id, ticketRequest);
        //    if (ticket == null)
        //    {
        //        var error = new Error() { Message = "Something went wrong when save ticket" };
        //        return StatusCode(400, error);
        //    }
        //    return RedirectToRoute("GetTicket", new { id = ticket.Id });
        //}

        // DELETE: api/ticket/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //var isExist = ticketRepository.GetTicketById(id);
            //if (isExist == null) return NotFound();

            if (!ticketRepository.DeleteTicket(id))
            {
                var error = new Error() { Message = "Something went wrong when delete ticket" };
                return StatusCode(400, error);
            }
            return Ok(id);
        }

        private StatusCodeResult ValidateTicket(TicketRequest ticketRequest)
        {
            if (ticketRequest == null || !ModelState.IsValid) return BadRequest();

            return NoContent();
        }
    }
}
