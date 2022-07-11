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
            //��ʼ��
            s_pick = PickSystem.s;
            //�������
            TaskSystem.s.Declare("PickClosestItem", PickCloseItemTask);

        }

        /// <summary>
        /// ʰȡ�������������
        /// </summary>
        /// <param name="values">command,proposer</param>
        /// <param name="taskData"></param>
        public async void PickCloseItemTask(string[] values, TaskData taskData)
        {
            //��ȡ��������transform
            Transform proposer = s_pick.components[values[1]].transform;
            //��ȡ�����Ŀ������
            string itemName = s_pick.ClosestItem(proposer.position);
            //��ȡĿ���transform
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

