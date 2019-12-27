using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInitialization : MonoBehaviour
{
    public CharacterCreateCommand CharacterCreateCommandPrefab;

    private void Start()
    {

        WorldTestPrefab();
        
    }

    //测试预设
    private void WorldTestPrefab()
    {
        CreateCharacterCreateCommand("origin",true);
        CreateCharacterCreateCommand("target", false);
    }

    //简单函数
    private void CreateCharacterCreateCommand(string Name,bool IsMainCharacter)
    {
        CharacterCreateCommand characterCreateCommand = Instantiate(CharacterCreateCommandPrefab);
        characterCreateCommand.IsMainCharacter = IsMainCharacter;
        characterCreateCommand.Name = Name;
        characterCreateCommand.gameObject.SetActive(true);
        Debug.Log("Create Character "+Name+ " successed");
    }
}
