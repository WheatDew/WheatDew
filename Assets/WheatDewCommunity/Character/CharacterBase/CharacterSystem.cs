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
        CatchSendWordCommandJob();
    }

    //简单函数

    /// <summary>
    /// 创建角色
    /// </summary>
    private void CharacterCreateJob()
    {
        Entities.ForEach((CharacterCreateCommand characterCreateCommand) =>
        {

            GameObject character = Object.Instantiate(characterPrefab);
            counter++;
            CharacterProperty characterProperty = character.GetComponent<CharacterProperty>();
            DialogueProperty dialogueProperty = character.GetComponent<DialogueProperty>();
            characterProperty.ID = counter;
            characterProperty.Name = characterCreateCommand.Name;
            characterProperty.IsMainCharacter = characterCreateCommand.IsMainCharacter;
            dialogueProperty.target = characterCreateCommand.target;
            CharacterList.Add(counter, character);
            Object.Destroy(characterCreateCommand.gameObject);
            character.SetActive(true);
            Debug.Log("Create Character " + characterProperty.Name + " succeed");
        });
    }

    /// <summary>
    /// 销毁角色
    /// </summary>
    private void CharacterDestroyJob()
    {
        Entities.ForEach((CharacterDestroyCommand characterCreateCommand) =>
        {
            Object.Destroy(CharacterList[characterCreateCommand.ID]);
        });
    }

    private void CatchSendWordCommandJob()
    {
        HashSet<SendWordCommand> deleteList = new HashSet<SendWordCommand>();
        Entities.ForEach((SendWordCommand sendWordCommand) => {
            CharacterReceivedWordsProperty p_ReceivedWords = CharacterList[sendWordCommand.target].GetComponent<CharacterReceivedWordsProperty>();
            string log = "";
            foreach (var item in sendWordCommand.context)
            {
                p_ReceivedWords.ReceivedWords.Add(item);
                log += item+" ";
            }
            deleteList.Add(sendWordCommand);
            Debug.Log("写入ReceivedWords属性,ID" + sendWordCommand.target + " 内容:" + log);
        });

        foreach(var item in deleteList)
        {
            Object.Destroy(item.gameObject);
        }
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
