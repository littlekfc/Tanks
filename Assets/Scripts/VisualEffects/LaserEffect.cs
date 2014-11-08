using UnityEngine;
using System.Collections;

namespace Tanks.VisualEffects
{
    public class LaserEffect : MonoBehaviour
    {
        public float maxStrength;
        public float maxLightIntensity;
        public float maxHitEffectLightIntensity;
        public float maxHitEffectScale = 4.0f;

        private LineRenderer laserLine;
        private LineRenderer LaserLine
        {
            get
            {
                if (laserLine == null)
                    laserLine = GetComponent<LineRenderer>();

                return laserLine;
            }
        }

        private Transform hitEffect;
        private Transform HitEffect
        {
            get
            {
                if (hitEffect == null)
                    hitEffect = transform.FindChild("HitEffect");

                return hitEffect;
            }
        }

        void Update()
        {
            var offset = new Vector2(-Time.time, 0.0f);
            LaserLine.materials[1].mainTextureOffset = offset;
        }

        public void SetPowerLevel(float normalised_power_level)
        {
            var current_strength = maxStrength * normalised_power_level;
            LaserLine.SetWidth(current_strength, current_strength);

            var current_intensity = maxLightIntensity * normalised_power_level;
            light.intensity = current_intensity;

            var current_hit_intensity = maxHitEffectLightIntensity * normalised_power_level;
            HitEffect.light.intensity = current_hit_intensity;

            var current_hit_scale = maxHitEffectScale * normalised_power_level;
            HitEffect.localScale = new Vector3(current_hit_scale, current_hit_scale, current_hit_scale);
        }

        public void SetLength(float length)
        {
            LaserLine.SetPosition(1, new Vector3(0.0f, 0.0f, length));
            HitEffect.localPosition = new Vector3(0.0f, 0.0f, length);

            var wave_length = length / 100f;
            LaserLine.materials[1].mainTextureScale = new Vector2(wave_length, 1.0f);
        }
    }
}