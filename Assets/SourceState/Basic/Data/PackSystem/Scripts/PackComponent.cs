using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackComponent : MonoBehaviour
    {
        public string key;
        public Dictionary<string, Item> pack = new Dictionary<string, Item>();
        public string recipes;

        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {

            key = transform.GetInstanceID().ToString();
            PackSystem.S.components.Add(key, this);
            print(key);
        }

    }
}

