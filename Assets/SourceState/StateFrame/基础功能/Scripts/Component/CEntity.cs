using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CEntity : MonoBehaviour
{
    public string key;
    public List<string> tags=new List<string> ();

    private void Start()
    {
        key = transform.GetInstanceID().ToString();
        SEntity.Add(key, this);
    }
}
