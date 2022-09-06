using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace State
{
    public class CRange : CDrive
    {
        //碰撞相关
        [HideInInspector] public string closedTarget="";
        public HashSet<string> enterTargets=new HashSet<string>();

        //触发指令相关
        [TextArea(5,20)] public string enterCommand,exitCommand;
        [HideInInspector] public string[] enterCommands,exitCommands;

        //显示信息相关
        public string displayName;

        public override void Init()
        {
            SRange.s.citems.Add(key, this);
            enterCommands = enterCommand.Split('\n');
            exitCommands = exitCommand.Split('\n');

            //关联命令数据指代
            AddDriveData("范围内最近物体的名字", ()=>closedTarget);
            //添加命令缓存
            AddDriveCache("进入范围时触发",enterCommand);
            AddDriveCache("离开范围时触发", exitCommand);
        }

        

        private void OnTriggerEnter(Collider other)
        {
            closedTarget = SRange.s.citems[other.transform.GetInstanceID().ToString()].displayName ;
            caches["进入范围时触发"]();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.transform.GetInstanceID().ToString() == closedTarget)
            {
                closedTarget = null;
            }
            enterTargets.Remove(other.transform.GetInstanceID().ToString());
            caches["离开范围时触发"]();
        }
    }
}

