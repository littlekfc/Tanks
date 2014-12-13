using UnityEngine;
using System.Collections;

using Tanks.Resources;
using Tanks.Battle;

namespace Tanks.UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        public Transform crosshairCursor;
        public HealthBar healthBar;
        public ResourceDisplay resourceDisplay;

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

        public bool IsShown
        {
            get
            {
                return UIRoot.GetComponent<Canvas>().enabled;
            }
            set
            {
                UIRoot.GetComponent<Canvas>().enabled = value;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            IsCrossHairEnabled = false;
        }

        void Update()
        {
            // Cursors.
            if (IsCursorActive)
            {
                Cursor.position = Input.mousePosition;
            }

            // Resources.
            var my_resource = ResourceManager.Instance.MyResource;
            if (my_resource != null)
            {
                resourceDisplay.mineral.text = my_resource.Mineral.ToString();
            }
        }

        public void SetHealthBarFor(IDestroyable owner, float max_health, float normalized_current_health = 1f, float min_health = 0f)
        {
            var health_bar = Instantiate(healthBar) as HealthBar;
            health_bar.CachedTransform.parent = UIRoot;
            // There is a Unity bug or some sort here... If you reparent an UI element to a disabled Canvas, it will reset the child's scale to (0, 0, 0)!
            // For reverting that, I manually set it back to (1, 1, 1)... How stupid!
            health_bar.CachedTransform.localScale = Vector3.one;  
            health_bar.Reset(owner, max_health, normalized_current_health, min_health);
        }
    }
}