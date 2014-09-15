using UnityEngine;
using System.Collections;

public interface IObserver<T> 
{
    IObserable<T> ObservingObject { get; set; }

    void OnChange(T value);

    void UnSubscribe();
}
