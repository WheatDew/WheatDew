using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MindLogSystem : ComponentSystem
{
    public string mindLog;

    protected override void OnUpdate()
    {
        Entities.ForEach((CharacterMindProperty characterMindProperty, CharacterProperty characterProperty) =>
        {
            mindLog = "";
            foreach(var item in characterMindProperty.Mind)
            {
                mindLog += characterProperty.Name + ": " + item.Key + " " + item.Value.ToString()+"\n"; 
            }

            mindLog += characterProperty.Name + ": ";
            foreach (var item in characterMindProperty.ReceivedWords)
            {
                mindLog += item;
            }
            mindLog += "\n";
        });
    }
}
