using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackComponent : MonoBehaviour
    {

        public Dictionary<string, ItemData> pack = new Dictionary<string, ItemData>();


        private void Start()
        {
            Init();
        }

        public virtual void Init()
        {

        }
    }
}

