using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickComponent : MonoBehaviour
    {
        public string key;

        //开始运行时将自身添加到列表
        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            PickSystem.s.components.Add(key, this);
        }


        private void Update()
        {
            Job();
        }
        virtual public InfoData Job()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                PickSystem.s.CreateFocusPage();
            }

            if (Input.GetKey(KeyCode.C))
            {
                PickSystem.s.DisplayFocusItemInfo();
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                PickSystem.s.PickCheckout(key);
            }

            return null;
        }
    }
}

