using cinema_core.DTOs.ClusterDTOs;
using cinema_core.Form;
using cinema_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cinema_core.Repositories
{
    public interface IClusterRepository
    {
        ICollection<ClusterDTO> GetAllClusters(int skip, int limit);

        Cluster GetClusterById(int id);

        Cluster GetClusterByManagerId(int managerId);

        Cluster CreateCluster(ClusterRequest clusterRequest);

        Cluster UpdateCluster(int id, ClusterRequest clusterRequest);

        bool DeleteCluster(Cluster cluster);

        bool Save();
    }
}
