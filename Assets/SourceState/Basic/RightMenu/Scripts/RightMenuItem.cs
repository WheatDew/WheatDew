using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class RightMenuItem : MonoBehaviour
    {
        public TextMeshProUGUI textMeshPro;
        public string characterName;

        public void Init(string name,string command)
        {
            textMeshPro.text = name;
            GetComponent<Button>().onClick.AddListener(delegate
            {
                CommandSystem.S.Execute(command);
            });
        }


    }
}

