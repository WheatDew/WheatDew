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

        private void Start()
        {
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

            BuildingSystem.S.CreateBuiling(GetComponent<BuildingBluePrint>().buildingName,transform.position,transform.rotation);
            Destroy(gameObject);

        }

        public void UpdateRequirementInfo()
        {
            string s = "";
            foreach (var item in requirement)
            {
                s += item.Value.name + ":" + item.Value.count.ToString() + '\n';
            }

            info.text = s;
        }
    }


}
