﻿using UnityEngine;
using System.Collections;

namespace Tanks.Dummy
{
    public class ResourceGeneratorTest : TBehaviour
    {
        public Battle.ResourceGenerator generator;

        private void Start()
        {
            generator.Owner = Battle.BattleManager.Instance.MyTeamID;
        }
    }
}