using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Origin
{
    public class PathFindingComponent : MonoBehaviour
    {
        [HideInInspector] public NavMeshAgent agent;
        [HideInInspector] public BehaviourComponent behaviour;
        
        public PickItem pickTarget;
        public Transform target;

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            PathFindingSystem.s.componentList.Add(GetComponent<EntityComponent>().key, this);
            agent.stoppingDistance = 2;

            behaviour = GetComponent<BehaviourComponent>();
            behaviour.behaviourDatas.Add(new BehaviourData("type", -10,
                string.Format("GetFoodWeight {0} {1}", GetComponent<EntityComponent>().key, 30),
                PickClosestItem));
        }

        private void Update()
        {
            if(pickTarget != null&&Vector3.Distance(transform.position,pickTarget.transform.position)<=2)
            {
                Destroy(pickTarget.gameObject);
            }
        }

        public void PickClosestItem()
        {
            string closestItem = PickSystem.s.ClosestItem(transform.position);
            if (closestItem != null)
            {
                PickItem target = PickSystem.s.pickItems[closestItem];
                pickTarget = target;
                agent.destination = target.transform.position;
            }

        }
    }
}

