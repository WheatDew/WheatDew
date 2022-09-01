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
        }

        

        private void OnTriggerEnter(Collider other)
        {
            closedTarget = SRange.s.citems[other.transform.GetInstanceID().ToString()].displayName ;
            for(int i = 0; i < enterCommands.Length; i++)
            {
                string[] commands = enterCommands[i].Split(' ');
                for(int n=0; n<commands.Length; n++)
                {
                    if (rule.ContainsKey(commands[n]))
                    {
                        commands[n] = rule[commands[n]]();
                    }
                }

                SCommand.s.ExecuteAlone(commands);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.GetInstanceID().ToString() == closedTarget)
            {
                closedTarget = null;
            }
            enterTargets.Remove(other.transform.GetInstanceID().ToString());
            for (int i = 0; i < exitCommands.Length; i++)
            {
                SCommand.s.Execute(exitCommands[i]);
            }
        }
    }
}

