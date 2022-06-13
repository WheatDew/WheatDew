using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Origin
{

    /*
     ���Ĳ㼶Ҫ��Ϊ7��������Ҫ��
     */

    public class BuildingSystem : MonoBehaviour
    {
        private static BuildingSystem _s;
        public static BuildingSystem S { get { return _s; } }

        //���õ�Ԥ�����б�
        public List<Building> buildingPrefabList = new List<Building>(); //����
        public List<string> buildingDataNameList = new List<string>();//����
        public List<string> buildingRequirementList = new List<string>();//����
        public List<Sprite> buildingIconList = new List<Sprite>();//����

        public Material tmMat; //͸������

        public Material baseMaterial;

        private void Start()
        {
            if (!_s) _s = this;

            BasicInit();

            CommandInit();
        }

        //�����е�����
        public Dictionary<string, BuildingComponent> cbuildingList = new Dictionary<string, BuildingComponent>();

        //��Դ��
        public Dictionary<string, BuildingData> BuildingDataList = new Dictionary<string, BuildingData>();
        
        //��һ���õ�Ԥ����
        [SerializeField] private BuildingBluePrintFloatPicture floatPicturePrefab;
        [HideInInspector] private BuildingBluePrintFloatPicture floatPicture;
        [SerializeField] private BuildingBluePrintPage bluePrintPagePrefab;
        [HideInInspector] public BuildingBluePrintPage bluePrintPage;

        //��ʼ������
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

        #region ����

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


        //�򿪹ر���ͼ����
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

        //����������ͼ
        public void CreateFloatPicture(string buildingName,Sprite sprite)
        {
            if (floatPicture == null)
            {
                floatPicture = Instantiate(floatPicturePrefab, FindObjectOfType<Canvas>().transform);
            }
            floatPicture.Init(buildingName, sprite);
        }

        //������ͼ
        public void CreateBuildingBluePrint(string buildingName)
        {
            Building obj = Instantiate(BuildingDataList[buildingName].building);
            SetTranslucentMaterial(obj.transform);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                Collider collider = obj.transform.GetChild(i).GetComponent<Collider>();

                if (collider != null)
                    collider.isTrigger = true;
            }
            obj.gameObject.AddComponent<BuildingBluePrint>().buildingName=buildingName;
            obj.GetComponent<BuildingPack>().requirement = new Dictionary<string, ItemData>(BuildingDataList[buildingName].requirement);

        }

        //��������
        public void CreateBuiling(string buildingName,Vector3 position,Quaternion rotation)
        {
            Instantiate(BuildingDataList[buildingName].building,position,rotation);

        }


        #region ���ܺ���
        //���ܺ���

        //��������ʸ�Ϊ��͸������
        public void SetTranslucentMaterial(Transform target)
        {

            Material[] newTmMatArray;//͸����������

            for (int i = 0; i < target.childCount; i++)
            {
                if (target.GetChild(i).tag != "NoTranslucent")
                {
                    MeshRenderer meshRenderer = target.GetChild(i).GetComponent<MeshRenderer>();

                    newTmMatArray = new Material[meshRenderer.materials.Length];//�����ʼ������
                                                                                //��ȡȫ������
                    for (int j = 0; j < newTmMatArray.Length; j++)
                    {
                        newTmMatArray[j] = tmMat;
                    }

                    meshRenderer.materials = newTmMatArray;//��͸�����鸳��ģ�Ͳ���
                }

            }
        }

        //��������ʸ�Ϊ��͸����ɫ����
        public void SetTranslucentBlueMaterial(Transform target)
        {

            print(target.name + " " + target.childCount.ToString());
            Material[] newTmMatArray;//͸����������

            for (int i = 0; i < target.childCount; i++)
            {
                MeshRenderer meshRenderer = target.GetChild(i).GetComponent<MeshRenderer>();

                newTmMatArray = new Material[meshRenderer.materials.Length];//�����ʼ������
                                                                            //��ȡȫ������
                for (int j = 0; j < newTmMatArray.Length; j++)
                {
                    newTmMatArray[j] = tmMat;
                }

                meshRenderer.materials = newTmMatArray;//��͸�����鸳��ģ�Ͳ���
            }
        }

        #endregion
    }

    public class BuildingData
    {
        public Sprite icon;
        public Building building;
        public Dictionary<string, ItemData> requirement = new Dictionary<string, ItemData>();
    }
}

