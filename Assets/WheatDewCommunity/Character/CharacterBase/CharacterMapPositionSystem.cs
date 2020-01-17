using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

//尝试在不用命令对象的情况下传递数据
public class CharacterMapPositionSystem : ComponentSystem
{

    private TimerSystem s_Timer;

    protected override void OnStartRunning()
    {
        s_Timer = World.Active.GetExistingSystem<TimerSystem>();
    }

    protected override void OnUpdate()
    {
        
    }

    public void SetCharacterMapPosition(int ID,string mapPosition)
    {
        Entities.ForEach((CharacterMapPositionProperty p_mapPosition,CharacterProperty p_base) =>
        {
            if (p_base.ID == ID)
            {
                p_mapPosition.mapPosition = mapPosition;
                s_Timer.TimeGain(18000f);
            }
        });
    }

    public void SetCharacterMapPosition(string mapPosition)
    {
        Entities.ForEach((CharacterMapPositionProperty p_mapPosition, CharacterProperty p_base) =>
        {
            if (p_base.IsMainCharacter)
            {
                p_mapPosition.mapPosition = mapPosition;
                s_Timer.TimeGain(18000f);
            }

        });
    }

}
