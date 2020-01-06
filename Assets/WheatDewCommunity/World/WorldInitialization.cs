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
        CreateCharacterCreateCommand("origin",true,1);
        CreateCharacterCreateCommand("target", false,2);
    }

    //简单函数
    private void CreateCharacterCreateCommand(string Name,bool IsMainCharacter,int target)
    {
        CharacterCreateCommand characterCreateCommand = Instantiate(CharacterCreateCommandPrefab);
        characterCreateCommand.IsMainCharacter = IsMainCharacter;
        characterCreateCommand.Name = Name;
        characterCreateCommand.target = target;
        characterCreateCommand.gameObject.SetActive(true);
        Debug.Log("Create Character command "+Name+ " succeed");
    }
}
