using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MindLogSystem : ComponentSystem
{
    public string mindLog;

    protected override void OnUpdate()
    {
        //Entities.ForEach((CharacterReceivedWordsProperty p_ReceivedWords, CharacterProperty characterProperty) =>
        //{

        //    mindLog += characterProperty.Name + ": ";
        //    foreach (var item in p_ReceivedWords.ReceivedWords)
        //    {
        //        mindLog += item;
        //    }
        //    mindLog += "\n";
        //});
    }
}
