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


        public void ButtonInit(HashSet<string> values,string key)
        {
            for(int i = 0; i < parent.childCount; i++)
            {
                print("destroy" + parent.GetChild(i).name);
                Destroy(parent.GetChild(i).gameObject);
            }
            foreach(var item in values)
            {
                string[] aguments = item.Split(' ');
                SelectionMenuButton button;
                button = Instantiate(buttonPrefab, parent);
                button.Init(aguments[0], aguments[1],key);
            }


        }
    }
}

