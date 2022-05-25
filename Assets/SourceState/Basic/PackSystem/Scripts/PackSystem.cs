using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackSystem : MonoBehaviour
    {
        private static PackSystem _s;
        public static PackSystem S { get { return _s; } }

        public PackPage packPagePrefab;
        private PackPage packPage;

        //右键按钮菜单
        public RightMenu rightMenu;
        public RightMenuItem rightMenuItem;

        //组件列表
        public Dictionary<string, PackComponent> PackList = new Dictionary<string, PackComponent>();


        private void Awake()
        {
            if (!_s) _s = this;


        }

        private void Start()
        {
            CommandSystem.S.Declare("GainPackItem", PackItemGainCommand);
        }

        //命令
        public InfoData PackItemGainCommand(params string[] values)
        {
            string s = "PackItemGainCommand";
            foreach(var item in values)
            {
                s += item + " ";
            }
            Debug.Log(s);
            PackList[values[0]].PackItemGain(values[1], int.Parse(values[2]));
            return null;
        }

        public void CreatePackPage(string name)
        {
            if (packPage == null)
            {
                packPage = Instantiate(packPagePrefab, FindObjectOfType<Canvas>().transform);
                packPage.targetPack = name;
            }
        }

        public int SwitchPackPage(string name)
        {
            if (packPage == null)
            {
                packPage = Instantiate(packPagePrefab, FindObjectOfType<Canvas>().transform);
                packPage.targetPack = name;
            }
            else
                Destroy(packPage.gameObject);

            return 0;
        }

        //创建右键菜单
        public void CreateItemRightMenu(string target,PackPageItem packPageItem,List<CommandButton> buttons)
        {
            RightMenu rightMenuEntity = Instantiate(rightMenu, FindObjectOfType<Canvas>().transform);
            List<CommandButton> commandButtons = new List<CommandButton>();
            commandButtons.Add(new CommandButton("增加", string.Format("{0},{1},{2},1", "GainPackItem",
                target,packPageItem.itemName.text, packPageItem.itemCount.text)));
            rightMenuEntity.Init(commandButtons,rightMenuItem);
            
        }

        
        public void OpenTargetPack(string selfKey)
        {
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out result, 100, 1 << 6))
            {
                print("射线检测成功");
                BuildingPack buildingPack = result.collider.GetComponent<BuildingPack>();
                Dictionary<string,ItemData> requirement = buildingPack.requirement;
                Dictionary<string, ItemData> pack = PackList[selfKey].pack;
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
        public int count;

        public ItemData(string name,int count)
        {
            this.name = name;
            this.count = count;
        }
    }

    public class CommandButton
    {
        public string name;
        public string command;

        public CommandButton(string name,string command)
        {
            this.name = name;
            this.command = command;
        }
    }
}
