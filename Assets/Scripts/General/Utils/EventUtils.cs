using UnityEngine;
using System.Collections;
using System;

public static class EventUtils
{
    public static void Emit(Action event_to_emit)
    {
        if (event_to_emit != null)
            event_to_emit();
    }

    public static void Emit<T>(Action<T> event_to_emit, T event_param)
    {
        if (event_to_emit != null)
            event_to_emit(event_param);
    }

    public static void Emit<T1, T2>(Action<T1, T2> event_to_emit, T1 event_param1, T2 event_param2)
    {
        if (event_to_emit != null)
            event_to_emit(event_param1, event_param2);
    }

    public static void Emit<T1, T2, T3>(Action<T1, T2, T3> event_to_emit, T1 event_param1, T2 event_param2, T3 event_param3)
    {
        if (event_to_emit != null)
            event_to_emit(event_param1, event_param2, event_param3);
    }
}