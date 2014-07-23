using UnityEngine;
using System.Collections;

namespace Weapons
{
    /// <summary>
    /// A base for all the weapons implementing the common parts of IWeapon interface.
    /// </summary>
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        public int ammo = 0;
        public int Ammo
        {
            get
            {
                return ammo;
            }
            set
            {
                ammo = value;
            }
        }

        public float firingRate = 1.0f;
        public float FiringRate
        {
            get { return firingRate; }
        }

        private float remainingCDTime = 0.0f;
        public float RemainingCDTime
        {
            get { return remainingCDTime; }
        }

        public float NormalisedRemainingCDTime
        {
            get { return remainingCDTime * firingRate; }
        }

        public float standardDamage = 100.0f;
        public float StandardDamage
        {
            get { return standardDamage; }
        }

        public abstract void Fire();
    }
}