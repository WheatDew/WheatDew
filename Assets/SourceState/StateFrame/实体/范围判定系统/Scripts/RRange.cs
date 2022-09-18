using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RRange : MonoBehaviour
{
    [SerializeField] private CInfoInScene infoInScenePrefab;
    private CInfoInScene infoInScene;

    public Transform infop;

    private void Start()
    {
        //添加命令部分
        SCommand.Declare(@"显示 \S+? 的信息",DisplayInfo);
        SCommand.Declare(@"隐藏 信息", HideInfo);
    }

    public void DisplayInfo(string value,CommandData commandData)
    {
        
        string[] values = value.Split(' ');
        if (commandData.replaceDatas.ContainsKey(values[1]))
        {
            values[1] = commandData.replaceDatas[values[1]](commandData.key);
        }

        if (infoInScene != null)
        {
            infoInScene.content.text = values[1];
        }
        else
        {
            infoInScene = Instantiate(infoInScenePrefab, infop);
            infoInScene.content.text=values[1];
        }
    }

    public void HideInfo(string value,CommandData commandData)
    {
        if (infoInScene != null)
            Destroy(infoInScene.gameObject);
    }
}
