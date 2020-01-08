using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CharacterMindProperty类用于记录角色的想法,相关系统获取想法处理成角色的行为指令
//DialogueImmediateMind表示在接收到对话信息时的瞬时反应,是瞬时的想法,处理后会被立刻销毁
//DialoguePersistMind当前所思考的有关对话的信息,是延续性的想法,处理后不会被立刻销毁,可用于持续下达指令或者下达一个持续性的指令

public class CharacterMindProperty : MonoBehaviour
{
    //public Dictionary<string, float> DialogueImmediateMind=new Dictionary<string, float>();
    //public Dictionary<string, float> DialoguePersistMind = new Dictionary<string, float>();
    //public HashSet<string> ReceivedWords = new HashSet<string>();
}
