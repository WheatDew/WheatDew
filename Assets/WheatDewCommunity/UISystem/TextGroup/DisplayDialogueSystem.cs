using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class DisplayDialogueSystem : ComponentSystem
{

    Dictionary<Vector2Int, string> DialogueSet = new Dictionary<Vector2Int, string>();

    protected override void OnUpdate()
    {
        GetDialogueContentJob();
    }

    

    //简单函数
    private void GetDialogueContentJob()
    {
        Entities.ForEach((DialogueCommand dialogueCommand) =>
        {
            DialogueSet.Add(new Vector2Int { x = dialogueCommand.origin, y = dialogueCommand.target }, dialogueCommand.content);
            Object.Destroy(dialogueCommand.gameObject);
        });
    }

    //供外部调用

    /// <summary>
    /// 获取内容
    /// </summary>
    public string GetDialogueForUI(int origin, int target)
    {
        Vector2Int temp = new Vector2Int { x = origin, y = target };
        Debug.Log("origin:" + origin + "target:" + target);
        if(DialogueSet.ContainsKey(temp))
        return DialogueSet[temp];

        return "……";
    }
}
