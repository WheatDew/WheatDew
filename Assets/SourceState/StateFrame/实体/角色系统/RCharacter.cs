using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCharacter : MonoBehaviour
{
    public Dictionary<string, DCharacter> datas = new Dictionary<string, DCharacter>();

    public Dictionary<string, string> commandRule = new Dictionary<string, string>();

    private async void Start()
    {
        //常规初始化
        await new WaitForUpdate();
        HashSet<string> keysSet = SEntity.GetKeyByTag("character");

        foreach(var key in keysSet)
        {
            datas.Add(key, new DCharacter());
        }

    }

    //private void Update()
    //{
    //    foreach (var item in datas)
    //    {
            
    //        item.Value.health -= 0.1f;
    //    }
    //}

    ////
    //public void SetAction()
    //{

    //}

    ////命令函数
    //public float GetHealthValue(string[] values,CommandData commandData)
    //{
    //    Debug.Log(datas[values[1]].health);
    //    return datas[values[1]].health;
    //}
    
    
    ////初始化函数
    ////初始化词条
    //public void InitWords(params string[] words)
    //{
    //    foreach(var word in words)
    //    {
    //        if (!SWord.words.Contains(word))
    //            SWord.words.Add(word);
    //    }
    //}
}

//public class DCharacter
//{
//    public float health=1000f;

//    public DCharacter()
//    {
//        health = 1000;
//    }
//}
