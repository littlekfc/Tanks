using UnityEngine;
using System.Collections;
using System;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public interface ICapturePolicy
    {
        event Action<Team.TeamID> onCaptured;

        void BeginCapturing(ICaptor captor);

        void EndCapturing(ICaptor captor);
    }
}