using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDialogue : MonoBehaviour
{
    [SerializeField] private PDialogue dialoguePagePrefab;
    [HideInInspector] public PDialogue dialoguePage;

    private void Start()
    {
        SCommand.Declare(@"设置 对话信息 为 \S+?", SetDialogueContent);
    }

    public void SetDialogueContent(string value,CommandData commandData)
    {
        string[] values = value.Split(' ');
        if (commandData.replaceDatas.ContainsKey(values[2]))
        {
            values[2] = commandData.replaceDatas[values[2]](commandData.key);
        }
    }
}
