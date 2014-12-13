using UnityEngine;
using System.Collections;

using Tanks.VisualEffects;
using Tanks.Battle;

namespace Tanks.Weapons
{
    /// <summary>
    /// The laser canon!
    /// </summary>
    public class LaserCanon : Weapon
    {
        /// <summary>
        /// Time needed for charging the laser canon to its full power state from 0 power state.
        /// </summary>
        public float chargingPeriod = 0.0f;

        /// <summary>
        /// Time needed for decharging the laser canon to its 0 power state from full power state.
        /// </summary>
        public float dechargingPeriod = 0.0f;

        /// <summary>
        /// Describe how the laser canon's power level changes during charging.
        /// </summary>
        public AnimationCurve chargingCurve;

        /// <summary>
        /// Describe how the laser canon's power level changes during decharging.
        /// </summary>
        public AnimationCurve dechargingCurve;

        /// <summary>
        /// Where the laser is starting from.
        /// </summary>
        public Transform gunTip;

        /// <summary>
        /// A weak nomalised value describes current power level of the laser canon.
        /// </summary>
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

        /// <summary>
        /// A strong normalised value describes the charging progress.
        /// </summary>
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

        /// <summary>
        /// Whether the laser canon is charging or not.
        /// </summary>
        private bool isCharging = false;

        /// <summary>
        /// For fixing the laser length when the gun tip position differs from the laser effect object position.
        /// </summary>
        private float gunTipLaserOffset = 0.0f;

        /// <summary>
        /// The laser effect!
        /// </summary>
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

        /// <summary>
        /// The damage of the laser canon at the given moment.
        /// </summary>
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