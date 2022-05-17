using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CPick : MonoBehaviour
    {

        //开始运行时将自身添加到列表
        private void Start()
        {
            FindObjectOfType<SPick>().pickList.Add(name, this);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SPick.S.BeforePicking();
            }

            if (Input.GetKey(KeyCode.C))
            {
                SPick.S.DuringPicking();
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                SPick.S.AfterPicked(name);
            }
        }
    }
}

