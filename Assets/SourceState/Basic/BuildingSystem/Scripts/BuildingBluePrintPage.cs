using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class BuildingBluePrintPage : MonoBehaviour
    {
        public RectTransform rectTransform;
        public PackComponent cpack;
        public BuildingBulePrintPageItem itemPrefab;
        public Transform contentTransform;

        public void CreateItem(Sprite icon,string buildingName)
        {
            BuildingBulePrintPageItem item = Instantiate(itemPrefab, contentTransform);
            item.sprite = icon;
            item.buildingName = buildingName;
        }
    }
}

