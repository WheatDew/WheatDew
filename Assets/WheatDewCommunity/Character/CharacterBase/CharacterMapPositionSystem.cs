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

    //设定固定位置
    public void SetPositionByPosition(string characterName)
    {
        Entities.ForEach((CharacterProperty p_character,CharacterMapPositionProperty p_MapPosition) =>
        {
            if(p_character.Name==characterName)
            {
                if(s_Timer.d==1)
                {
                    p_MapPosition.mapPosition = "park";
                }
                else if (s_Timer.d == 2)
                {
                    p_MapPosition.mapPosition = "restaurant";
                }
                else if (s_Timer.d == 3)
                {
                    p_MapPosition.mapPosition = "library";
                }
                else if (s_Timer.d == 4)
                {
                    p_MapPosition.mapPosition = "market";
                }
                else if (s_Timer.d == 5)
                {
                    p_MapPosition.mapPosition = "JimingTemple";
                }
                else if (s_Timer.d == 6)
                {
                    p_MapPosition.mapPosition = "store";
                }
                else if (s_Timer.d == 7)
                {
                    p_MapPosition.mapPosition = "home";
                }
            }
        });
    }

}
