using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{
    private static EntitySystem instance;
    public static EntitySystem s { get { return instance; } }

    private void Awake()
    {
        if (instance == null) instance = this;   
    }

    public Dictionary<string, EntityComponent> components = new Dictionary<string, EntityComponent>();

    //É¾³ýÊµÌå
    public void DestroyEntity(string key)
    {
        Destroy(components[key].gameObject);
    }
}
