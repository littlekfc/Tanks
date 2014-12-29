using UnityEngine;
using System.Collections;
using System;

namespace Tanks.Battle.Capture
{
    public interface ICapturable : IAttribute
    {
        ICapturePolicy CapturePolicy { get; set; }
        ICaptorSelectionPolicy CaptorSelectionPolicy { get; set; }

        void BeginCapturing(ICaptor captor);

        void EndCapturing(ICaptor captor);
    }
}