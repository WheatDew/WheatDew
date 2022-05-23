using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CCharacter : MonoBehaviour
    {
        //基础属性
        public string characterName;
        public string key;

        private void Awake()
        {
            SCharacter.CreateCharacter("lianxi", this);
            key = name + Random.Range(int.MinValue, int.MaxValue).ToString();
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //SCommand.Excute("CreateBuilding","empty"+(transform.position + transform.forward * 8).ToString());
            }
            
        }
    }
}

