using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackComponent : MonoBehaviour
    {
        public string key;
        public Dictionary<string, ItemData> pack = new Dictionary<string, ItemData>();


        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            key = transform.GetInstanceID().ToString();
            PackSystem.S.PackList.Add(key, this);
        }

        //功能函数
        public InfoData PackItemGain(string itemName, int itemCount)
        {
            InfoData infoData = new InfoData();


            if (pack.ContainsKey(itemName))
            {
                if (pack[itemName].count + itemCount < 0)
                {
                    infoData.stringValue = "物品数量不能为负数";
                    infoData.intValue = -2;
                    return infoData;
                }
                else
                {
                    pack[itemName].count += itemCount;
                }
            }
            else
            {
                if (itemCount > 0)
                {
                    pack.Add(itemName, new ItemData(itemName, itemCount));
                }
                else
                {
                    infoData.stringValue = "物品数量不能为负数";
                    infoData.intValue = -1;
                    return infoData;
                }
            }

            return infoData;
        }
    }
}

