using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{
    public class PackPage : MonoBehaviour
    {
        public Transform content;

        public PackPageItem packItemPrefab;

        public string targetPack;

        public Dictionary<string, PackPageItem> itemList = new Dictionary<string, PackPageItem>();

        public Button backButton;

        private void Start()
        {
            backButton.onClick.AddListener(delegate
            {
                SelectionSystem.s.selectionMenu.gameObject.SetActive(true);
                Destroy(gameObject);
            });
        }

        private void Update()
        {
            UpdataPackPage();
        }

        public void UpdataPackPage()
        {
            Dictionary<string, Item> targetList = PackSystem.S.components[targetPack].pack;

            Dictionary<string, PackPageItem> recordList = itemList;

            HashSet<string> removeList = new HashSet<string>();

            foreach (var item in recordList)
            {
                if (!targetList.ContainsKey(item.Key))
                {
                    Destroy(item.Value.gameObject);
                    removeList.Add(item.Key);
                }
                else if (targetList[item.Key].count == 0)
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
                    PackPageItem obj = Instantiate(packItemPrefab, content);
                    obj.target = targetPack;
                    obj.itemName.text = item.Value.name;
                    obj.itemCount.text = item.Value.count.ToString();

                    recordList.Add(item.Key, obj);
                }
            }


        }
    }
}


