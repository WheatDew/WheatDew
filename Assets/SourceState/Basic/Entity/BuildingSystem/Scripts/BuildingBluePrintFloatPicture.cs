using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class BuildingBluePrintFloatPicture : MonoBehaviour
    {
        private RectTransform rectTransform;
        private BuildingBluePrintPage bluePrintPage;
        private Image image;
        private string buildingName;



        public void Update()
        {
            if (Input.GetMouseButton(0))
                MoveFloatPicture();
            if (Input.GetMouseButtonUp(0))
                Destroy(gameObject);
        }

        //初始化
        public void Init(string buildingName,Sprite sprite)
        {
            rectTransform = GetComponent<RectTransform>();
            bluePrintPage = BuildingSystem.S.bluePrintPage;
            image = GetComponent<Image>();
            this.buildingName = buildingName;
            image.sprite = sprite;
        }

        //随鼠标移动
        public void MoveFloatPicture()
        {
            if (rectTransform != null)
            {
                rectTransform.position = Input.mousePosition;

                if (Input.mousePosition.x > bluePrintPage.rectTransform.position.x + bluePrintPage.rectTransform.sizeDelta.x / 2
                    || Input.mousePosition.x < bluePrintPage.rectTransform.position.x - bluePrintPage.rectTransform.sizeDelta.x / 2
                    || Input.mousePosition.y > bluePrintPage.rectTransform.position.y + bluePrintPage.rectTransform.sizeDelta.y / 2
                    || Input.mousePosition.y < bluePrintPage.rectTransform.position.y - bluePrintPage.rectTransform.sizeDelta.y / 2)
                {

                    Destroy(gameObject);
                    BuildingSystem.S.CreateBuildingBluePrint(buildingName);
                }
            }

        }
    }
}

