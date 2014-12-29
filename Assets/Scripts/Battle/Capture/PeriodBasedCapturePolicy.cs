using UnityEngine;
using System.Collections;
using System;

using Tanks.Players;
using Tanks.Utils;

namespace Tanks.Battle.Capture
{
    public class PeriodBasedCapturePolicy : CapturePolicy
    {
        public float capturePeriod = 0.0f;
        public float captureProgress = 0.0f;

        public override event Action<Team.TeamID> onCaptured;

        private Team.TeamID currentCaptorTeamId = Team.TeamID.NONE;

        public override void CaptureBy(ICaptor captor)
        {
            if (captor.Owner == currentCaptorTeamId)
            {
                captureProgress = Mathf.Clamp01(captureProgress + Time.deltaTime / capturePeriod);
                if (captureProgress == 1)
                {
                    EventUtils.Emit(onCaptured, captor.Owner);
                }
            }
            else
            {
                captureProgress = Mathf.Clamp01(captureProgress - Time.deltaTime / capturePeriod);
                if (captureProgress == 0)
                {
                    currentCaptorTeamId = captor.Owner;
                }
            }
        }
    }
}