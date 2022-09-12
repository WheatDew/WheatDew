using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DPlant
{
    public static Dictionary<string, PlantData> plants = new Dictionary<string, PlantData>();

    public static Dictionary<string, replace> repalceDatas = InitDatas();

    public static Dictionary<string, replace> InitDatas()
    {
        Dictionary<string, replace> initReplaceDatas = new Dictionary<string, replace>();

        initReplaceDatas.Add("×ÔÉíÃû³Æ", delegate (string key) { return plants[plants[key].name].name; });

        return initReplaceDatas;
    }
}



public class PlantData
{
    public string name;
    public float stageTime;
    public string[] modelNameList;
    public int currentStageIndex;

    public PlantData(string name)
    {
        this.name = name;
    }
}