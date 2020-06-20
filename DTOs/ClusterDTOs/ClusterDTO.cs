using cinema_core.DTOs.RoomDTOs;
using cinema_core.DTOs.UserDTOs;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.DTOs.ClusterDTOs
{
    public class ClusterDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<RoomDTO> Rooms { get; set; }
        public UserDTO Manager { get; set; }

        public ClusterDTO() { }

        public ClusterDTO(Cluster cluster) {
            if (cluster == null)
                return;

            this.Id = cluster.Id;
            this.Name = cluster.Name;
            this.Address = cluster.Address;
            if (cluster.ClusterUser != null)
            {
                this.Manager = new UserDTO()
                {
                    Id = cluster.ClusterUser.User.Id,
                    FullName = cluster.ClusterUser.User.FullName,
                    Username = cluster.ClusterUser.User.Username,
                };
            }
            List<RoomDTO> roomList = new List<RoomDTO>();
            if (cluster.Rooms != null) {
                foreach (Room room in cluster.Rooms) {
                    roomList.Add(new RoomDTO(room));
                }
            }
            this.Rooms = roomList;
        }
    }
}
