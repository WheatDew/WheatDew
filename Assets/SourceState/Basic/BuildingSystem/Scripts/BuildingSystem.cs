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
        public List<string> buildingPrefabNameList = new List<string>();//����

        public Material tmMat; //͸������

        public Material baseMaterial;

        private void Start()
        {
            if (!_s) _s = this;

            Init();
        }

        //�����е�����
        public Dictionary<string, BuildingComponent> cbuildingList = new Dictionary<string, BuildingComponent>();

        //��Դ��
        public Dictionary<string, Building> buildingList = new Dictionary<string, Building>();

        //��һ���õ�Ԥ����
        [SerializeField] private BuildingBluePrintFloatPicture floatPicturePrefab;
        [HideInInspector] private BuildingBluePrintFloatPicture floatPicture;
        [SerializeField] private BuildingBluePrintPage bluePrintPagePrefab;
        [HideInInspector] public BuildingBluePrintPage bluePrintPage;

        //��ʼ������
        public void Init()
        {
            for(int i = 0; i < buildingPrefabList.Count; i++)
            {
                buildingList.Add(buildingPrefabNameList[i], buildingPrefabList[i]);
            }
        }

        //�򿪹ر���ͼ����
        public void SwitchBluePrintPage()
        {
            if (bluePrintPage == null)
            {
                bluePrintPage = Instantiate(bluePrintPagePrefab, FindObjectOfType<Canvas>().transform);
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

        //��������
        public void CreateBuiling(string buildingName,Vector3 position,Quaternion rotation)
        {
            Instantiate(buildingList[buildingName],position,rotation);

        }


        #region ���ܺ���
        //���ܺ���

        //��������ʸ�Ϊ��͸������
        public void SetTranslucentMaterial(Transform target)
        {

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
}

