using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TimerSystem : ComponentSystem
{
    private float referTime;
    protected override void OnUpdate()
    {
        float deltaTimer = Time.deltaTime;
        referTime += deltaTimer;
        TimerJob(deltaTimer);
        DialogueTimerJob(deltaTimer);
    }

    //简单函数
    private void TimerJob(float deltaTime)
    {
        Entities.ForEach((TimerProperty timerProperty) =>
        {
            timerProperty.currentTime = referTime;
            timerProperty.currentTime = deltaTime;
        });
    }

    private void DialogueTimerJob(float deltaTime)
    {
        Entities.ForEach((DialogueCommand c_dialogue) =>
        {
            c_dialogue.life -= deltaTime;
        });
    }
}
