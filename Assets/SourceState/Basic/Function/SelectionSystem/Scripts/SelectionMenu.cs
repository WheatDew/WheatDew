using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class SelectionMenu : MonoBehaviour
    {
        public SelectionMenuButton buttonPrefab;
        public Transform parent;

        private void Start()
        {
            CreateButton();
        }

        public void CreateButton()
        {
            SelectionMenuButton button;
            button = Instantiate(buttonPrefab, parent);
            button.Init("Building","SwitchBluePrintPage");

        }
    }
}

