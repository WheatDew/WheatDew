using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class RangeSystem : MonoBehaviour
    {
        private static RangeSystem instance;
        public static RangeSystem s { get { return instance; } }

        public void Awake()
        {
            if (instance == null) instance = this;
        }

        public Dictionary<string, RangeComponent> components = new Dictionary<string, RangeComponent>();


    }
}

