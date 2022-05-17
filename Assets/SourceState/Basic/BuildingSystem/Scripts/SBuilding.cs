using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SBuilding : MonoBehaviour
    {
        private static SBuilding _s;
        public static SBuilding S { get { return _s; } }

        //引用的预制体列表
        public List<CBuildingItem> buildingPrefabList = new List<CBuildingItem>();

        public Material tmMat; //透明材质

        public Material baseMaterial;

        private void Start()
        {
            if (!_s) _s = this;
        }

        //场景中的引用
        public Dictionary<string, CBuilding> cbuildingList = new Dictionary<string, CBuilding>();
        //public static Dictionary<string, CBuildingItem> buildingItemList = new Dictionary<string, CBuildingItem>();

        private CBuildingItem currentBuildingItem;


        //放置前准备
        public int CheckBlueprint(string buildingName)
        {
            print("PrepareBuilding:"+buildingName);
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out result, 100, 1 << 7))
            {
                if (currentBuildingItem == null)
                {
                    currentBuildingItem = Instantiate(buildingPrefabList[0]);
                    SetTranslucentMaterial(currentBuildingItem.transform);
                    currentBuildingItem.name = buildingName;
                }
                currentBuildingItem.transform.position = result.point;
            }
            return 0;
        }

        //放置
        public int SetBuilding()
        {
            currentBuildingItem = null;
            return 0;
        }


        public string SetBuildingBasicState(string value)
        {
            
            return "";
        }

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

    }
}

