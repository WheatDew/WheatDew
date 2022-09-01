using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public delegate string DriveData();
    public class CDrive : MonoBehaviour
    {
        [HideInInspector] public string key;
        public Dictionary<string, DriveData> rule = new Dictionary<string, DriveData>();

        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            Init();
        }

        virtual public void Init()
        {

        }

        //¹¦ÄÜº¯Êý
        public void AddDriveData(string address,DriveData driveDataFunction)
        {
            rule.Add(address, driveDataFunction);
        }
    }
}

