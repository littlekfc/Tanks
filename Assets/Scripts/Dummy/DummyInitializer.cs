using UnityEngine;
using System.Collections;

using UI;

namespace Dummy
{
    public class DummyInitializer : Singleton<DummyInitializer>
    {
        void Start()
        {
            HUDManager.Instance.IsCrossHairEnabled = true;
        }
    }
}