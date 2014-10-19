using UnityEngine;
using System.Collections;

using Attributes;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        public Transform crosshairCursor;
        public HealthBar healthBar;

        private Transform uiRoot = null;
        private Transform UIRoot
        {
            get
            {
                if (uiRoot == null)
                    uiRoot = GetComponentInChildren<Canvas>().transform;

                return uiRoot;
            }
        }

        private Transform Cursor
        {
            get
            {
                return crosshairCursor;
            }
        }

        private bool IsCursorActive
        {
            get
            {
                return Cursor.gameObject.activeSelf;
            }
        }

        public bool IsCrossHairEnabled
        {
            get
            {
                return crosshairCursor.gameObject.activeSelf;
            }
            set
            {
                crosshairCursor.gameObject.SetActive(value);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            IsCrossHairEnabled = false;
        }

        void Update()
        {
            if (IsCursorActive)
            {
                Cursor.position = Input.mousePosition;
            }
        }

        public void SetHealthBarFor(IDestroyable owner, float max_health, float normalized_current_health = 1f, float min_health = 0f)
        {
            var health_bar = Instantiate(healthBar) as HealthBar;
            health_bar.CachedTransform.parent = UIRoot;
            health_bar.Reset(owner, max_health, normalized_current_health, min_health);
        }
    }
}