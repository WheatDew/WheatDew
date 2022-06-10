using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickItem : MonoBehaviour
    {
        public string key;
        public string pickItemName;
        public int pickItemCount;

        private void Start()
        {
            key = transform.GetInstanceID().ToString();
            PickSystem.s.items.Add(key,this);
        }

        public void Destroy()
        {
            PickSystem.s.items.Remove(key);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            PickSystem.s.items.Remove(key);
        }

    }
}

