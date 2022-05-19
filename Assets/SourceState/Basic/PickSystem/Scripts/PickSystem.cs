using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickSystem : MonoBehaviour
    {
        private static PickSystem _s;
        public static PickSystem S { get { return _s; } }
        //组件列表
        public Dictionary<string, PickComponent> pickList = new Dictionary<string, PickComponent>();

        //功能所需变量
        public CFocus focusPrefab;
        private CFocus focus;

        private void Start()
        {
            if (!_s) _s = this;
        }

        public int CreateFocusPage()
        {
            if (focus == null)
                focus = Instantiate(focusPrefab, FindObjectOfType<Canvas>().transform);
            return 0;
        }


        public int DisplayFocusItemInfo()
        {
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100,1<<6))
            {
                focus.focusName.text = result.collider.name;
            }
            else
            {
                focus.focusName.text = "";
            }
            return 0;
        }

        public void PickCheckout(string name)
        {
            Destroy(focus.gameObject);

            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100,1<<6))
            {
                if(result.collider.tag == "PickItem")
                {
                    PickItem pickItem = result.collider.gameObject.GetComponent<PickItem>();
                    PackSystem.S.GainPackItem(name, pickItem.pickItemName, pickItem.pickItemCount);
                    Destroy(result.collider.gameObject);
                }


            }        
        }
    }

}

