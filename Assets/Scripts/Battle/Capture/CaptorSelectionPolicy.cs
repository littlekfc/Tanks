using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public abstract class CaptorSelectionPolicy : TBehaviour, ICaptorSelectionPolicy
    {
        public abstract ICaptor SelectCaptorFrom(IEnumerable<CapturableObject.CaptorList> captorLists);
    }
}