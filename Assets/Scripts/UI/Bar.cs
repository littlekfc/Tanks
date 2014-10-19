using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UI
{
    public abstract class Bar : TBehaviour
    {
        private Slider slider = null;
        private Slider Slider
        {
            get
            {
                if (slider == null)
                    slider = GetComponentInChildren<Slider>();

                return slider;
            }
        }

        public float MaxValue
        {
            get
            {
                return Slider.maxValue;
            }
            set
            {
                Slider.maxValue = value;
            }
        }

        public float MinValue
        {
            get
            {
                return Slider.minValue;
            }
            set
            {
                Slider.minValue = value;
            }
        }

        public float NormalizedValue
        {
            get
            {
                return Slider.normalizedValue;
            }
            set
            {
                Slider.normalizedValue = value;
            }
        }

        public float Value
        {
            get
            {
                return Slider.value;
            }
            set
            {
                Slider.value = value;
            }
        }
    }
}