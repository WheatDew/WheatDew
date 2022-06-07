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
            key = GetComponent<EntityComponent>().key;
            PickSystem.s.pickItems.Add(key,this);
        }

        public void Destroy()
        {
            PickSystem.s.pickItems.Remove(key);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            PickSystem.s.pickItems.Remove(key);
        }

    }
}

