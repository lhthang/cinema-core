using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cinema_core.DTOs.RoomDTOs;
using cinema_core.Form.Room;
using cinema_core.Models.Room;
using cinema_core.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cinema_core.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomsController : Controller
    {
        private IRoomRepository roomRepository;

        private IScreenTypeRepository screenTypeRepository;

        public RoomsController(IRoomRepository repository,IScreenTypeRepository screenTypeRepository)
        {
            roomRepository = repository;
            this.screenTypeRepository = screenTypeRepository;
        }
        // GET: api/rooms
        [HttpGet]
        public IActionResult Get()
        {
            var rooms = roomRepository.GetAllRooms();
            return Ok(rooms);
        }

        // GET: api/rooms/5
        [HttpGet("{id}", Name = "GetRoom")]
        public IActionResult Get(int id)
        {
            var room = roomRepository.GetRoomById(id);
            if (room == null)
                return NotFound();
            var roomDTO = new RoomDTO(room);
            return Ok(roomDTO);
        }

        // POST: api/rooms
        [HttpPost]
        public IActionResult Post([FromBody] RoomRequest roomRequest)
        {
            if (roomRequest == null) return StatusCode(400, ModelState);

            var statusCode = ValidateRoom(roomRequest);


            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);

            var room = roomRepository.CreateRoom(roomRequest);
            if (room == null)
            {
                ModelState.AddModelError("", "Something went wrong when save room");
                return StatusCode(400, ModelState);
            }
            return CreatedAtRoute("GetRoom", new { id = room.Id }, new RoomDTO(room));
        }

        private StatusCodeResult ValidateRoom(RoomRequest roomRequest)
        {
            if (roomRequest == null || !ModelState.IsValid) return BadRequest();

            foreach(var id in roomRequest.ScreenTypeIds)
            {
                if (screenTypeRepository.GetScreenTypeById(id) == null)
                {
                    ModelState.AddModelError("", $"Id {id} not found");
                    return StatusCode(404);
                }
            }
            return NoContent();
        }
    }
}