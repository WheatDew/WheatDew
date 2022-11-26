using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBuilding : MonoBehaviour
{

    private static DBuilding _s;
    public static DBuilding s { get{ return _s; } }

    private Stack<GameObject> characterAddBuffer = new Stack<GameObject>();
    private Stack<CBuilding> buildingAddBuffer = new Stack<CBuilding>();

    private void Awake()
    {
        if (_s == null)
            _s = this;
    }


    public Dictionary<int,CBuilding> buildingList=new Dictionary<int, CBuilding>();
    public Dictionary<int, BuildingEnterCharacterData> buildingEnterCharacterDataList = new Dictionary<int, BuildingEnterCharacterData>();

    //测试数组
    public Dictionary<int, CCharacter> characterList = new Dictionary<int, CCharacter>();
    

    private async void Start()
    {

        Debug.Log(characterAddBuffer.Count);
        await new WaitForUpdate();
        Debug.Log(characterAddBuffer.Count);
        if (characterAddBuffer.Count > 0)
        {
            GameObject character = characterAddBuffer.Pop();
            buildingEnterCharacterDataList.Add(character.GetInstanceID(),new BuildingEnterCharacterData(character));
            
            //Debug.Log(character.GetInstanceID().ToString()+" "+character.transform.GetInstanceID().ToString());

        }

        if (buildingAddBuffer.Count > 0)
        {
            CBuilding building = buildingAddBuffer.Pop();
            buildingList.Add(building.gameObject.GetInstanceID(), building);
        }

        
    }

    //添加对象相关信息
    public void AddCharacterData(CCharacter character)
    {
        characterList.Add(character.gameObject.GetInstanceID(), character);
    }

    public void AddBuildingData(CBuilding building)
    {
        buildingAddBuffer.Push(building);
    }

    public CCharacter GetCharacterData(int id)
    {

        return characterList[id];
    }

}

public class BuildingEnterCharacterData
{

    public GameObject gameObject;
    public bool buildingPrepare;
    public bool isbuilding;

    public BuildingEnterCharacterData(GameObject gameObject)
    {
        this.gameObject = gameObject;
        
    }
}


//测试类

public class CharacterBuildingData
{
    public GameObject gameObject;
    public bool buildingPrepare, isbuilding;

    public CharacterBuildingData(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }
}
