using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CharacterSystem : ComponentSystem
{

    private GameObject characterPrefab;
    private GameObject characterCreateCommandPrefab;

    public int counter = 0;

    private Dictionary<int, GameObject> CharacterList = new Dictionary<int, GameObject>();

    protected override void OnStartRunning()
    {
        InitializeCharacterPrefab();
        InitializeCharacterCreateCommandPrefab();
    }

    protected override void OnUpdate()
    {
        CharacterCreateJob();
        CharacterDestroyJob();
    }

    //简单函数
    private void CharacterCreateJob()
    {
        Entities.ForEach((CharacterCreateCommand characterCreateCommand) =>
        {

            GameObject character = Object.Instantiate(characterPrefab);
            counter++;
            CharacterProperty characterProperty = character.GetComponent<CharacterProperty>();
            characterProperty.ID = counter;
            characterProperty.Name = characterCreateCommand.Name;
            characterProperty.IsMainCharacter = characterCreateCommand.IsMainCharacter;
            CharacterList.Add(counter, character);
            Object.Destroy(characterCreateCommand.gameObject);
            character.SetActive(true);
        });
    }

    private void CharacterDestroyJob()
    {
        Entities.ForEach((CharacterDestroyCommand characterCreateCommand) =>
        {
            Object.Destroy(CharacterList[characterCreateCommand.ID]);
        });
    }

    public void InitializeCharacterPrefab()
    {
        if (characterPrefab == null)
        {

            Entities.ForEach((CharacterProperty character) =>
            {
                characterPrefab = character.gameObject;
                character.gameObject.SetActive(false);
            });
        }
    }

    public void InitializeCharacterCreateCommandPrefab()
    {
        if (characterCreateCommandPrefab == null)
        {
            Entities.ForEach((CharacterCreateCommand characterCreateCommand) =>
            {
                if (characterCreateCommand.Name == "" && !characterCreateCommand.IsMainCharacter)
                {
                    characterCreateCommandPrefab = characterCreateCommand.gameObject;
                    characterCreateCommand.gameObject.SetActive(false);
                }
            });
        }
    }
}
