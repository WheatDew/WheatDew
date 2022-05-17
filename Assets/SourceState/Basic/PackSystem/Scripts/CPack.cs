using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CPack : MonoBehaviour
    {

        public Dictionary<string, uint> packItems = new Dictionary<string, uint>();

        private void Start()
        {
            SPack.S.cpackList.Add(name, this);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SPack.S.SwitchPackPage(name);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SPack.S.GainPackItem(name, "测试", 20);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SPack.S.LosePackItem(name, "测试", 20);
            }
        }
    }
}

