using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.IO;

public class CharacterMindSystem : ComponentSystem
{

    private Dictionary<string, HashSet<string>> oppositeList = new Dictionary<string, HashSet<string>>();


    protected override void OnCreate()
    {
        InitializeOppositeList();
        //DisplayOppositeList();
    }
    protected override void OnUpdate()
    {
        CharacterMindJob();
        ReceivedWordsJob();
    }



    //简单函数
    /// <summary>
    /// 添加条目,添加条目时会对已存在的相斥条目造成影响,当条目值为0时条目会被删除
    /// </summary>
    private void CharacterMindGain(int characterID, string mind, float value)
    {
        Entities.ForEach((CharacterMindProperty characterMindProperty, CharacterProperty characterProperty) =>
        {
            if (characterProperty.ID == characterID)
            {
                if (characterMindProperty.Mind.ContainsKey(mind))
                {
                    characterMindProperty.Mind[mind] += value;
                }
                else
                {
                    characterMindProperty.Mind.Add(mind, value);
                    characterMindProperty.Mind[mind] += value;
                }
                HashSet<string> list = new HashSet<string>(characterMindProperty.Mind.Keys);
                Debug.Log(list);
                list.IntersectWith(oppositeList[mind]);
                float lossValue = 0;
                foreach (var item in list)
                {
                    characterMindProperty.Mind[item] -= value;
                    lossValue -= characterMindProperty.Mind[item];
                }
                //todo 这里的算法比较简单,未来可以改得更贴近现实一些
                //加上传入的值减去相斥条目的值为最终值
                characterMindProperty.Mind[mind] += lossValue;

                HashSet<string> deleteList = new HashSet<string>();

                foreach (var item in characterMindProperty.Mind)
                {
                    if (item.Value <= 0)
                        deleteList.Add(item.Key);
                }

                foreach (var item in deleteList)
                {
                    characterMindProperty.Mind.Remove(item);
                }
            }
        });
    }

    /// <summary>
    /// 在互斥列表中添加条目
    /// </summary>
    private void OppositeListAdd(string originTag,HashSet<string> tags)
    {
        HashSet<string> totalList = new HashSet<string>(tags);
        totalList.Add(originTag);
        foreach(var item in totalList)
        {
            HashSet<string> currentList = new HashSet<string>(totalList);
            currentList.Remove(item);

            if (oppositeList.ContainsKey(item))
            {
                foreach (var tag in currentList)
                    oppositeList[item].Add(tag);
            }
            else
            {
                oppositeList.Add(item, new HashSet<string>());
                foreach (var tag in currentList)
                    oppositeList[item].Add(tag);

            }
        }
    }

    /// <summary>
    /// 初始化相消列表
    /// </summary>
    private void InitializeOppositeList()
    {
        using (StreamReader sr = new StreamReader("Assets//WheatDewCommunity//Data//OppositeList.txt"))
        {

            string line;
            string tag = "";
            HashSet<string> tags = new HashSet<string>();

            while ((line = sr.ReadLine()) != null)
            {
                if (line[0] == '@')
                {
                    tag = line.Substring(1);
                    tags.Add(line.Substring(1));
                }
                else if (tag != "" && line != "")
                {
                    OppositeListAdd(line, tags);

                    tag = "";
                    tags.Clear();
                }
            }
        }
        Debug.Log("相消列表写入成功");
    }

    //工作函数
    /// <summary>
    /// 属性随时间衰减
    /// </summary>
    private void CharacterMindJob()
    {
        Entities.ForEach((CharacterMindProperty characterMindProperty, TimerProperty timerProperty) =>
        {
            if (characterMindProperty.Mind == null)
            {
                Debug.Log("字典为空");
                return;
            }
            foreach (var item in characterMindProperty.Mind)
            {
                //ToDo 需要加上乘数

                characterMindProperty.Mind[item.Key] -= timerProperty.currentDeltaTime * 0;
            }
        });
    }

    /// <summary>
    /// 接收语句的简单处理
    /// </summary>
    private void ReceivedWordsJob()
    {
        Entities.ForEach((CharacterMindProperty characterMindProperty, TimerProperty timerProperty,CharacterProperty characterProperty,DialogueProperty dialogueProperty) =>
        {
            if (characterMindProperty.ReceivedWords == null)
            {
                Debug.Log("字典为空");
                return;
            }
            foreach (var item in characterMindProperty.ReceivedWords)
            {
                if (item == "询问")
                {
                    CharacterMindGain(characterProperty.ID, "回答", 100f);
                    dialogueProperty.dialogueChance = true;
                }
            }
        });
    }


    //测试
    /// <summary>
    /// 显示相消列表
    /// </summary>
    private void DisplayOppositeList()
    {
        foreach(var item in oppositeList)
        {
            string log = item.Key+":";
            foreach(var opposite in item.Value)
                log+=opposite+" ";
            Debug.Log(log);
        }
    }

    public string GetMindLog()
    {
        string log = "";

        Entities.ForEach((CharacterMindProperty characterMindProperty, CharacterProperty characterProperty) =>
        {
            log += characterProperty.Name + ":\n";
            if (characterMindProperty.Mind.Count == 0)
                log += "(空)";
            else
                foreach (var item in characterMindProperty.Mind)
                {
                    log += item.Key + " " + item.Value + ",";
                }
            log += "\n";
            if (characterMindProperty.ReceivedWords.Count == 0)
                log += "(空)";
            else
                foreach (var item in characterMindProperty.ReceivedWords)
                {
                    log += item + ",";
                }
            log += "\n";
        });

        return log;
    }
    
}
