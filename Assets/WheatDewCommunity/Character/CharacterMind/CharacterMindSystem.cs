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
        ConvertReceivedWordsJob();
    }



    //简单函数
    /// <summary>
    /// 添加条目,添加条目时会对已存在的相斥条目造成影响,当条目值为0时条目会被删除
    /// </summary>
    private void CharacterMindGain(int characterID, string mind, float value)
    {
        Debug.Log("添加想法条目" + mind + value.ToString());
        Entities.ForEach((CharacterMindProperty characterMindProperty, CharacterProperty characterProperty) =>
        {
            
            if (characterProperty.ID == characterID)
            {
                Debug.Log("循环查找角色,匹配id" + characterProperty.ID.ToString() + "和" + characterID);

                if (characterMindProperty.DialogueImmediateMind.ContainsKey(mind))
                {
                    characterMindProperty.DialogueImmediateMind[mind] += value;
                }
                else
                {
                    characterMindProperty.DialogueImmediateMind.Add(mind, value);
                }

                if (oppositeList.ContainsKey(mind))
                {
                    Debug.Log("计算条目之间的排斥关系" + mind);

                    HashSet<string> list = new HashSet<string>(characterMindProperty.DialogueImmediateMind.Keys);
                    Debug.Log(list);

                    list.IntersectWith(oppositeList[mind]);
                    float lossValue = 0;
                    foreach (var item in list)
                    {
                        characterMindProperty.DialogueImmediateMind[item] -= value;
                        lossValue -= characterMindProperty.DialogueImmediateMind[item];
                    }
                    //todo 这里的算法比较简单,未来可以改得更贴近现实一些
                    //加上传入的值减去相斥条目的值为最终值
                    characterMindProperty.DialogueImmediateMind[mind] += lossValue;

                    HashSet<string> deleteList = new HashSet<string>();

                    foreach (var item in characterMindProperty.DialogueImmediateMind)
                    {
                        if (item.Value <= 0)
                            deleteList.Add(item.Key);
                    }

                    foreach (var item in deleteList)
                    {
                        characterMindProperty.DialogueImmediateMind.Remove(item);
                    }
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
            if (characterMindProperty.DialogueImmediateMind == null)
            {
                Debug.Log("字典为空");
                return;
            }
            Dictionary<string, float> mindClone = new Dictionary<string, float>(characterMindProperty.DialogueImmediateMind);
            foreach (var item in characterMindProperty.DialogueImmediateMind)
            {
                //ToDo 需要加上乘数

                //mindClone[item.Key] -= timerProperty.currentDeltaTime * 0;
            }
            foreach(var item in mindClone)
            {
                characterMindProperty.DialogueImmediateMind[item.Key] = mindClone[item.Key];
            }
            //todo 写法有一些问题
        });
    }

    /// <summary>
    /// 接收语句的简单处理
    /// </summary>
    private void ConvertReceivedWordsJob()
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
                switch (item)
                {
                    case "询问":

                        ConvertReceivedWords(characterMindProperty, characterProperty, dialogueProperty,"询问","回答");
                        return;
                }
            }
        });
    }

    /// <summary>
    /// 处理接收到的信息
    /// </summary>
    private void ConvertReceivedWords(CharacterMindProperty characterMindProperty,CharacterProperty characterProperty,DialogueProperty dialogueProperty,string originWord,string convertedWord)
    {
        HashSet<string> vocabularies = new HashSet<string>(characterMindProperty.ReceivedWords);
        //foreach(var item in characterMindProperty.ReceivedWords)  vocabularies.Add(item); 
        vocabularies.Add(convertedWord);
        Debug.Log("在单词卡数组中添加" + convertedWord);
        vocabularies.Remove(originWord);
        Debug.Log("在单词卡数组中移除" + originWord);
        foreach (var word in vocabularies) { CharacterMindGain(characterProperty.ID, word, 100f); };
        characterMindProperty.ReceivedWords.Clear();

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

    /// <summary>
    /// 在记录系统中显示CharacterMindProperty
    /// </summary>
    /// <returns></returns>
    public string GetMindLog()
    {
        string log = "";

        Entities.ForEach((CharacterMindProperty characterMindProperty, CharacterProperty characterProperty) =>
        {
            log += characterProperty.ID+" "+ characterProperty.Name + ":\n";
            if (characterMindProperty.DialogueImmediateMind.Count == 0)
                log += "(空)";
            else
                foreach (var item in characterMindProperty.DialogueImmediateMind)
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
