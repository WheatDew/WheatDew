using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Origin
{
    public class BehaviourSystem : MonoBehaviour
    {
        private static BehaviourSystem _s;
        public static BehaviourSystem s { get { return _s; } }

        private void Awake()
        {
            if (!_s) _s = this;
        }

        public Dictionary<string, BehaviourComponent> components = new Dictionary<string, BehaviourComponent>();

        
    }

    public class BehaviourData
    {
        public string type;
        public float weight;
        public string weightCalculation;
        public UnityAction action;

        public BehaviourData()
        {
            type = "default";
            weight = 0;
            weightCalculation = null;
            action = null;
        }

        public BehaviourData(string tpye,float weight,string weightCalculation,UnityAction action)
        {
            this.type = tpye;
            this.weight = weight;
            this.weightCalculation = weightCalculation;
            this.action = action;
        }
    }
}

