using UnityEngine;
using System.Collections;

namespace VisualEffects
{
    public class LaserEffect : MonoBehaviour
    {
        public float chargingPeriod = 0.0f;
        public float dechargingPeriod = 0.0f;
        public AnimationCurve chargingCurve;
        public AnimationCurve dechargingCurve;
        public float maxStrength;
        public float maxLightIntensity;

        public float PowerLevel
        {
            get
            {
                if (isCharging)
                    return chargingCurve.Evaluate(ChargingProgress);
                else
                    return dechargingCurve.Evaluate(ChargingProgress);
            }
        }

        private LineRenderer laserLine;
        private Light light;
        private float chargingProgress = 0.0f;
        private bool isCharging = false;

        private float ChargingProgress
        {
            get
            {
                return chargingProgress;
            }
            set
            {
                chargingProgress = Mathf.Clamp01(value);
            }
        }

        // Use this for initialization
        void Awake()
        {
            laserLine = GetComponent<LineRenderer>();
            light = GetComponent<Light>();
        }

        void Update()
        {
            var offset = new Vector2(-Time.time, 0.0f);
            laserLine.materials[1].mainTextureOffset = offset;
        }

        IEnumerator Charge()
        {
            isCharging = true;

            if (chargingPeriod <= 0.0f)
                ChargingProgress = 1.0f;

            while (ChargingProgress < 1.0f)
            {
                ChargingProgress += Time.deltaTime / chargingPeriod;

                SetPowerLevel(PowerLevel);

                yield return null;
            }

            laserLine.SetWidth(maxStrength, maxStrength);

            yield break;
        }

        IEnumerator Decharge()
        {
            isCharging = false;

            if (dechargingPeriod <= 0.0f)
                ChargingProgress = 0.0f;

            while (ChargingProgress > 0.0f)
            {
                ChargingProgress -= Time.deltaTime / dechargingPeriod;

                SetPowerLevel(PowerLevel);

                yield return null;
            }

            laserLine.SetWidth(0.0f, 0.0f);
            gameObject.SetActive(false);

            yield break;
        }

        void SetPowerLevel(float normalised_power_level)
        {
            var current_strength = maxStrength * normalised_power_level;
            laserLine.SetWidth(current_strength, current_strength);

            var current_intensity = maxLightIntensity * normalised_power_level;
            light.intensity = current_intensity;
        }

        public void SetTargetPosition(Vector3 target)
        {
            laserLine.SetPosition(1, target);

            var target_dist = target.sqrMagnitude;
            var wave_length = target_dist / 100f;

            laserLine.materials[1].mainTextureOffset = new Vector2(wave_length, 1.0f);
        }

        public void StartCharging()
        {
            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine("Charge");
        }

        public void StopCharging()
        {
            StopAllCoroutines();
            StartCoroutine("Decharge");
        }
    }
}