using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
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
        [HttpGet("[action]/{showtimeId}")]
        public IActionResult GetByShowtime(int showtimeId)
        {
            try
            {
                var tickets = ticketRepository.GetAllTicketsByShowtime(showtimeId);
                return Ok(tickets);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // GET: api/tickets/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var ticket = ticketRepository.GetTicketById(id);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // POST: api/tickets
        [HttpPost]
        public IActionResult Post([FromBody] TicketRequest ticketRequest)
        {
            if (ticketRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var ticket = ticketRepository.BuyTicket(ticketRequest);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // POST: api/tickets
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TicketRequest ticketRequest)
        {
            if (ticketRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);

            try
            {
                var ticket = ticketRepository.UpdateTicket(id, ticketRequest);
                return Ok(ticket);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        // DELETE: api/ticket/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var isDeleted = ticketRepository.DeleteTicket(id);
                return Ok(isDeleted);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
    }
}
