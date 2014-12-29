﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Tanks.Players;

namespace Tanks.Battle.Capture
{
    public interface ICaptorSelectionPolicy
    {
        ICaptor SelectCaptorFrom(IDictionary<Team.TeamID, ICollection<ICaptor>> captors);
    }
}