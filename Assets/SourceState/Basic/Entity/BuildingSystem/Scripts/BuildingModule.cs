using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingModule : MonoBehaviour
    {
        private void Start()
        {
            TaskSystem.s.Declare("FinishBuilding", FinishBuilding);

        }

        public void FinishBuilding(string[] values,TaskData taskData)
        {
            BuildingComponent building = BuildingSystem.S.buildings[values[2]];
            BuildingPack buildingPack = building.GetComponent<BuildingPack>();
            if (building != null && buildingPack != null)
            {
                if (buildingPack.requirement.Count != 0)
                {
                    Dictionary<string, Item> requirement = buildingPack.requirement;
                    foreach (var item in requirement)
                    {
                        Debug.Log(item.Key + " " + item.Value.ToString());
                    }
                    Dictionary<string, Item> pack =
                        PackSystem.S.components[values[1]].pack;
                    if (requirement != null)
                    {
                        foreach (var item in requirement)
                        {
                            if (pack.ContainsKey(item.Key))
                            {
                                if (pack[item.Key].count > requirement[item.Key].count)
                                {
                                    pack[item.Key].count -= requirement[item.Key].count;
                                    requirement[item.Key].count = 0;
                                }
                                else
                                {
                                    requirement[item.Key].count -= pack[item.Key].count;
                                    pack[item.Key].count = 0;
                                }
                            }
                        }
                        buildingPack.CheckRequirement();
                        HashSet<string> items = RangeSystem.s.components[values[1]].currentRange;
                        items.Clear();
                        InfoInSceneSystem.s.HiddenItemInfoInScene();
                    }
                }
                


            }

        }
    }
}

