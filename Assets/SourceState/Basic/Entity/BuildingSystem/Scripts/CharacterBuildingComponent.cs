using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CharacterBuildingComponent : MonoBehaviour
    {
        private RangeComponent c_range;

        string key;
        private void Start()
        {
            c_range = GetComponent<RangeComponent>();
            key = transform.GetInstanceID().ToString();
            BuildingSystem.S.components.Add(key,this);
        }

        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                foreach(var item in c_range.currentRange)
                {
                    Debug.Log("ÅÐ¶Ï½¨Öþ" + item);
                    if (BuildingSystem.S.buildings.ContainsKey(item))
                    {
                        TaskSystem.s.Execute(string.Format("FinishBuilding {0} {1}", key, item));
                        break;
                    }
                }
                
            }
        }

    }
}

