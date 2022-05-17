using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SPack : MonoBehaviour
    {
        private static SPack _s;
        public static SPack S { get { return _s; } }

        public UPack packPagePrefab;
        private UPack packPage;

        //����б�
        public Dictionary<string, CPack> cpackList = new Dictionary<string, CPack>();

        private void Start()
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

        //��ӽ�ɫ������Ʒ [����]&[��Ʒ����]&[����]
        public int GainPackItem(string component,string item,uint count)
        {
            CPack target = cpackList[component];
            if (!target.packItems.ContainsKey(item))
                target.packItems.Add(item, count);
            else
                target.packItems[item] += count;

            return 0;
        }

        public string LosePackItem(string component,string item,uint count)
        {
            CPack target = cpackList[component];
            if (!target.packItems.ContainsKey(item))
                return "�����ڸ���Ʒ";
            else if (target.packItems[item] < count)
                return "��Ʒ��������";
            else
            {
                target.packItems[item] -= count;
                if (target.packItems[item] == 0)
                    target.packItems.Remove(item);
            }
            return "ִ�гɹ�";
        }
    }
}
