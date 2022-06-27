using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingComponent : MonoBehaviour
    {
        public string buildingTypeName;
        public bool isbluePrint;

        private void Start()
        {
            BuildingSystem.S.buildings.Add(transform.GetInstanceID().ToString(), this);
        }

        public void OnDestroy()
        {
            BuildingSystem.S.buildings.Remove(transform.GetInstanceID().ToString());
        }
    }
}

