using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackSystem : MonoBehaviour
    {
        private static PackSystem _s;
        public static PackSystem S { get { return _s; } }

        public UPack packPagePrefab;
        private UPack packPage;

        //组件列表
        public Dictionary<string, PackComponent> cpackList = new Dictionary<string, PackComponent>();

        private void Awake()
        {
            if (!_s) _s = this;
        }

        public int CreatePackPage(string name)
        {
            if (packPage == null)
            {
                packPage = Instantiate(packPagePrefab, FindObjectOfType<Canvas>().transform);
                packPage.targetPack = cpackList[name];
            }

            return 0;
        }

        public int SwitchPackPage(string name)
        {
            if (packPage == null)
            {
                packPage = Instantiate(packPagePrefab, FindObjectOfType<Canvas>().transform);
                packPage.targetPack = cpackList[name];
            }
            else
                Destroy(packPage.gameObject);

            return 0;
        }

        //添加角色背包物品 [名字]&[物品名字]&[数量]
        public int GainPackItem(string component,string item,uint count)
        {
            PackComponent target = cpackList[component];
            if (!target.pack.ContainsKey(item))
                target.pack.Add(item, new ItemData(item,count));
            else
                target.pack[item].count += count;

            return 0;
        }

        public string LosePackItem(string component,string item,uint count)
        {
            PackComponent target = cpackList[component];
            if (!target.pack.ContainsKey(item))
                return "不存在该物品";
            else if (target.pack[item].count < count)
                return "物品数量不够";
            else
            {
                target.pack[item].count -= count;
                if (target.pack[item].count == 0)
                    target.pack.Remove(item);
            }
            return "执行成功";
        }

        
        public void OpenTargetPack(string selfKey)
        {
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100, 1 << 6))
            {
                print("射线检测成功");
                BuildingPack buildingPack = result.collider.GetComponent<BuildingPack>();
                Dictionary<string,ItemData> requirement = buildingPack.requirement;
                Dictionary<string, ItemData> pack = cpackList[selfKey].pack;
                if (requirement != null)
                {
                    foreach(var item in requirement)
                    {
                        if (pack.ContainsKey(item.Key))
                        {
                            if (pack[item.Key].count > requirement[item.Key].count)
                            {
                                pack[item.Key].count -= requirement[item.Key].count;
                                requirement[item.Key].count = 0;
                            }
                            else
                            {
                                requirement[item.Key].count -= pack[item.Key].count;
                                pack[item.Key].count = 0;
                            }
                        }
                    }
                    buildingPack.CheckRequirement();
                }
            }
        }
    }

    public class ItemData
    {
        public string name;
        public uint count;

        public ItemData(string name,uint count)
        {
            this.name = name;
            this.count = count;
        }

        public bool Greater(ItemData other)
        {
            if (other.name == name && other.count < count)
            {
                return true;
            }
            return false;
        }
        public bool Less(ItemData other)
        {
            if(other.name == name && other.count > count)
            {
                return true;
            }
            return false;
        }
        public bool Equality(ItemData other)
        {
            if(other.name == name && other.count == count)
            {
                return true;
            }
            return false;
        }
    }
}
