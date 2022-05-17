using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CBuilding : MonoBehaviour
    {
        public bool isPrepare = false;

        public string buildingName = "default";

        private void Start()
        {
            //SBuilding.cbuildingList.Add("test", this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isPrepare = true;

            }

            if (Input.GetMouseButtonDown(0))
            {
                isPrepare = false;
                SBuilding.S.SetBuilding();
            }

            if (isPrepare)
            {
                SBuilding.S.CheckBlueprint("test");
            }
        }

    }
}

