using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class CRange : MonoBehaviour
{
    [HideInInspector] public string key;
    RangeData rangeData;
    CommandData commandData;

    delegate string conversion();

    //规则相关
    private Dictionary<string, conversion> rules = new Dictionary<string, conversion>();

    //触发指令相关
    [TextArea(0, 5)] public string enterCommand, exitCommand,clickCommand;

    //外部输入
    public string displayInfo;

    private void Start()
    {
        key = transform.GetInstanceID().ToString();
        SRange.Add(key, this);
        rangeData=new RangeData(key);
        rangeData.displayInfo = displayInfo;
        commandData = new CommandData(key,DRange.repalceDatas);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SCommand.Execute(clickCommand);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        rangeData.closedTarget = other.transform.GetInstanceID().ToString();
        rangeData.enterTargets.Add(rangeData.closedTarget);
        SCommand.Execute(enterCommand, commandData);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetInstanceID().ToString() == rangeData.closedTarget)
        {
            rangeData.closedTarget = null;
        }
        rangeData.enterTargets.Remove(other.transform.GetInstanceID().ToString());
        SCommand.Execute(exitCommand,commandData);
    }
}


