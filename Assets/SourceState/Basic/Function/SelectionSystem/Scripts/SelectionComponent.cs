using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SelectionComponent : MonoBehaviour
    {
        [HideInInspector] public string key;
        public Transform target;

        private void Start()
        {
            if(target == null)
                target = transform;

            key = target.GetInstanceID().ToString();
            SelectionSystem.s.components.Add(target.GetInstanceID().ToString(), this);
        }
    }
}

