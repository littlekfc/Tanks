using System.Collections;
using UnityEngine;

using Tanks.Attributes;

namespace Tanks.Weapons
{
    /// <summary>
    /// The universe super invincible ultimate explosive bomb 
    /// </summary>
    public class UltimateExplosiveBomb: MonoBehaviour
    {
        /// <summary>
        /// the bullet's flying speed
        /// </summary>
        public float flyingSpeed = 100.0f;

        /// <summary>
        /// the bullet's life time
        /// </summary>
        public float lifeTime = 2.0f;

        public GameObject bombEffect;

        public Weapon bulletGun;

        private Object m_bombEffectIns;

        private bool m_hasBomb = false;

        void Start()
        {
            rigidbody.velocity = transform.forward * flyingSpeed;
        }

        protected virtual void Update()
        {
            if (lifeTime > 0.0f)
                lifeTime -= Time.deltaTime;
            else
                Bomb();
        }

        void OnCollisionEnter(Collision collisionInfo)
        {
            Bomb();
            TObject o = collisionInfo.collider.GetComponent<TObject>();
            if (o is IDestroyable && bulletGun)
            {
                 (o as IDestroyable).Hit(bulletGun.StandardDamage);
            }
        }

        IEnumerator DestroyEffect()
        {
            GetComponent<MeshRenderer>().enabled = false;
            yield return new WaitForSeconds(1);
            Destroy(m_bombEffectIns);
            Destroy(gameObject);
        }

        void Bomb()
        {
            if (!m_hasBomb)
            {
                m_bombEffectIns = Instantiate(bombEffect, gameObject.transform.position, gameObject.transform.rotation);
                StartCoroutine(DestroyEffect());
                m_hasBomb = true;
            }
        }
    }
}