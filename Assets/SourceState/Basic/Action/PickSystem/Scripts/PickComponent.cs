using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    
    public class PickComponent : MonoBehaviour
    {
        public enum Subject {Player,AI };
        public string key;
        private RangeComponent c_range;

        //��ʼ����ʱ��������ӵ��б�
        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            PickSystem.s.components.Add(key, this);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.E))
                PickClosedItemJob();
        }


        public InfoData PickClosedItemJob()
        {
            TaskSystem.s.Execute(string.Format("PickItem {0}", key));

            return null;
        }
    }
}

