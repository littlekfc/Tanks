using UnityEngine;
using System.Collections;

namespace UI
{
    public class HUDManager : Singleton<HUDManager>
    {
        public Transform crosshairCursor;

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
    }
}