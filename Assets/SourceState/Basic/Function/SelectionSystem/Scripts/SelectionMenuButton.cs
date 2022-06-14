using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class SelectionMenuButton : MonoBehaviour
    {
        public Button button;
        public TextMeshProUGUI buttonText;
        public string command;
        public string key;

        public void Init(string buttonText,string command,string key)
        {
            this.buttonText.text = buttonText;
            this.command = command;
            this.key = key;
            button.onClick.AddListener(delegate
            {
                ButtonCommand();
            });
        }

        public void ButtonCommand()
        {
            CommandSystem.s.Execute(command + " " +key);
            SelectionSystem.s.selectionMenu.gameObject.SetActive(false);
        }
    }
}

