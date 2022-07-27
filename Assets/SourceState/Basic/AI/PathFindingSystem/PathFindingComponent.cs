using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Origin
{
    public class PathFindingComponent : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public TaskComponent behaviour;
        
        [HideInInspector] public PickItem pickTarget;
        [HideInInspector] public Transform target;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            PathFindingSystem.s.componentList.Add(transform.GetInstanceID().ToString(), this);
            agent.stoppingDistance = 2;
        }
    }
}

