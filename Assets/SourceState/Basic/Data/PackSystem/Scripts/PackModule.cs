using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackModule : MonoBehaviour
    {
        public PackSystem packSystem;

        private void Start()
        {
            TaskSystem.s.Declare("GainItem", GainItem);
        }

        public void GainItem(string[] value,TaskData task)
        {
            packSystem.PackItemGain(value[0], value[1], int.Parse(value[2])); 
        }
    }
}

