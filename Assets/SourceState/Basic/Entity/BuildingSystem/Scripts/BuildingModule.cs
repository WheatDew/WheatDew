using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingModule : MonoBehaviour
    {
        private void Start()
        {
            new NewCommand("FinishBuilding", FinishBuilding);
        }

        public void FinishBuilding(string value)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit result;
            if (Physics.Raycast(ray, out result, 100, 1 << 9))
            {
                BuildingComponent building = result.collider.gameObject.GetComponent<BuildingComponent>();
                BuildingPack buildingPack = result.collider.GetComponent<BuildingPack>();
                if (building!=null&&buildingPack!=null)
                {

                    Dictionary<string, Item> requirement = buildingPack.requirement;
                    foreach (var item in requirement)
                    {
                        Debug.Log(item.Key + " " + item.Value.ToString());
                    }
                    Dictionary<string, Item> pack = 
                        PackSystem.S.components[value].pack;
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
                    }


                }
            }

        }
    }
}

