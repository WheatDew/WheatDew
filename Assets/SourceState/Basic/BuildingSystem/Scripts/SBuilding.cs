using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SBuilding : MonoBehaviour
    {
        private static SBuilding _s;
        public static SBuilding S { get { return _s; } }

        //���õ�Ԥ�����б�
        public List<CBuildingItem> buildingPrefabList = new List<CBuildingItem>();

        public Material tmMat; //͸������

        public Material baseMaterial;

        private void Start()
        {
            if (!_s) _s = this;
        }

        //�����е�����
        public Dictionary<string, CBuilding> cbuildingList = new Dictionary<string, CBuilding>();
        //public static Dictionary<string, CBuildingItem> buildingItemList = new Dictionary<string, CBuildingItem>();

        private CBuildingItem currentBuildingItem;


        //����ǰ׼��
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

        //����
        public int SetBuilding()
        {
            currentBuildingItem = null;
            return 0;
        }


        public string SetBuildingBasicState(string value)
        {
            
            return "";
        }

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

    }
}

