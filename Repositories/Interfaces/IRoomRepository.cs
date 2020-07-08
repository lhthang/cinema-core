using cinema_core.DTOs.RoomDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories
{
    public interface IRoomRepository
    {
        ICollection<RoomDTO> GetAllRooms(int skip, int limit,int clusterId);
        Room GetRoomById(int id);

        ICollection<RoomDTO> GetRoomsByClusterId(int clusterId);

        Room CreateRoom(RoomRequest roomRequest);

        Room UpdateRoom(int id,RoomRequest roomRequest);

        bool DeleteRoom(Room room);

        bool Save();
    }
}
