using cinema_core.DTOs.RoomDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using cinema_core.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class RoomRepository : IRoomRepository
    {
        private MyDbContext dbContext;

        public RoomRepository(MyDbContext context)
        {
            dbContext = context;
        }

        public Room CreateRoom(RoomRequest roomRequest)
        {
            var room = new Room();
            Coppier<RoomRequest, Room>.Copy(roomRequest, room);
            room.Cluster = dbContext.Clusters.Where(c => c.Id == roomRequest.ClusterId).FirstOrDefault();
            var screenTypes = dbContext.ScreenTypes.Where(s => roomRequest.ScreenTypeIds.Contains(s.Id)).ToList();
            foreach (var screen in screenTypes)
            {
                var roomScreenType = new RoomScreenType()
                {
                    ScreenType = screen,
                    Room = room,
                };
                dbContext.Add(roomScreenType);
            }
            dbContext.Add(room);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return room;
        }

        public bool DeleteRoom(Room room)
        {
            dbContext.Remove(room);
            return Save();
        }

        public ICollection<RoomDTO> GetAllRooms(int skip,int limit, int clusterId)
        {
            List<RoomDTO> results = new List<RoomDTO>();
            List<Room> rooms = new List<Room>();

            var cluster = dbContext.Clusters.Where(c => c.Id == clusterId).FirstOrDefault();
            rooms = dbContext.Rooms
                                .Include(rs => rs.RoomScreenTypes).ThenInclude(s => s.ScreenType)
                                .Include(c => c.Cluster)
                                .OrderBy(r => r.Id).Skip(skip).Take(limit).ToList();
            if (cluster!=null)
            {
                rooms = rooms.Where(r => r.ClusterId == clusterId).ToList();
            }
            foreach (Room room in rooms)
            {
                //System.Diagnostics.Debug.WriteLine();
                results.Add(new RoomDTO(room));
            }
            return results;
        }

        public ICollection<RoomDTO> GetRoomsByClusterId(int clusterId)
        {
            List<RoomDTO> results = new List<RoomDTO>();
            var cluster = dbContext.Clusters
                                .Where(c => c.Id == clusterId)
                                .Include(c => c.Rooms).ThenInclude(r => r.RoomScreenTypes).ThenInclude(s => s.ScreenType)
                                .FirstOrDefault();
            if (cluster != null)
            {
                foreach (Room room in cluster.Rooms)
                {
                    results.Add(new RoomDTO(room));
                }
            }
            return results;
        }

        public Room GetRoomById(int id)
        {
            var room = dbContext.Rooms
                            .Where(r => r.Id == id)
                            .Include(rs => rs.RoomScreenTypes).ThenInclude(s => s.ScreenType)
                            .Include(c => c.Cluster)
                            .FirstOrDefault();
            return room;
        }

        public bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }

        public Room UpdateRoom(int id, RoomRequest roomRequest)
        {
            var room = dbContext.Rooms.Where(r => r.Id == id).FirstOrDefault();

            var screenTypesIsDelete = dbContext.RoomScreenTypes.Where(rs => rs.RoomId == id).ToList();

            if (screenTypesIsDelete != null)
                dbContext.RemoveRange(screenTypesIsDelete);

            Coppier<RoomRequest, Room>.Copy(roomRequest, room);

            room.Cluster = dbContext.Clusters.Where(c => c.Id == roomRequest.ClusterId).FirstOrDefault();

            var screenTypes = dbContext.ScreenTypes.Where(s => roomRequest.ScreenTypeIds.Contains(s.Id)).ToList();
            foreach (var screen in screenTypes)
            {
                var roomScreenType = new RoomScreenType()
                {
                    ScreenType = screen,
                    Room = room,
                };
                dbContext.Add(roomScreenType);
            }
            dbContext.Update(room);
            var isSuccess = Save();
            if (!isSuccess) return null;
            return room;

        }
    }
}
