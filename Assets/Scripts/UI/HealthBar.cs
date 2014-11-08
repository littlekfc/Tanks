using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Tanks.Battle;
using Tanks.Attributes;

namespace Tanks.UI
{
    public class HealthBar : Bar
    {
        private Transform MountingPoint { get; set; }

        private void OnHealthChanged(float current_health)
        {
            Value = current_health;
        }

        private void OnOnwerKilled()
        {
            // I know this line is stupid. But such stupid test, Unity will tell you 'Object is already destroyed and you are still trying to access it!'. >_<
            if (this != null && gameObject != null)
                Destroy(gameObject);
        }

        void Update()
        {
            var pos = MountingPoint.position;
            CachedTransform.position = CameraManager.Instance.MainCamera.Camera.WorldToScreenPoint(pos);
        }

        public void Reset(IDestroyable owner, float max_health, float normalized_current_health = 1f, float min_health = 0f)
        {
            MountingPoint = owner.HealthBarMountingPoint;
            owner.onHit += OnHealthChanged;
            owner.onKilled += OnOnwerKilled;

            MaxValue = max_health;
            MinValue = min_health;
            NormalizedValue = normalized_current_health;
        }
    }
}