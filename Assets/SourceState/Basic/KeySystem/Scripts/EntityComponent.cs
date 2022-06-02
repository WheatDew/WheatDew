using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EntityComponent : MonoBehaviour
{
    public string key;

    private void Awake()
    {
        EntitySystem.DistributeKey(this);
    }


}
