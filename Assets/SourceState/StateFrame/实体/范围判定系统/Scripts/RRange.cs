using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RRange : MonoBehaviour
{
    private CInfoInScene infoInScenePrefab;
    public CInfoInScene infoInScene;

    private void Start()
    {
        SCommand.Declare(@"��ʾ \S+? ��Ϣ",DisplayInfo);
        SCommand.Declare(@"���� ��Ϣ", HideInfo);
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
            infoInScene = Instantiate(infoInScenePrefab, FindObjectOfType<Canvas>().transform);
            infoInScene.content.text=values[1];
        }
    }

    public void HideInfo(string value,CommandData commandData)
    {
        if (infoInScene != null)
            DestroyImmediate(infoInScene.gameObject);
    }
}
