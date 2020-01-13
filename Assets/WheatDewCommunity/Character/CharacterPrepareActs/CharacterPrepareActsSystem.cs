using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterPrepareActsSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        CharacterActJob();
    }

    private void CharacterActJob()
    {
        Entities.ForEach((CharacterPrepareActsProperty p_PrepareActs,DialogueProperty p_dialogue) =>
        {
            if (p_PrepareActs.PrepareDialogueActs.Contains("回答"))
            {
                p_dialogue.dialogueChance = true;
                Debug.Log("检测到回答关键词,开启对话");
            }
            else if (p_PrepareActs.PrepareDialogueActs.Contains("回应"))
            {
                p_dialogue.dialogueChance = true;
                Debug.Log("检测到回应关键词,开启对话");
            }
        });
    }
}
