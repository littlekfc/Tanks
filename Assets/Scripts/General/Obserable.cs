using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Obserable<T> : IObserable<T> where T : class 
{
    private HashSet<IObserver<T>> observers = new HashSet<IObserver<T>>();

    public void Subscribe(IObserver<T> observer)
    {
        if (observers.Add(observer))
            observer.ObservingObject = this;
    }

    public void Unsubscribe(IObserver<T> observer)
    {
        if (observers.Remove(observer))
            observer.ObservingObject = null;
    }

    protected void OnChange(T value)
    {
        foreach (var o in observers)
            o.OnChange(value);
    }
}
