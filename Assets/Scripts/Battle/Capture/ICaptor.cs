using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public interface ICaptor : IAttribute
    {
        Team.TeamID Owner
        {
            get;
        }

        ICollection<ICapturable> CapturingObjects
        {
            get;
        }
    }
}