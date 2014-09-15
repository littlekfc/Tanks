using UnityEngine;
using System.Collections;

public interface IObserable<T> 
{
    void Subscribe(IObserver<T> observer);

    void Unsubscribe(IObserver<T> observer);
}
