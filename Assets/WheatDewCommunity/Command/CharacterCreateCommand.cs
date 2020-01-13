using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//角色创建命令,用于创建角色
public class CharacterCreateCommand : MonoBehaviour
{
    public bool IsMainCharacter;//是否为主要角色
    public string Name;//角色名
    public int target;//todo 对话的目标，目前还没有指定对话目标的系统,暂时在角色创建时指定对话目标,到时候要删掉
    public Dictionary<string,float> Tendency;//角色偏好
}
