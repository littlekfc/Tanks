using UnityEngine;
using System.Collections;
using System;

using Tanks.Players;
using Tanks.Utils;

namespace Tanks
{
    public abstract class TObject : TBehaviour
    {
        public event Action<Team.TeamID> onOwnerChanged;

        private Team.TeamID owner = Team.TeamID.NONE;
        public Team.TeamID Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
                EventUtils.Emit(onOwnerChanged, owner);
            }
        }
    }
}