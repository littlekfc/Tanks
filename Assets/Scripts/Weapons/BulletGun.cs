using UnityEngine;

using Tanks.Attributes;

namespace Tanks.Weapons
{
    public class BulletGun: Weapon
    {
        public GameObject Bullet;

        public GameObject BulletTip;

        public override bool IsOneShot
        {
            get { return true; }
        }

        public override void Fire()
        {
            if (IsCooledDown)
            {
                GameObject obj = Instantiate(Bullet, BulletTip.transform.position, BulletTip.transform.rotation) as GameObject;
                obj.GetComponent<UltimateExplosiveBomb>().bulletGun = this;
                ResetCoolDownTimer();
            }
        }
    }
}
