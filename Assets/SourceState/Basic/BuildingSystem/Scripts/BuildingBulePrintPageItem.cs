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
        public List<int> count = new List<int>();

        public void OnPointerDown(PointerEventData eventData)
        {
            BuildingSystem.S.CreateFloatPicture(buildingName,sprite);
        }
    }
}

