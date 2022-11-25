using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBuildingRange : MonoBehaviour
{
    public CBuilding target;

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            target.EnterList.Add(other.gameObject);   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target.EnterList.Contains(other.gameObject))
        {
            target.EnterList.Remove(other.gameObject);
        }
    }
}
