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
        Dictionary<string, float> tendencyofOrigin = new Dictionary<string, float>();
        tendencyofOrigin.Add("仿生人", 60f);
        tendencyofOrigin.Add("机械", 30f);
        tendencyofOrigin.Add("神话", -30f);
        tendencyofOrigin.Add("写作", -30f);
        tendencyofOrigin.Add("游泳", -30f);
        tendencyofOrigin.Add("理性", 30f);
        tendencyofOrigin.Add("哲学", -30f);
        tendencyofOrigin.Add("科学", 60f);
        CreateCharacterCreateCommand("origin",true,1, tendencyofOrigin);
        CreateCharacterCreateCommand("target", false,2);
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
