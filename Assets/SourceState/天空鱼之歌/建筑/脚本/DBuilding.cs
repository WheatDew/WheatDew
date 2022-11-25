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


    private async void Start()
    {
        await new WaitForUpdate();
        if(characterAddBuffer.Count > 0)
        {
            GameObject character = characterAddBuffer.Pop();
            buildingEnterCharacterDataList.Add(character.GetInstanceID(),new BuildingEnterCharacterData(character));
            Debug.Log(character.GetInstanceID());
        }

        if (buildingAddBuffer.Count > 0)
        {
            CBuilding building = buildingAddBuffer.Pop();
            buildingList.Add(building.gameObject.GetInstanceID(), building);
        }
    }

    //添加对象相关信息
    public void AddCharacterData(GameObject gameObject)
    {
        characterAddBuffer.Push(gameObject);
    }

    public void AddBuildingData(CBuilding building)
    {
        buildingAddBuffer.Push(building);
    }

    public BuildingEnterCharacterData GetCharacterData(int id)
    {
        return buildingEnterCharacterDataList[id];
    }

    public bool IsCharacterInList(int id)
    {
        if (buildingEnterCharacterDataList.ContainsKey(id))
            return true;
        return false;
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
