using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourComponent : MonoBehaviour
{
    private void Start()
    {
        BehaviourSystem.s.behaviours.Add(GetComponent<EntityComponent>().key, this);
    }
}
