using cinema_core.DTOs.ClusterDTOs;
using cinema_core.Form;
using cinema_core.Repositories;
using cinema_core.Utils;
using cinema_core.Utils.ErrorHandle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Controllers
{
    [Route("api/clusters")]
    [ApiController]
    public class ClustersController : Controller
    {
        private IClusterRepository clusterRepository;
        private IUserRepository userRepository;
        private IRoomRepository roomRepository;

        public ClustersController(IClusterRepository clusterRepository, IUserRepository userRepository, IRoomRepository roomRepository)
        {
            this.clusterRepository = clusterRepository;
            this.userRepository = userRepository;
            this.roomRepository = roomRepository;
        }

        //GET: api/clusters
        [HttpGet]
        public IActionResult Get(int skip = 0, int limit = 100000)
        {
            if (limit <= 0)
            {
                var error = new Error() { message = "Limit must be greater than 0" };
                return StatusCode(400, error);
            }
            var clusters = clusterRepository.GetAllClusters(skip, limit);
            return Ok(clusters);
        }

        // GET: api/clusters/5
        [HttpGet("{id}", Name = "GetCluster")]
        public IActionResult Get(int id)
        {
            var cluster = clusterRepository.GetClusterById(id);
            if (cluster == null)
            {
                return NotFound();
            }
            var clusterDTO = new ClusterDTO(cluster);
            return Ok(clusterDTO);
        }

        // POST: api/clusters
        [HttpPost]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Post([FromBody] ClusterRequest clusterRequest)
        {
            if (clusterRequest == null) 
            {
                return StatusCode(400, ModelState);
            }

            var statusCode = ValidateCluster(clusterRequest, true);


            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            //var room = roomRepository.CreateRoom(roomRequest);
            var cluster = clusterRepository.CreateCluster(clusterRequest);
            if (cluster == null)
            {
                var error = new Error() { message = "Cluster went oopsie when creating" };
                return StatusCode(400, error);
            }
            return RedirectToRoute("GetCluster", new { id = cluster.Id });
        }

        // POST: api/clusters
        [HttpPut("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Put(int id, [FromBody] ClusterRequest clusterRequest)
        {
            if (clusterRepository.GetClusterById(id) == null)
            {
                return NotFound();
            }

            if (clusterRequest == null)
            {
                return StatusCode(400, ModelState);
            }

            var statusCode = ValidateCluster(clusterRequest, false);

            if (!ModelState.IsValid)
            {
                return StatusCode(statusCode.StatusCode);
            }

            var cluster = clusterRepository.UpdateCluster(id, clusterRequest);
            if (cluster == null)
            {
                var error = new Error() { message = "Cluster went oopsie when updating" };
                return StatusCode(400, error);
            }
            return Ok(new ClusterDTO(cluster));
        }

        // DELETE: api/clusters/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Authorize.Admin)]
        public IActionResult Delete(int id)
        {
            var cluster = clusterRepository.GetClusterById(id);
            if (cluster == null)
            {
                return NotFound();
            }

            if (!clusterRepository.DeleteCluster(cluster))
            {
                var error = new Error() { message = "Cluster went oopsie when deleting" };
                return StatusCode(400, error);
            }
            return Ok(cluster);
        }

        private StatusCodeResult ValidateCluster(ClusterRequest clusterRequest, bool fromCreating)
        {
            if (clusterRequest == null || !ModelState.IsValid) return BadRequest();

            int? managerId = clusterRequest.ManagerId;

            if (managerId != null)
            {
                if (userRepository.GetUserById(managerId.GetValueOrDefault()) == null)
                {
                    ModelState.AddModelError("", $"UserId {managerId} not found");
                    return StatusCode(404);
                }
                else if (fromCreating && clusterRepository.GetClusterByManagerId(managerId.GetValueOrDefault()) != null)
                {
                    ModelState.AddModelError("", $"UserId {managerId} is already associated with another cluster");
                    return StatusCode(400);
                }
            }

            return NoContent();
        }
    }
}
