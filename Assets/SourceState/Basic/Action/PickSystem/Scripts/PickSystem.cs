using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickSystem : MonoBehaviour
    {
        private static PickSystem _s;
        public static PickSystem s { get { return _s; } }
        //组件列表
        public Dictionary<string, PickComponent> pickList = new Dictionary<string, PickComponent>();
        public Dictionary<string,PickItem> pickItems = new Dictionary<string, PickItem>();

        //功能所需变量
        public CFocus focusPrefab;
        private CFocus focus;

        private void Awake()
        {
            if (!_s) _s = this;

            
        }

        private void Start()
        {
            CommandSystem.S.Declare("GetClosestItem", GetClosestItemCommand);
            CommandSystem.S.Declare("AppraiseTargetDistance", AppraiseTargetDistanceCommand);
            CommandSystem.S.Declare("PickItem", PickItemCommand);
        }

        #region 命令函数

        public InfoData GetClosestItemCommand(string[] values)
        {
            InfoData infoData = new InfoData();

            infoData.stringValue = ClosestItem(pickList[values[1]].transform.position);
            infoData.intValue = 1;
            return infoData;
        }

        public InfoData AppraiseTargetDistanceCommand(string[] values)
        {
            InfoData infoData = new InfoData();

            if (Vector3.Distance(pickList[values[1]].transform.position,
                pickItems[values[2]].transform.position) <= 2)
            {
                infoData.intValue = 1;
                infoData.stringValue = values[2];
            }
            else
                infoData.intValue = 0;

            return infoData;
        }

        public InfoData PickItemCommand(string[] values)
        {
            InfoData infoData = new InfoData();

            PickItem(values[1], values[2]);
            infoData.intValue = 1;

            return infoData;
        }

        #endregion


        #region 功能函数

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
                    PackSystem.S.PackList[name].PackItemGain(pickItem.pickItemName, pickItem.pickItemCount);
                    Destroy(result.collider.gameObject);
                }


            }        
        }

        public void PickItem(string component,string item)
        {
            PackSystem.S.PackList[component].PackItemGain(
                pickItems[item].pickItemName, pickItems[item].pickItemCount);
            Destroy(pickItems[item].gameObject);
        }

        public string ClosestItem(Vector3 self)
        {
            string result=null;
            float distance = 9999;
            foreach(var item in pickItems)
            {
                float tempDistance = Vector3.Distance(item.Value.transform.position, self);
                if (Vector3.Distance(item.Value.transform.position, self) < distance)
                {
                    result = item.Key;
                    distance = tempDistance;
                }
            }

            return result;
        }

        

        #endregion

    }

}

