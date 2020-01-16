using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterMemorySystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        
    }

    private void MainCharacterGetWordJob()
    {
        Entities.ForEach((CharacterProperty p_character, CharacterLongMemoryProperty p_characterLongMemory,CharacterReceivedWordsProperty p_characterReceived,TimerProperty p_timer) =>
        {
            //p_characterLongMemory.LongMemory.Add(GetMemoryStringFromReceivedWordsAndCurrentDate())
        });
    }

    private string GetMemoryStringFromReceivedWordsAndCurrentDate(HashSet<string> receivedWords,string currentDate)
    {
        string memoryString = "";


        return memoryString;
    }
}
