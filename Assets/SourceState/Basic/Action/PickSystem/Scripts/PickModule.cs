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
            //初始化
            s_pick = PickSystem.s;
            //添加命令
            TaskSystem.s.Declare("PickClosestItem", PickClosestItemTask);
        }

        /// <summary>
        /// 拾取距离最近的物体
        /// </summary>
        /// <param name="values">command,proposer</param>
        /// <param name="taskData"></param>
        public async void PickClosestItemTask(string[] values, TaskData taskData)
        {
            //获取发起对象的transform
            if (!s_pick.components.ContainsKey(values[1]))
            {
                Debug.LogError(string.Format("不存在ID{0}", values[1]));
                return;
            }
            Transform proposer = s_pick.components[values[1]].transform;
            //获取最近的目标名称
            string itemName = s_pick.ClosestItem(proposer.position);
            //获取目标的transform
            Transform target = s_pick.items[itemName].transform;

            PathFindingSystem.s.SetTargetPosition(values[1], target.position);

            while (Vector3.Distance(target.position, proposer.position) > 2)
            {
                await new WaitForSeconds(Time.deltaTime);
            }
            s_pick.PickItem(values[1], itemName);
        }
    }
}

