using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackComponent : MonoBehaviour
    {

        public Dictionary<string, ItemData> pack = new Dictionary<string, ItemData>();


        private void Start()
        {
            PackSystem.S.cpackList.Add(name, this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                PackSystem.S.SwitchPackPage(name);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PackSystem.S.GainPackItem(name, "测试", 20);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PackSystem.S.LosePackItem(name, "测试", 20);
            }
        }
    }
}

