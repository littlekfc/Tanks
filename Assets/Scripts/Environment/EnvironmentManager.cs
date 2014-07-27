using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour 
{
    public float environmentDrag = 0.0f;

    public static EnvironmentManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            throw new System.Exception("Tring to create multiple instances of a singleton!");
    }
}
