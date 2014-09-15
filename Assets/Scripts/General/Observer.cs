using UnityEngine;
using System.Collections;

public abstract class Observer<T> : IObserver<T> where T : class 
{
    public IObserable<T> ObservingObject { get; set; }

    public abstract void OnChange(T value);

    public void UnSubscribe()
    {
        if (ObservingObject != null)
            ObservingObject.Unsubscribe(this);
    }
}
