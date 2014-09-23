using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Players;

namespace Battle
{
    public class BattleManager : Singleton<BattleManager>
    {
        private IDictionary<Team.TeamID, HashSet<SpawnPoint>> spawnPoints = new Dictionary<Team.TeamID, HashSet<SpawnPoint>>();
        
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
                return null;
        }
    }
}