using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Origin
{
    public class BehaviourComponent : MonoBehaviour
    {
        private string key;
        float weightTimer=0;

        public HashSet<BehaviourData> behaviourDatas = new HashSet<BehaviourData>();

        //默认行为
        public BehaviourData normal=new BehaviourData();
        private void Start()
        {
            key = GetComponent<EntityComponent>().key;
            BehaviourSystem.s.components.Add(key, this);

            
        }

        private void Update()
        {
            weightTimer += Time.deltaTime;

            //权重计算
            if(weightTimer > 1)
            {
                float currentWeight = 0;
                BehaviourData currentBehaviour=normal;
                foreach(var item in behaviourDatas)
                {
                    item.weight = CommandSystem.S.Execute(item.weightCalculation).floatValue;
                    if(item.weight > currentWeight)
                    {
                        currentWeight = item.weight;
                        currentBehaviour = item;
                    }
                    print(item.type + " " + item.weight.ToString());
                }
                currentBehaviour.action();
                weightTimer = 0;
            }

        }


    }
}

