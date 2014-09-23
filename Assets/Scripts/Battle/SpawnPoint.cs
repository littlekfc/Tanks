using UnityEngine;
using System.Collections;

using Players;

namespace Battle
{
    public class SpawnPoint : MonoBehaviour
    {
        public Team.TeamID ownerTeamID;

        void Awake()
        {
            BattleManager.Instance.RegisterSpawnPoint(ownerTeamID, this);
        }
    }
}