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
        Dictionary<string, float> tendencyTarget = new Dictionary<string, float>();
        CreateCharacterCreateCommand("origin",true,1);
        tendencyTarget.Add("g仿生人", 60f);
        tendencyTarget.Add("g机械", 30f);
        tendencyTarget.Add("g神话", -30f);
        tendencyTarget.Add("g写作", -30f);
        tendencyTarget.Add("g游泳", -30f);
        tendencyTarget.Add("g理性", 30f);
        tendencyTarget.Add("g哲学", -30f);
        tendencyTarget.Add("g科学", 60f);
        CreateCharacterCreateCommand("target", false,2, tendencyTarget);
    }

    //简单函数
    /// <summary>
    /// 添加创建角色命令
    /// </summary>
    private void CreateCharacterCreateCommand(string Name,bool IsMainCharacter,int target,Dictionary<string,float> tendency)
    {
        CharacterCreateCommand characterCreateCommand = Instantiate(CharacterCreateCommandPrefab);
        characterCreateCommand.IsMainCharacter = IsMainCharacter;
        characterCreateCommand.Name = Name;
        characterCreateCommand.target = target;
        characterCreateCommand.gameObject.SetActive(true);
        characterCreateCommand.Tendency = new Dictionary<string, float>(tendency);

        Debug.Log("Create Character command "+Name+ " succeed");
    }

    /// <summary>
    /// 添加创建角色命令,不带偏好参数
    /// </summary>
    private void CreateCharacterCreateCommand(string Name, bool IsMainCharacter, int target)
    {
        CharacterCreateCommand characterCreateCommand = Instantiate(CharacterCreateCommandPrefab);
        characterCreateCommand.IsMainCharacter = IsMainCharacter;
        characterCreateCommand.Name = Name;
        characterCreateCommand.target = target;
        characterCreateCommand.gameObject.SetActive(true);
        characterCreateCommand.Tendency = new Dictionary<string, float>();

        Debug.Log("Create Character command " + Name + " succeed");
    }
}
