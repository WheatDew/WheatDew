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

        public void OnPointerDown(PointerEventData eventData)
        {
            BuildingSystem.S.CreateFloatPicture(buildingName,sprite);
        }
    }
}

