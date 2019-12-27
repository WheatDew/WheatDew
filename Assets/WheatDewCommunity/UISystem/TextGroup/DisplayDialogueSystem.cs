using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class DisplayDialogueSystem : ComponentSystem
{

    struct Connect
    {
        public int origin;
        public int target;
    }
    Dictionary<Connect, string> DialogueSet = new Dictionary<Connect, string>();

    protected override void OnUpdate()
    {
        GetDialogueContentJob();
    }

    

    //简单函数
    private void GetDialogueContentJob()
    {
        Entities.ForEach((DialogueCommand dialogueCommand) =>
        {
            DialogueSet.Add(new Connect { origin = dialogueCommand.origin, target = dialogueCommand.target }, dialogueCommand.content);
            Object.Destroy(dialogueCommand.gameObject);
        });
    }

    //供外部调用

    /// <summary>
    /// 获取内容
    /// </summary>
    public string GetDialogueForUI(int origin, int target)
    {
        Connect temp = new Connect { origin = origin, target = target };
        return DialogueSet[temp];
    }
}
