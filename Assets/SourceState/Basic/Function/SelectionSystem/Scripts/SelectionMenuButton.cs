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

        public void Init(string buttonText,string command)
        {
            this.buttonText.text = buttonText;
            this.command = command;
            button.onClick.AddListener(delegate
            {
                ButtonCommand();
            });
        }

        public void ButtonCommand()
        {
            CommandSystem.s.Execute(command + " " +
                SelectionSystem.s.currentSelection.transform.GetInstanceID());
        }
    }
}

