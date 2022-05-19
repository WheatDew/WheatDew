using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class UPack : MonoBehaviour
    {
        public Transform content;

        public CPackItem packItemPrefab;

        public PackComponent targetPack;

        public Dictionary<string, CPackItem> itemList = new Dictionary<string, CPackItem>();

        private void Update()
        {
            UpdataPackPage();
        }

        public void UpdataPackPage()
        {
            Dictionary<string, ItemData> targetList = targetPack.pack;

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
                    recordList[item.Key].itemName.text = item.Value.name;
                    recordList[item.Key].itemCount.text = item.Value.count.ToString();
                }
                else
                {
                    CPackItem obj = Instantiate(packItemPrefab, content);
                    obj.itemName.text = item.Value.name;
                    obj.itemCount.text = item.Value.count.ToString();
                    recordList.Add(item.Key, obj);
                }
            }


        }
    }
}


