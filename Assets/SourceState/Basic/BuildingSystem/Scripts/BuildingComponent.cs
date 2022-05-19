using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingComponent : MonoBehaviour
    {
        public bool isPrepare = false;

        public string buildingName = "default";

        private void Update()
        {

            //¿∂ÕºΩÁ√Ê
            if (Input.GetKeyDown(KeyCode.B))
            {
                BuildingSystem.S.SwitchBluePrintPage();
            }
            
        }

    }
}

