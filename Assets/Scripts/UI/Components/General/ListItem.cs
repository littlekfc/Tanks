using UnityEngine;
using System.Collections;

namespace Tanks.UI.Components
{
    public abstract class ListItem : MonoBehaviour
    {

    }

    public abstract class ListItem<T> : ListItem where T : class
    {
        public T Data { get; set; }

        public void Reset(T data)
        {
            Data = data;
            OnReset(data);
        }

        protected abstract void OnReset(T data);
    }
}