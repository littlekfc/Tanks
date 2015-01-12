using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public class SimpleCaptorSelectionPolicy : CaptorSelectionPolicy
    {
        public override ICaptor SelectCaptorFrom(IEnumerable<CapturableObject.CaptorList> captorLists)
        {
            if (captorLists != null && captorLists.Count() == 1)
            {
                var captor_list = captorLists.Single().captors;
                if (captor_list != null)
                {
                    return captor_list.FirstOrDefault();
                }
            }

            return null;
        }
    }
}