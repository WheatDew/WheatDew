using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{

    /*
     检测的层级要求为7，无名称要求
     */

    public class BuildingSystem : MonoBehaviour
    {
        private static BuildingSystem _s;
        public static BuildingSystem S { get { return _s; } }

        //引用的预制体列表
        public List<Building> buildingPrefabList = new List<Building>(); //测试
        public List<string> buildingPrefabNameList = new List<string>();//测试

        public Material tmMat; //透明材质

        public Material baseMaterial;

        private void Start()
        {
            if (!_s) _s = this;

            Init();
        }

        //场景中的引用
        public Dictionary<string, BuildingComponent> cbuildingList = new Dictionary<string, BuildingComponent>();

        //资源库
        public Dictionary<string, Building> buildingList = new Dictionary<string, Building>();

        //单一引用的预制体
        [SerializeField] private BuildingBluePrintFloatPicture floatPicturePrefab;
        [HideInInspector] private BuildingBluePrintFloatPicture floatPicture;
        [SerializeField] private BuildingBluePrintPage bluePrintPagePrefab;
        [HideInInspector] public BuildingBluePrintPage bluePrintPage;

        //初始化函数
        public void Init()
        {
            for(int i = 0; i < buildingPrefabList.Count; i++)
            {
                buildingList.Add(buildingPrefabNameList[i], buildingPrefabList[i]);
            }
        }

        //打开关闭蓝图界面
        public void SwitchBluePrintPage()
        {
            if (bluePrintPage == null)
            {
                bluePrintPage = Instantiate(bluePrintPagePrefab, FindObjectOfType<Canvas>().transform);
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
            Building obj = Instantiate(buildingList[buildingName]);
            SetTranslucentMaterial(obj.transform);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                Collider collider = obj.transform.GetChild(i).GetComponent<Collider>();

                if (collider != null)
                    collider.isTrigger = true;
            }
            obj.gameObject.AddComponent<BuildingBluePrint>().buildingName=buildingName;

        }

        //创建建筑
        public void CreateBuiling(string buildingName,Vector3 position,Quaternion rotation)
        {
            Instantiate(buildingList[buildingName],position,rotation);

        }


        #region 功能函数
        //功能函数

        //将对象材质改为半透明材质
        public void SetTranslucentMaterial(Transform target)
        {

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
}

