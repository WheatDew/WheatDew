using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateSystem<MonoBehaviour>
{
    public static Dictionary<string, MonoBehaviour> components=new Dictionary<string, MonoBehaviour>();

    public static void Add(string key, MonoBehaviour component)
    {
        components.Add(key,component);
    }

    public static void Add(MonoBehaviour component)
    {
        
    }
}
