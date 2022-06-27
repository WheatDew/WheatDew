using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CharacterBuildingComponent : MonoBehaviour
    {
        public bool isPrepare = false;

        public string buildingName = "default";

        string key;
        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            BuildingSystem.S.components.Add(key,this);
        }

        private void Update()
        {

            //¿∂ÕºΩÁ√Ê
            if (Input.GetKeyDown(KeyCode.B))
            {
                BuildingSystem.S.SwitchBluePrintPage();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                NewCommandSystem.Execute(string.Format("FinishBuilding {0}",key));
            }
        }

    }
}

