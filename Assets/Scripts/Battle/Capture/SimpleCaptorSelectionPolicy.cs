using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public class SimpleCaptorSelectionPolicy : CaptorSelectionPolicy
    {
        public override ICaptor SelectCaptorFrom(IDictionary<Team.TeamID, ICollection<ICaptor>> captors)
        {
            if (captors != null && captors.Count == 1)
            {
                var captor_list = captors.Single().Value;
                if (captor_list != null)
                {
                    return captor_list.FirstOrDefault();
                }
            }

            return null;
        }
    }
}