using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickComponent : MonoBehaviour
    {

        //开始运行时将自身添加到列表
        private void Start()
        {
            //FindObjectOfType<PickSystem>().pickList.Add(name, this);
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                PickSystem.S.CreateFocusPage();
            }

            if (Input.GetKey(KeyCode.C))
            {
                PickSystem.S.DisplayFocusItemInfo();
            }

            if (Input.GetKeyUp(KeyCode.C))
            {
                PickSystem.S.PickCheckout(GetComponent<CCharacter>().key);
            }
        }
    }
}

