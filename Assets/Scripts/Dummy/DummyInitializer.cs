using UnityEngine;
using System.Collections;

using Tanks.UI;

namespace Tanks.Dummy
{
    public class DummyInitializer : Singleton<DummyInitializer>
    {
        void Start()
        {
            HUDManager.Instance.IsCrossHairEnabled = true;
        }
    }
}