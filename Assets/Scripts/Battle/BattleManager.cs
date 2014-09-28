using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Photon;

using Players;

namespace Battle
{
    public class BattleManager : Singleton<BattleManager>
    {
        private IDictionary<Team.TeamID, HashSet<SpawnPoint>> spawnPoints = new Dictionary<Team.TeamID, HashSet<SpawnPoint>>();
        
        // The following field is subject to be refactored.
        public GameObject vehiclePrefab;

        // The following property is subject to be refactored.
        public Team.TeamID MyTeamID { get; set; }

        public bool RegisterSpawnPoint(Team.TeamID team_id, SpawnPoint spawn_point)
        {
            if (!spawnPoints.ContainsKey(team_id))
                spawnPoints.Add(team_id, new HashSet<SpawnPoint>());

            return spawnPoints[team_id].Add(spawn_point);
        }

        public IEnumerable<SpawnPoint> GetSpawnPointsFor(Team.TeamID team_id)
        {
            if (spawnPoints.ContainsKey(team_id))
                return spawnPoints[team_id];
            else
                return new List<SpawnPoint>();
        }

        void Start()
        {
            var my_spawn_points = GetSpawnPointsFor(MyTeamID).GetEnumerator();
            if (my_spawn_points.MoveNext())
            {
                var spawn_point = my_spawn_points.Current;

                PhotonNetwork.Instantiate(vehiclePrefab.name, spawn_point.transform.position, spawn_point.transform.rotation, 0);
            }
        }
    }
}