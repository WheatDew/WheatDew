using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Origin
{
    public class BuildingPack : PackComponent
    {
        public Dictionary<string, ItemData> requirement = new Dictionary<string, ItemData>();
        public TextMeshPro info;

        public BuildingComponent building;

        private void Start()
        {
            Init();
            building=GetComponent<BuildingComponent>();
            UpdateRequirementInfo();
        }

        public void CheckRequirement()
        {
            UpdateRequirementInfo();
            foreach (var item in requirement)
            {
                if (item.Value.count != 0)
                    return;
            }
            BuildingSystem.S.CreateBuiling(building.buildingTypeName, transform);
            Destroy(gameObject);

        }

        public void UpdateRequirementInfo()
        {
            string s = "";
            foreach (var item in requirement)
            {
                s += item.Key + ":" + item.Value.count.ToString() + '\n';
            }

            info.text = s;
        }
    }


}
