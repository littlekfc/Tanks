using UnityEngine;
using System.Collections;
using System;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public abstract class CapturePolicy : TBehaviour, ICapturePolicy
    {
        public abstract event Action<Team.TeamID> onCaptured;

        public abstract void CaptureBy(ICaptor captor);
    }
}