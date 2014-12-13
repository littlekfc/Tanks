using UnityEngine;
using System.Collections;
using System;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public abstract class CapturePolicy : ICapturePolicy
    {
        public event Action<Team.TeamID> onCaptured;

        public void BeginCapturing(ICaptor captor)
        {
            throw new System.NotImplementedException();
        }

        public void EndCapturing(ICaptor captor)
        {
            throw new System.NotImplementedException();
        }

        protected abstract bool Capture(ICaptor captor);
    }
}