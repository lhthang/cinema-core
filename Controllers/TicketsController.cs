using cinema_core.Form;
using cinema_core.Repositories.Interfaces;
using cinema_core.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
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
                throw e;
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
                throw e;
            }
        }

        // POST: api/tickets
        [HttpPost("[action]")]
        [Authorize]
        public IActionResult BuyTickets([FromBody] TicketRequest ticketRequest)
        {
            if (ticketRequest == null)
                return StatusCode(400, ModelState);
            if (!ModelState.IsValid)
                return StatusCode(400, ModelState);
            var username = Constants.GetUsername(Request);
            try
            {
                var tickets = ticketRepository.BuyTickets(username, ticketRequest);
                return Ok(tickets);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //// POST: api/tickets
        //[HttpPut("{id}")]
        //public IActionResult Put(int id, [FromBody] TicketRequest ticketRequest)
        //{
        //    if (ticketRequest == null)
        //        return StatusCode(400, ModelState);
        //    if (!ModelState.IsValid)
        //        return StatusCode(400, ModelState);

        //    try
        //    {
        //        var ticket = ticketRepository.UpdateTicket(id, ticketRequest);
        //        return Ok(ticket);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

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
                throw e;
            }
        }
    }
}
