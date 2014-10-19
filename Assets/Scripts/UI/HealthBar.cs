using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Battle;
using Attributes;

namespace UI
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