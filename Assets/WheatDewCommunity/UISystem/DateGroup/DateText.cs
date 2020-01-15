using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class DateText : MonoBehaviour
{

    private TimerSystem s_Timer;
    private Text dateText;

    void Start()
    {
        s_Timer = World.Active.GetExistingSystem<TimerSystem>();
        dateText = GetComponent<Text>();
    }

    void Update()
    {
        dateText.text = s_Timer.date;
    }


}
