using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Origin
{
    public class PathFindingComponent : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        public PickItem pickTarget;
        public Transform target;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            PathFindingSystem.s.componentList.Add(GetComponent<EntityComponent>().key, this);
            agent.stoppingDistance = 2;
            test();
        }

        public void Update()
        {
            if (pickTarget != null)
            {
                if (Vector3.Distance(pickTarget.transform.position, transform.position) <= 2)
                {
                    pickTarget.Destroy();
                    PickClosestItem();
                }
            }
        }

        public void test()
        {
            PickClosestItem();
        }

        public void PickClosestItem()
        {
            string closestItem = PickSystem.s.ClosestItem(transform.position);
            if(closestItem != null)
            {
                PickItem target = PickSystem.s.pickItems[closestItem];
                pickTarget = target;
                agent.destination = target.transform.position;
            }

        }
    }
}

