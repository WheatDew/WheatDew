using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickComponent : MonoBehaviour
    {
        public string key;

        //��ʼ����ʱ��������ӵ��б�
        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            PickSystem.s.components.Add(key, this);
        }


        private void Update()
        {
            Job();
        }
        virtual public InfoData Job()
        {
            

            return null;
        }
    }
}

