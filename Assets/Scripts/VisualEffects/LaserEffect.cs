using UnityEngine;
using System.Collections;

namespace VisualEffects
{
    public class LaserEffect : MonoBehaviour
    {
        public float maxStrength;
        public float maxLightIntensity;

        private LineRenderer laserLine;

        // Use this for initialization
        void Awake()
        {
            laserLine = GetComponent<LineRenderer>();
        }

        void Update()
        {
            var offset = new Vector2(-Time.time, 0.0f);
            laserLine.materials[1].mainTextureOffset = offset;
        }

        public void SetPowerLevel(float normalised_power_level)
        {
            var current_strength = maxStrength * normalised_power_level;
            laserLine.SetWidth(current_strength, current_strength);

            var current_intensity = maxLightIntensity * normalised_power_level;
            light.intensity = current_intensity;
        }

        public void SetLength(float length)
        {
            laserLine.SetPosition(1, new Vector3(0.0f, 0.0f, length));

            var wave_length = length / 100f;
            laserLine.materials[1].mainTextureOffset = new Vector2(wave_length, 1.0f);
        }
    }
}