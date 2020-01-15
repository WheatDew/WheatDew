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
        Entities.ForEach((CharacterProperty p_character, CharacterLongMemoryProperty p_characterLongMemory,CharacterReceivedWordsProperty p_characterReceived) =>
        {
            //todo mainCharacter的接收词汇处理成记忆
        });
    }
}
