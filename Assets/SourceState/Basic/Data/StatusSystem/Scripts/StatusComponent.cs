using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class StatusComponent : MonoBehaviour
    {
        public StatusData statusData=new StatusData();

        private void Start()
        {
            StatusSystem.S.statusList.Add(transform.GetInstanceID().ToString(),this);

            statusData.name = name;
            statusData.food = 50;
        }

        public void Update()
        {

            FoodUpdata();
        }

        public void FoodUpdata()
        {
            statusData.food -= 1 * Time.deltaTime;
            if(statusData.food < 0)
                statusData.food = 0;
        }

        public void FoodGain(float value)
        {
            statusData.food += value;
        }
    }
}

