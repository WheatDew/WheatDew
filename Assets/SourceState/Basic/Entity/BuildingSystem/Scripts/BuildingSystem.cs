using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{


    public class BuildingSystem : MonoBehaviour
    {
        private static BuildingSystem _s;
        public static BuildingSystem S { get { return _s; } }

        //引用的预制体列表
        public List<BuildingComponent> buildingPrefabList = new List<BuildingComponent>(); //测试
        public List<string> buildingDataNameList = new List<string>();//测试
        public List<string> buildingRequirementList = new List<string>();//测试
        public List<Sprite> buildingIconList = new List<Sprite>();//测试

        public Material tmMat; //透明材质

        public Material baseMaterial;

        private void Start()
        {
            if (!_s) _s = this;

            BasicInit();

            CommandInit();
        }

        //场景中的引用
        public Dictionary<string, CharacterBuildingComponent> components = new Dictionary<string, CharacterBuildingComponent>();

        //建筑组件
        public Dictionary<string, BuildingComponent> buildings = new Dictionary<string, BuildingComponent>();
        //资源库
        public Dictionary<string, BuildingData> BuildingDataList = new Dictionary<string, BuildingData>();
        
        //单一引用的预制体
        [SerializeField] private BuildingBluePrintFloatPicture floatPicturePrefab;
        [HideInInspector] private BuildingBluePrintFloatPicture floatPicture;
        [SerializeField] private BuildingBluePrintPage bluePrintPagePrefab;
        [HideInInspector] public BuildingBluePrintPage bluePrintPage;

        //初始化函数
        public void BasicInit()
        {
            for(int i = 0; i < buildingPrefabList.Count; i++)
            {
                BuildingData buildingData = new BuildingData();
                buildingData.icon = buildingIconList[i];
                buildingData.building = buildingPrefabList[i];
                Dictionary<string, ItemData> requirement = new Dictionary<string, ItemData>();
                string[] requirements = buildingRequirementList[i].Split('&');
                for(int j = 0; j < requirements.Length; j++)
                {
                    string[] Items = requirements[j].Split(' ');
                    requirement.Add(Items[0], new ItemData(Items[0], int.Parse(Items[1])));
                }
                buildingData.requirement = requirement;
                BuildingDataList.Add(buildingDataNameList[i],buildingData);
            }
        }

        #region 命令

        public void CommandInit()
        {
            CommandSystem.s.Declare("SwitchBluePrintPage", SwitchBluePrintPageCommand);
        }

        public InfoData SwitchBluePrintPageCommand(string[] values)
        {
            SwitchBluePrintPage();

            return null;
        }

        #endregion


        //打开关闭蓝图界面
        public void SwitchBluePrintPage()
        {
            if (bluePrintPage == null)
            {
                bluePrintPage = Instantiate(bluePrintPagePrefab, FindObjectOfType<Canvas>().transform);
                foreach(var item in BuildingDataList)
                {
                    bluePrintPage.CreateItem(item.Value.icon,item.Key);
                }
            }
            else
                Destroy(bluePrintPage.gameObject);
        }

        //创建浮动蓝图
        public void CreateFloatPicture(string buildingName,Sprite sprite)
        {
            if (floatPicture == null)
            {
                floatPicture = Instantiate(floatPicturePrefab, FindObjectOfType<Canvas>().transform);
            }
            floatPicture.Init(buildingName, sprite);
        }

        //创建蓝图
        public void CreateBuildingBluePrint(string buildingName)
        {
            BuildingComponent bluePrint = Instantiate(BuildingDataList[buildingName].building);
            bluePrint.buildingTypeName = buildingName;
            bluePrint.isbluePrint=true;
            SetTranslucentMaterial(bluePrint.transform);
            for (int i = 0; i < bluePrint.transform.childCount; i++)
            {
                Collider collider = bluePrint.transform.GetChild(i).GetComponent<Collider>();

                if (collider != null)
                    collider.isTrigger = true;
            }
            bluePrint.gameObject.AddComponent<BuildingBluePrint>().buildingName=buildingName;
            bluePrint.GetComponent<BuildingPack>().requirement = new Dictionary<string, ItemData>(BuildingDataList[buildingName].requirement);

        }

        //创建建筑
        public void CreateBuiling(string buildingName,Transform origin)
        {
            BuildingComponent obj = Instantiate(BuildingDataList[buildingName].building);
            obj.transform.position = origin.position;
            obj.transform.rotation = origin.rotation;
        }


        #region 功能函数
        //功能函数

        //将对象材质改为半透明材质
        public void SetTranslucentMaterial(Transform target)
        {

            Material[] newTmMatArray;//透明材质数组

            for (int i = 0; i < target.childCount; i++)
            {
                if (target.GetChild(i).tag != "NoTranslucent")
                {
                    MeshRenderer meshRenderer = target.GetChild(i).GetComponent<MeshRenderer>();

                    newTmMatArray = new Material[meshRenderer.materials.Length];//数组初始化长度
                                                                                //获取全部材质
                    for (int j = 0; j < newTmMatArray.Length; j++)
                    {
                        newTmMatArray[j] = tmMat;
                    }

                    meshRenderer.materials = newTmMatArray;//将透明数组赋给模型材质
                }

            }
        }

        //将对象材质改为半透明蓝色材质
        public void SetTranslucentBlueMaterial(Transform target)
        {

            print(target.name + " " + target.childCount.ToString());
            Material[] newTmMatArray;//透明材质数组

            for (int i = 0; i < target.childCount; i++)
            {
                MeshRenderer meshRenderer = target.GetChild(i).GetComponent<MeshRenderer>();

                newTmMatArray = new Material[meshRenderer.materials.Length];//数组初始化长度
                                                                            //获取全部材质
                for (int j = 0; j < newTmMatArray.Length; j++)
                {
                    newTmMatArray[j] = tmMat;
                }

                meshRenderer.materials = newTmMatArray;//将透明数组赋给模型材质
            }
        }

        #endregion
    }

    public class BuildingData
    {
        public Sprite icon;
        public BuildingComponent building;
        public Dictionary<string, ItemData> requirement = new Dictionary<string, ItemData>();
    }
}

