using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class StatusComponent : MonoBehaviour
    {
        public string name;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (StatusSystem.S.statusPage == null)
                {
                    
                }
        }
        }
    }
}

