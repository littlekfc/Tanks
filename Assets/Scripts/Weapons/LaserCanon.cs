using UnityEngine;
using System.Collections;

using VisualEffects;
using Attributes;

namespace Weapons
{
    public class LaserCanon : Weapon
    {
        public float chargingPeriod = 0.0f;
        public float dechargingPeriod = 0.0f;
        public AnimationCurve chargingCurve;
        public AnimationCurve dechargingCurve;

        public Transform gunTip;

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

        private float chargingProgress = 0.0f;
        public float ChargingProgress
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

        private bool isCharging = false;

        private float gunTipLaserOffset = 0.0f;

        private LaserEffect laser;
        protected LaserEffect Laser
        {
            get
            {
                if (laser == null)
                    laser = GetComponentInChildren<LaserEffect>();

                return laser;
            }
        }

        protected float InstantDamage
        {
            get
            {
                return StandardDamage * PowerLevel;
            }
        }

        void Awake()
        {
            Laser.gameObject.SetActive(false);

            if (gunTip == null)
            {
                gunTip = transform;
            }

            gunTipLaserOffset = Vector3.Distance(gunTip.position, Laser.transform.position);
        }

        void FixedUpdate()
        {
            if (Laser.gameObject.activeInHierarchy)
            {
                Ray ray = new Ray(gunTip.position, gunTip.forward);
                RaycastHit hit;
                float laser_length = Range;

                if (Physics.Raycast(ray, out hit, Range, HitMask))
                {
                    laser_length = hit.distance;

                    TObject o = hit.collider.GetComponent<TObject>();
                    if (o is IDestroyable && IsCooledDown)
                    {
                        (o as IDestroyable).Hit(InstantDamage);
                        ResetCoolDownTimer();
                    }
                }

                laser_length -= gunTipLaserOffset;
                Laser.SetLength(laser_length);
            }
        }

        IEnumerator Charge()
        {
            isCharging = true;
            Laser.gameObject.SetActive(true);

            if (chargingPeriod <= 0.0f)
                ChargingProgress = 1.0f;

            while (ChargingProgress < 1.0f)
            {
                ChargingProgress += Time.deltaTime / chargingPeriod;

                Laser.SetPowerLevel(PowerLevel);

                yield return null;
            }

            Laser.SetPowerLevel(1.0f);

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

                Laser.SetPowerLevel(PowerLevel);

                yield return null;
            }

            Laser.SetPowerLevel(0.0f);
            Laser.gameObject.SetActive(false);

            yield break;
        }

        public override bool IsOneShot
        {
            get { return false; }
        }

        public override void Fire()
        {
            StopAllCoroutines();
            StartCoroutine("Charge");
        }

        public override void CeaseFire()
        {
            StopAllCoroutines();
            StartCoroutine("Decharge");
        }
    }
}