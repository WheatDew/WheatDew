using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrollPoint : MonoBehaviour
{
    public HashSet<GameObject> inside=new HashSet<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        inside.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if(inside.Contains(other.gameObject))
            inside.Remove(other.gameObject);
    }
}
