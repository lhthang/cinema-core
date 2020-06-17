using cinema_core.DTOs.ClusterDTOs;
using cinema_core.Form;
using cinema_core.Models;
using cinema_core.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories.Implements
{
    public class ClusterRepository : IClusterRepository
    {
        private MyDbContext dbContext;

        public ClusterRepository(MyDbContext context) {
            this.dbContext = context;
        }

        public ICollection<ClusterDTO> GetAllClusters(int skip, int limit)
        {
            List<ClusterDTO> results = new List<ClusterDTO>();
            List<Cluster> clusters = dbContext.Clusters
                                        .Include(r => r.Rooms)
                                        .Include(cs => cs.ClusterUser).ThenInclude(u => u.User)
                                        .OrderBy(c => c.Id).Skip(skip).Take(limit)
                                        .ToList();
            foreach (Cluster cluster in clusters)
            {
                results.Add(new ClusterDTO(cluster));
            }
            return results;
        }

        public Cluster GetClusterById(int id)
        {
            Cluster cluster = dbContext.Clusters
                                .Where(c => c.Id == id)
                                .Include(r => r.Rooms)
                                .Include(cs => cs.ClusterUser).ThenInclude(u => u.User)
                                .FirstOrDefault();
            return cluster;
        }

        public Cluster GetClusterByManagerId(int managerId)
        {
            User manager = dbContext.Users
                            .Where(u => u.Id == managerId)
                            .Include(cu => cu.ClusterUser).ThenInclude(c => c.Cluster)
                            .FirstOrDefault();
            if (manager.ClusterUser == null)
            {
                return null;
            }
            //Cluster cluster = dbContext.Clusters
            //                    .Where(c => c.Id == manager.ClusterUser.ClusterId)
            //                    .Include(r => r.Rooms)
            //                    .Include(cs => cs.ClusterUser).ThenInclude(u => u.User)
            //                    .FirstOrDefault();
            return manager.ClusterUser.Cluster;
        }

        public Cluster CreateCluster(ClusterRequest clusterRequest)
        {
            Cluster cluster = new Cluster();
            cluster.Name = clusterRequest.Name;
            cluster.Address = clusterRequest.Address;
            User manager = dbContext.Users.Where(u => u.Id == clusterRequest.ManagerId).FirstOrDefault();
            if (manager != null)
            {
                ClusterUser clusterUser = new ClusterUser()
                {
                    Cluster = cluster,
                    User = manager,
                };
                dbContext.Add(clusterUser);
            }
            dbContext.Add(cluster);
            bool isSuccess = Save();
            if (!isSuccess)
            {
                return null;
            }
            return cluster;
        }

        public Cluster UpdateCluster(int id, ClusterRequest clusterRequest)
        {
            Cluster cluster = dbContext.Clusters.Where(c => c.Id == id).FirstOrDefault();
            List<ClusterUser> clusterUsersToDelete = dbContext.ClusterUsers.Where(cu => cu.ClusterId == id).ToList();
            if (clusterUsersToDelete != null)
            {
                dbContext.RemoveRange(clusterUsersToDelete);
            }
            cluster.Name = clusterRequest.Name;
            cluster.Address = clusterRequest.Address;
            User manager = dbContext.Users.Where(u => u.Id == clusterRequest.ManagerId).FirstOrDefault();
            if (manager != null)
            {
                ClusterUser clusterUser = new ClusterUser()
                {
                    Cluster = cluster,
                    User = manager,
                };
                dbContext.Add(clusterUser);
            }
            dbContext.Update(cluster);
            bool isSuccess = Save();
            if (!isSuccess)
            {
                return null;
            }
            return cluster;
        }

        public bool DeleteCluster(Cluster cluster)
        {
            dbContext.Remove(cluster);
            return Save();
        }

        public bool Save()
        {
            return dbContext.SaveChanges() > 0;
        }
    }
}
