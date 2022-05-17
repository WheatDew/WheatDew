using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class UPack : MonoBehaviour
    {
        public Transform content;

        public CPackItem packItemPrefab;

        public CPack targetPack;

        public Dictionary<string, CPackItem> itemList = new Dictionary<string, CPackItem>();

        private void Update()
        {
            UpdataPackPage();
        }

        public void UpdataPackPage()
        {
            Dictionary<string, uint> targetList = targetPack.packItems;

            Dictionary<string, CPackItem> recordList = itemList;

            HashSet<string> removeList = new HashSet<string>();

            foreach (var item in recordList)
            {
                if (!targetList.ContainsKey(item.Key))
                {
                    Destroy(item.Value.gameObject);
                    removeList.Add(item.Key);
                }
            }

            foreach(var item in removeList)
            {
                recordList.Remove(item);
            }

            foreach (var item in targetList)
            {
                if (recordList.ContainsKey(item.Key))
                {
                    recordList[item.Key].itemName.text = item.Key;
                    recordList[item.Key].itemCount.text = item.Value.ToString();
                    print(string.Format("设置条目:{0}", item.Key));
                }
                else
                {
                    CPackItem obj = Instantiate(packItemPrefab, content);
                    obj.itemName.text = item.Key;
                    obj.itemCount.text = item.Value.ToString();
                    recordList.Add(item.Key, obj);
                    print(string.Format("创建条目:{0}", item.Key));
                }
            }


        }
    }
}


