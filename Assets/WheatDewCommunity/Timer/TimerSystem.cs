using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TimerSystem : ComponentSystem
{
    private float referTime;
    protected override void OnUpdate()
    {
        referTime += Time.deltaTime;
        TimerJob();
    }

    //简单函数
    private void TimerJob()
    {
        Entities.ForEach((TimerProperty timerProperty) =>
        {
            timerProperty.currentTime = referTime;
            timerProperty.currentTime = Time.deltaTime;
        });
    }
}
