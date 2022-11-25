using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBuilding : MonoBehaviour
{
    public HashSet<GameObject> EnterList=new HashSet<GameObject>();

    private void Start()
    {
        DBuilding.s.AddBuildingData(this);
    }

    private void Update()
    {
        foreach(var item in EnterList)
        {
            if (DBuilding.s.buildingEnterCharacterDataList.ContainsKey(item.GetInstanceID()))
            {
                BuildingEnterCharacterData characterData = DBuilding.s.buildingEnterCharacterDataList[item.GetInstanceID()];
                if (characterData.buildingPrepare)
                {
                    characterData.buildingPrepare = false;
                    characterData.isbuilding = true;
                }

                if (characterData.isbuilding)
                {
                    Debug.Log(characterData.gameObject.name);
                }
            }
        }


    }
}
