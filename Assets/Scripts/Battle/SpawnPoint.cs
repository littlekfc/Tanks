using UnityEngine;
using System.Collections;

using Tanks.Players;

namespace Tanks.Battle
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