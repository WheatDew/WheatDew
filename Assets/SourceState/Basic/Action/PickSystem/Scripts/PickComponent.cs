using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    
    public class PickComponent : MonoBehaviour
    {
        public enum Subject {Player,AI };
        public string key;

        //开始运行时将自身添加到列表
        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            PickSystem.s.components.Add(key, this);
            Debug.Log("pickComponentKey:" + key);
        }


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.F))
                PickClosedItemJob();
        }


        public InfoData PickClosedItemJob()
        {
            TaskSystem.s.Execute(string.Format("{0} {1}", "PickClosestItem",key) );

            return null;
        }
    }
}

