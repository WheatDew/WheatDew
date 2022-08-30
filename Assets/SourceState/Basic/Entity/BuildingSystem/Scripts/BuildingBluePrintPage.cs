using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class BuildingBluePrintPage : MonoBehaviour
    {
        public RectTransform rectTransform;
        public PackComponent cpack;
        public BuildingBulePrintPageItem itemPrefab;
        public Transform contentTransform;
        public Button backButton;

        private void Start()
        {
            backButton.onClick.AddListener(delegate
            {
                SelectionSystem.s.selectionMenu.gameObject.SetActive(true);
                Destroy(gameObject);
            });
        }

        public void CreateItem(Sprite icon,string buildingName)
        {
            BuildingBulePrintPageItem item = Instantiate(itemPrefab, contentTransform);
            item.image.sprite = icon;
            item.buildingName = buildingName;
        }
    }
}

