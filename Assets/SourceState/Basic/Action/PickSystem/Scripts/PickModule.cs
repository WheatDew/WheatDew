using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickModule : MonoBehaviour
    {
        [HideInInspector] public PickSystem s_pick;

        private void Start()
        {
            //≥ı ºªØ
            s_pick = PickSystem.s;
            //ÃÌº”√¸¡Ó
            TaskSystem.s.Declare("PickClosestItem", PickCloseItemTask);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">command,proposer</param>
        /// <param name="taskData"></param>
        public async void PickCloseItemTask(string[] values, TaskData taskData)
        {

            Transform proposer = s_pick.components[values[1]].transform;
            string itemName = s_pick.ClosestItem(proposer.position);
            Transform target = s_pick.items[itemName].transform;

            if (values.Length == 3 && values[2] == "true" && Vector3.Distance(proposer.position, target.position) > 5)
            {
                return;
            }

            PathFindingSystem.s.SetTargetPosition(values[1], target.position);

            while (Vector3.Distance(target.position, proposer.position) > 2)
            {
                await new WaitForSeconds(Time.deltaTime);
            }
            s_pick.PickItem(values[1], itemName);
        }
    }
}

