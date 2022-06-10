using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Origin
{
    public class TaskComponent : MonoBehaviour
    {
        private string key;


        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            TaskSystem.s.components.Add(transform.GetInstanceID().ToString(), this);

            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                TaskSystem.s.Execute(string.Format("PickClosestItem {0}", key));
            }

        }
    }
}

