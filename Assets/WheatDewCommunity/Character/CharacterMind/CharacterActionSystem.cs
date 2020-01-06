using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterActionSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        Entities.ForEach((CharacterMindProperty characterMindProperty,DialogueProperty dialogueProperty) =>
        {
            if (characterMindProperty.DialogueImmediateMind.ContainsKey("回答"))
            {
                dialogueProperty.dialogueChance = true;
                
            }
        });
    }

}
