using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityComponent : MonoBehaviour
{
    public string displayName;
    public string itemName;
    public int itemCount;

    private void Start()
    {
        EntitySystem.s.components.Add(transform.GetInstanceID().ToString(), this);
    }
}
