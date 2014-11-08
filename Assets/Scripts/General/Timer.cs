using UnityEngine;
using System.Collections;
using System;

using Tanks.Utils;

namespace Tanks
{
    /// <summary>
    /// A timer.
    /// </summary>
    public class Timer : TBehaviour
    {
        /// <summary>
        /// Whether the timer ticks repeatly or not.
        /// </summary>
        public bool IsRepeating { get; set; }

        /// <summary>
        /// The interval between two ticks.
        /// </summary>
        public float Interval { get; private set; }

        /// <summary>
        /// The normalised progress towards the next tick.
        /// </summary>
        public float NormalizedProgress { get; private set; }

        /// <summary>
        /// The event that gets fired every time the timer's progress towards the next tick changes with NormalizedProgress as the parameter.
        /// </summary>
        public event Action<float> onProgress;

        /// <summary>
        /// The event that gets fired when the timer ticks.
        /// </summary>
        public event Action onTick;

        /// <summary>
        /// Reset the timer with new settings.
        /// </summary>
        /// <param name="interval">The interval between two ticks</param>
        /// <param name="initial_progress">The initial progress towards the next tick</param>
        public void Reset(float interval, float initial_progress = 0.0f, bool auto_start = false)
        {
            Interval = interval;
            NormalizedProgress = initial_progress;

            enabled = auto_start;
        }

        /// <summary>
        /// Reset the timer with the current settings.
        /// </summary>
        public void Reset(bool auto_start = false)
        {
            NormalizedProgress = 0.0f;
            enabled = auto_start;
        }

        private void Update()
        {
            NormalizedProgress = Mathf.Clamp01(NormalizedProgress + Time.deltaTime / Interval);
            EventUtils.Emit(onProgress, NormalizedProgress);

            if (NormalizedProgress == 1.0f)
            {
                EventUtils.Emit(onTick);

                if (IsRepeating)
                {
                    NormalizedProgress = 0.0f;
                }
                else
                {
                    enabled = false;
                }
            }
        }
    }
}