using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Origin
{
    public class BuildingPack : PackComponent
    {
        public Dictionary<string, int> requirement = new Dictionary<string, int>();
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
                if (item.Value != 0)
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
                s += item.Key + ":" + item.Value.ToString() + '\n';
            }

            info.text = s;
        }
    }


}
