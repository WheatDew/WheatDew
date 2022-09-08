using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DRange
{
    public static Dictionary<string, RangeData> datas = new Dictionary<string, RangeData>();

    public static Dictionary<string,replace> repalceDatas=new Dictionary<string, replace>();

    public static Dictionary<string,replace> InitDatas()
    {
        Dictionary<string, replace> initReplaceDatas = new Dictionary<string, replace>();
        initReplaceDatas.Add("范围内物体名称", delegate (string key) { return datas[datas[key].closedTarget].displayInfo; });
        return initReplaceDatas;
    }

    public static void Add(string key, RangeData rangeData)
    {
        datas.Add(key, rangeData);
    }
}

public class RangeData
{
    public string closedTarget;
    public HashSet<string> enterTargets=new HashSet<string>();
    public string displayInfo;

    public RangeData(string key)
    {
        DRange.datas.Add(key, this);
    }
}



