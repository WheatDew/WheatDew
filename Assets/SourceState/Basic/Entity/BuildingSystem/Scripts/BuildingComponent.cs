using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingComponent : MonoBehaviour
    {
        public bool isPrepare = false;

        public string buildingName = "default";

        private void Start()
        {
            print(transform.GetInstanceID().ToString());
            BuildingSystem.S.components.Add(transform.GetInstanceID().ToString(), this);
        }

        private void Update()
        {

            //��ͼ����
            if (Input.GetKeyDown(KeyCode.B))
            {
                BuildingSystem.S.SwitchBluePrintPage();
            }
            
        }

    }
}

