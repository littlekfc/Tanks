using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Tanks.Players;
using Tanks.Utils;
using Tanks.FogOfWar;
using Tanks.Battle;

namespace Tanks
{
    public abstract class TObject : TBehaviour
    {
        public event Action<Team.TeamID> onOwnerChanged;

        private Team.TeamID owner = Team.TeamID.NONE;

        /// <summary>
        /// the eyesight cycle's radius
        /// </summary>
        public int eyesightRange = 0;

        protected KeyValuePair<int, int> m_preGrid;

        protected virtual void FixedUpdate()
        {
//            if (owner != BattleManager.Instance.MyTeamID)
//                return;
            KeyValuePair<int, int> curGrid = FogOfWarManager.Instance.PosToGrid(transform.position);
            if (!curGrid.Equals(m_preGrid))
            {
                m_preGrid = curGrid;
                if (FogOfWarManager.Instance)
                    FogOfWarManager.Instance.PushMoveObject(this);
            }
        }

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

        virtual protected void OnDestroy()
        {
            if (FogOfWarManager.Instance)
                FogOfWarManager.Instance.PushDestroyObject(this);
        }
    }
}