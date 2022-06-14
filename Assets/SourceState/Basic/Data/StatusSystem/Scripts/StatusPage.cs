using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class StatusPage : MonoBehaviour
    {
        public TextMeshProUGUI nameText;
        public Image foodBar;
        StatusData data;

        public Button backButton;

        private void Start()
        {
            backButton.onClick.AddListener(delegate
            {
                SelectionSystem.s.selectionMenu.gameObject.SetActive(true);
                Destroy(gameObject);
            });
        }

        public void Init(StatusData data)
        {
            this.data = data;
        }

        private void Update()
        {
            nameText.text = data.name;
            foodBar.rectTransform.localScale =  new Vector3(data.food/50,1,1);
        }
    }
}

