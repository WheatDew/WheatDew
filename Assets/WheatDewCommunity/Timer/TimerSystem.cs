using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class TimerSystem : ComponentSystem
{
    private float referTime= 86390f;
    private int referDays = 739265;
    public string date;
    public int y, m, d, h, min, sec;

    protected override void OnUpdate()
    {
        float deltaTimer = Time.deltaTime;
        referTime += deltaTimer;
        TimerJob(deltaTimer);
        DateJob();
    }

    //简单函数
    private void TimerJob(float deltaTime)
    {
        Entities.ForEach((TimerProperty timerProperty) =>
        {
            timerProperty.currentTime = referTime;
            timerProperty.currentDeltaTime = deltaTime;
        });
    }

    private void DateJob()
    {
        if (referTime >= 86400f)
        {
            referDays += (int)(referTime / 86400f);
            referTime %= 86400f;
        }

        TimeToSecondUnitDate(referTime,out h,out min,out sec);
        TimeToDayUnitDate(referDays,out y,out m,out d);
        date = string.Format("{0:0000}年 {1:00}月 {2:00}日 {3:00}时 {4:00}分 {5:00}秒",y,m,d,h,min,sec);
    }

    //将参考时间增加
    public void TimeGain(float value)
    {
        referTime += value;
    }

    private void TimeToSecondUnitDate(in float referTime,out int h,out int min, out int sec)
    {
        float time = referTime;
        h = (int)(time / 3600f);
        time %= 3600f;
        min = (int)(time / 60f);
        sec = (int)(time % 60);
    }

    private void TimeToDayUnitDateWhereCommonYear(in int referDays, out int m, out int d)
    {
        int days = referDays;
        m = 0; d = 0;
        if (days <= 30)
        {
            m = 1; d = days + 1;
        }
        else if (days <= 58)
        {
            m = 2; d = days - 30;
        }
        else if (days <= 89)
        {
            m = 3; d = days - 58;
        }
        else if (days <= 119)
        {
            m = 4; d = days - 89;
        }
        else if (days <= 150)
        {
            m = 5; d = days - 119;
        }
        else if (days <= 180)
        {
            m = 6; d = days - 150;
        }
        else if (days <= 211)
        {
            m = 7; d = days - 180;
        }
        else if (days <= 242)
        {
            m = 8; d = days - 211;
        }
        else if (days <= 272)
        {
            m = 9; d = days - 242;
        }
        else if (days <= 303)
        {
            m = 10; d = days - 272;
        }
        else if (days <= 333)
        {
            m = 11; d = days - 303;
        }
        else if (days <= 364)
        {
            m = 12; d = days - 333;
        }
    }

    private void TimeToDayUnitDateWhereLeapYear(in int referDays, out int m, out int d)
    {
        int days = referDays;

        m = 0; d = 0;
        if (days <= 30)
        {
            m = 1; d = days+1;
        }
        else if (days <= 59)
        {
            m = 2; d = days - 30;
        }
        else if (days <= 90)
        {
            m = 3; d = days - 60;
        }
        else if (days <= 120)
        {
            m = 4; d = days - 91;
        }
        else if (days <= 151)
        {
            m = 5; d = days - 121;
        }
        else if (days <= 181)
        {
            m = 6; d = days - 152;
        }
        else if (days <= 212)
        {
            m = 7; d = days - 182;
        }
        else if (days <= 243)
        {
            m = 8; d = days - 213;
        }
        else if (days <= 273)
        {
            m = 9; d = days - 244;
        }
        else if (days <= 304)
        {
            m = 10; d = days - 274;
        }
        else if (days <= 334)
        {
            m = 11; d = days - 305;
        }
        else if (days <= 365)
        {
            m = 12; d = days - 335;
        }
    }

    private void TimeToDayUnitDate(int days,out int y,out int m,out int d)
    {
        y = 0;m = 0;d = 0;
        y = days / 1461*4;
        days %= 1461;

        if (days < 366)
            TimeToDayUnitDateWhereLeapYear(days,out m,out d);
        else
        {
            y += (days-366) / 365+1;
            days = (days - 366) % 365;
            TimeToDayUnitDateWhereCommonYear(days, out m, out d);
        }
    }
}
