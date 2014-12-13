using UnityEngine;
using System.Collections;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public interface ICaptor : IAttribute
    {
        Team.TeamID Owner
        {
            get;
        }
    }
}