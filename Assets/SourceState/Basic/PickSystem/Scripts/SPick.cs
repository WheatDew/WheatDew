using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SPick : MonoBehaviour
    {
        private static SPick _s;
        public static SPick S { get { return _s; } }
        //组件列表
        public Dictionary<string, CPick> pickList = new Dictionary<string, CPick>();

        //功能所需变量
        public CFocus focusPrefab;
        private CFocus focus;
        private GameObject focusItem;

        private void Start()
        {
            if (!_s) _s = this;
        }

        public int BeforePicking()
        {
            if (focus == null)
                focus = Instantiate(focusPrefab, FindObjectOfType<Canvas>().transform);
            return 0;
        }


        public int DuringPicking()
        {
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100, 1 << 6))
            {
                focus.focusName.text = result.collider.name;
                focusItem = result.collider.gameObject;
                if (focusItem.transform.parent != null && focusItem.transform.parent.gameObject.layer == LayerMask.NameToLayer("PickItem"))
                {
                    focusItem = focusItem.transform.parent.gameObject;
                }
            }
            else
            {
                focusItem = null;
                focus.focusName.text = "";
            }
            return 0;
        }

        public int AfterPicked(string name)
        {

            Destroy(focus.gameObject);
            if (focusItem != null)
            {
                CPickItem pickItem = focusItem.GetComponent<CPickItem>();
                if (pickItem != null)
                {
                    SPack.S.GainPackItem(name, pickItem.pickItemName, pickItem.pickItemCount);
                    Destroy(focusItem);
                }
            }

            
            return 0;
        }
    }

}

