using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Origin
{
    public enum RangeType { Player,Item}
    public class RangeComponent : MonoBehaviour
    {
        [HideInInspector] public Transform target;
        [HideInInspector] public string key;

        [HideInInspector] public bool inbound = false;

        public HashSet<string> currentRange = new HashSet<string>();

        public RangeType rangeType;


        private void Start()
        {
            if (target == null)
            {
                key = transform.GetInstanceID().ToString();
                RangeSystem.s.components.Add(key, this);
            }
            else
            {
                key = target.GetInstanceID().ToString();
                RangeSystem.s.components.Add(key, this);
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            switch (rangeType)
            {
                case RangeType.Player:
                    break;
                case RangeType.Item:
                    ItemActionEnter(other);
                    break;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            switch (rangeType)
            {
                case RangeType.Player:
                    break;
                case RangeType.Item:
                    ItemActionExit(other);
                    break;
            }

            inbound = false;
        }

        public void ItemActionEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                inbound = true;
                RangeSystem.s.components[other.transform.GetInstanceID().ToString()].currentRange.Add(key);
                InfoInSceneSystem.s.DisplayItemInfoInScene(EntitySystem.s.components[key].displayName);
            }
        }

        public void ItemActionExit(Collider other)
        {
            if (other.tag == "Player")
            {
                inbound = true;
                RangeSystem.s.components[other.transform.GetInstanceID().ToString()].currentRange.Remove(key);
                InfoInSceneSystem.s.HiddenItemInfoInScene();
            }
        }
    }
}

