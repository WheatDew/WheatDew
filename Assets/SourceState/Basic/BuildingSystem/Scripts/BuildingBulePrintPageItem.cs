using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Origin
{
    public class BuildingBulePrintPageItem : MonoBehaviour, IPointerDownHandler
    {
        public Sprite sprite;
        public string buildingName;
        public List<string> itemName =new List<string>();
        public List<uint> count = new List<uint>();

        private void Start()
        {
            Init();
        }

        //ÃÌº”¿∂Õº–Ë«Û
        public void Init()
        {
            Dictionary<string, ItemData> requirement = new Dictionary<string, ItemData>();
            for (int i = 0; i < itemName.Count; i++)
            {
                requirement.Add(itemName[i], new ItemData(itemName[i],count[i]));
            }
            if (BuildingSystem.S.requirements.ContainsKey(buildingName))
                BuildingSystem.S.requirements[buildingName] = new BuildingRequirement(requirement);
            else
                BuildingSystem.S.requirements.Add(buildingName, new BuildingRequirement(requirement));
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            BuildingSystem.S.CreateFloatPicture(buildingName,sprite);
        }
    }
}

