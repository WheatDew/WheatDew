using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CRange : MonoBehaviour
{
    public string key;
    RangeData rangeData;

    //触发指令相关
    [TextArea(1, 5)] public string enterCommand, exitCommand;

    //外部输入
    public string displayInfo;

    private void Start()
    {
        key = transform.GetInstanceID().ToString();
        SRange.Add(key, this);
        rangeData=new RangeData(key);
        rangeData.displayInfo = displayInfo;
    }

    private void OnTriggerEnter(Collider other)
    {
        rangeData.closedTarget = other.transform.GetInstanceID().ToString();
        rangeData.enterTargets.Add(rangeData.closedTarget);
        SCommand.Execute(enterCommand,null);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetInstanceID().ToString() == rangeData.closedTarget)
        {
            rangeData.closedTarget = null;
        }
        rangeData.enterTargets.Remove(other.transform.GetInstanceID().ToString());
        SCommand.Execute(exitCommand,null);
    }
}


