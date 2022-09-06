using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace State
{
    public class CRange : CDrive
    {
        //��ײ���
        [HideInInspector] public string closedTarget="";
        public HashSet<string> enterTargets=new HashSet<string>();

        //����ָ�����
        [TextArea(5,20)] public string enterCommand,exitCommand;
        [HideInInspector] public string[] enterCommands,exitCommands;

        //��ʾ��Ϣ���
        public string displayName;

        public override void Init()
        {
            SRange.s.citems.Add(key, this);
            enterCommands = enterCommand.Split('\n');
            exitCommands = exitCommand.Split('\n');

            //������������ָ��
            AddDriveData("��Χ��������������", ()=>closedTarget);
            //��������
            AddDriveCache("���뷶Χʱ����",enterCommand);
            AddDriveCache("�뿪��Χʱ����", exitCommand);
        }

        

        private void OnTriggerEnter(Collider other)
        {
            closedTarget = SRange.s.citems[other.transform.GetInstanceID().ToString()].displayName ;
            caches["���뷶Χʱ����"]();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.GetInstanceID().ToString() == closedTarget)
            {
                closedTarget = null;
            }
            enterTargets.Remove(other.transform.GetInstanceID().ToString());
            caches["�뿪��Χʱ����"]();
        }
    }
}

