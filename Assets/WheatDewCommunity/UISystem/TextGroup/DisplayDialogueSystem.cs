using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class DisplayDialogueSystem : ComponentSystem
{
    struct Content
    {
        public string content;
        public float life;
    }

    Dictionary<Vector2Int, Content> DialogueSet = new Dictionary<Vector2Int, Content>();

    protected override void OnUpdate()
    {
        GetDialogueContentJob();
    }

    

    //简单函数
    private void GetDialogueContentJob()
    {
        Entities.ForEach((DialogueCommand dialogueCommand) =>
        {
            Vector2Int v2 = new Vector2Int { x = dialogueCommand.origin, y = dialogueCommand.target };
            if (DialogueSet.ContainsKey(v2))
                DialogueSet[v2] = new Content {content=dialogueCommand.content,life=dialogueCommand.life };
            else
                DialogueSet.Add(v2, new Content { content = dialogueCommand.content, life = dialogueCommand.life });
            Debug.Log(dialogueCommand.origin.ToString() + " " + dialogueCommand.target.ToString());
            Object.Destroy(dialogueCommand.gameObject);
        });
    }

    //供外部调用

    /// <summary>
    /// 获取内容
    /// </summary>
    public void GetDialogueForUI(int origin, int target,out string content,out float life)
    {
        content = "";
        life = -1f;
        Vector2Int temp = new Vector2Int { x = origin, y = target };
        //Debug.Log("origin:" + origin + "target:" + target);
        if (DialogueSet.ContainsKey(temp))
        {
            content = DialogueSet[temp].content;
            life = DialogueSet[temp].life;
            DialogueSet.Remove(temp);
        }

        
    }
}
