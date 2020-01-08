using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.IO;

//获取词语接收处理的结果,获取表面想法
public class CharacterMindSystem : ComponentSystem
{


    protected override void OnCreate()
    {
    }
    protected override void OnUpdate()
    {
        
    }


    /// <summary>
    /// 将所有处理的信息结合起来做最终处理
    /// </summary>
    private void CharacterMindJob()
    {
        //todo 目前是不做任何处理,直接将接收对话后处理得到的行动直接落实
        Entities.ForEach((CharacterReceivedWordsProperty p_ReceivedWords,CharacterPrepareActsProperty p_PrepareActs) =>
        {
            p_PrepareActs.PrepareDialogueActs=new HashSet<string>(p_ReceivedWords.Act);
        });
    }
    
}
