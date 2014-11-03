using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tanks.Players;

namespace Tanks.Resources
{
    /// <summary>
    /// A class to manage the in-game resource.
    /// </summary>
    public class ResourceManager : Singleton<ResourceManager>
    {
        private IDictionary<Team.TeamID, Resource> teamResourceMap = new Dictionary<Team.TeamID, Resource>();

        public Resource MyResource { get; private set; }

        public void Initialize(Resource initial_resource, IEnumerable<Team.TeamID> teams, Team.TeamID my_team)
        {
            teamResourceMap.Clear();

            foreach (var team in teams)
            {
                if (teamResourceMap.ContainsKey(team))
                {
                    Debug.LogWarning("Trying to initialize the resource twice for a same team!");
                }
                else
                {
                    teamResourceMap.Add(team, new Resource(initial_resource));
                }
            }

            MyResource = ResourceFor(my_team);
        }

        public Resource ResourceFor(Team.TeamID team_id)
        {
            return teamResourceMap[team_id];
        }
    }
}