using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Origin
{
    public class PickSystem : MonoBehaviour
    {
        private static PickSystem _s;
        public static PickSystem s { get { return _s; } }
        //组件列表
        public Dictionary<string, PickComponent> components = new Dictionary<string, PickComponent>();
        public Dictionary<string,PickItem> items = new Dictionary<string, PickItem>();

        //功能所需变量
        public CFocus focusPrefab;
        private CFocus focus;

        private void Awake()
        {
            if (!_s) _s = this;

            
        }

        private void Start()
        {
            TaskSystem.s.Declare("PickClosestItem", PickCloseItemTask);
        }

        #region 命令函数

        public InfoData GetClosestItemCommand(string[] values)
        {
            InfoData infoData = new InfoData();

            infoData.stringValue = ClosestItem(components[values[1]].transform.position);
            infoData.intValue = 1;
            return infoData;
        }

        public InfoData AppraiseTargetDistanceCommand(string[] values)
        {
            InfoData infoData = new InfoData();

            if (Vector3.Distance(components[values[1]].transform.position,
                items[values[2]].transform.position) <= 2)
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

        #region 行为函数

        public async void PickCloseItemTask(string[] values,TaskData taskData)
        {
            Transform proposer = components[values[1]].transform;
            string itemName = ClosestItem(proposer.position);
            Transform target = items[itemName].transform;

            PathFindingSystem.s.SetTargetPosition(values[1],target.position);

            while (Vector3.Distance(target.position, proposer.position) > 2)
            {
                await new WaitForSeconds(Time.deltaTime);
            }
            PickItem(values[1], itemName);
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100,1<<9))
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100,1<<9))
            {
                if(result.collider.tag == "PickItem")
                {
                    PickItem pickItem = result.collider.gameObject.GetComponent<PickItem>();
                    PackSystem.S.components[name].PackItemGain(pickItem.pickItemName, pickItem.pickItemCount);
                    Destroy(result.collider.gameObject);
                }
            }        
        }

        public void PickItem(string component,string item)
        {
            print("pickItem " + component + " " + item);
            PackSystem.S.components[component].PackItemGain(
                items[item].pickItemName, items[item].pickItemCount);
            items[item].Destroy();
        }

        public string ClosestItem(Vector3 self)
        {
            string result=null;
            float distance = 9999;
            print(items.Count);
            foreach(var item in items)
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

        public void PrintInfo()
        {
            print(components.Count);
            foreach(var item in components)
            {
                print(item.Key);
            }
        }

        #endregion

    }

}

