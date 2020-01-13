using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.IO;

//计算角色的情绪属性
//情绪属性存在衰减
//不同情绪条目之间存在相消关系
[UpdateBefore(typeof(CharacterMindSystem))]
public class CharacterMoodSystem : ComponentSystem
{
    //相消关系列表,从文件初始化
    private Dictionary<string, HashSet<string>> oppositeList = new Dictionary<string, HashSet<string>>();

    protected override void OnUpdate()
    {
        TendencyJob();
        CharacterMoodJob();
    }

    /// <summary>
    /// 情绪属性随时间衰减
    /// </summary>
    private void CharacterMoodJob()
    {

        Entities.ForEach((CharacterMoodProperty characterMoodProperty, TimerProperty timerProperty) =>
        {
            if (characterMoodProperty.Mood.Count==0)
            {
                //如果情绪为空,则初始化情绪
                characterMoodProperty.Mood.Add("平静", 50f);
                return;
            }
            Dictionary<string, float> moodClone = new Dictionary<string, float>(characterMoodProperty.Mood);
            foreach (var item in characterMoodProperty.Mood)
            {
                //ToDo 目前情绪不会衰减

                moodClone[item.Key] -= timerProperty.currentDeltaTime * 0;
            }
            foreach (var item in moodClone)
            {
                characterMoodProperty.Mood[item.Key] = moodClone[item.Key];
            }
            //todo 写法有一些问题,而且这个函数遍历写的有点多,感觉遍历对象数量多的时候可能会很卡

            //如果对应条目大于平静的值,则将该条目添加到表达中
            HashSet<string> temp = new HashSet<string>();
            foreach(var item in characterMoodProperty.Mood)
            {
                if(item.Value>characterMoodProperty.Mood["平静"])
                {
                    temp.Add(item.Key);
                }
            }
            characterMoodProperty.Expression = new HashSet<string>(temp);

            //如果没有大于平静的条目,则将平静添加到表达中
            if (characterMoodProperty.Expression.Count == 0)
            {
                characterMoodProperty.Expression.Add("平静");
            }
        });
    }

    private void TendencyJob()
    {
        Entities.ForEach((CharacterMoodProperty p_Mood,CharacterReceivedWordsProperty p_ReceivedWords,CharacterProperty p_character) =>
        {
            if (p_ReceivedWords.Act.Contains("v回应")&&p_ReceivedWords.Act.Contains("v消极表达"))
                foreach (var item in p_ReceivedWords.Act)
                {
                    if(p_Mood.Tendency.ContainsKey(item))
                    {
                        CharacterMoodGain(p_character.ID, "高兴", -p_Mood.Tendency[item]);
                        CharacterMoodGain(p_character.ID, "生气", p_Mood.Tendency[item]);
                        Debug.Log("down");
                    }
                }
            else if(p_ReceivedWords.Act.Contains("v回应")&&p_ReceivedWords.Act.Contains("v积极表达"))
                foreach(var item in p_ReceivedWords.Act)
                {
                    if (p_Mood.Tendency.ContainsKey(item))
                    {
                        CharacterMoodGain(p_character.ID, "高兴", p_Mood.Tendency[item]);
                        CharacterMoodGain(p_character.ID, "生气", -p_Mood.Tendency[item]);
                        Debug.Log("up");
                    }
                }
        });
    }

    /// <summary>
    /// 添加情绪条目,不存在相消的情况
    /// </summary>
    private void CharacterMoodGain(int characterID,string mood,float value)
    {
        Entities.ForEach((CharacterProperty p_Character, CharacterMoodProperty p_CharacterMood) =>
        {
            if (p_Character.ID == characterID)
            {
                if (p_CharacterMood.Mood.ContainsKey(mood))
                    p_CharacterMood.Mood[mood] += value;
                else
                    p_CharacterMood.Mood.Add(mood, value);
            }
        });
    }


    /// <summary>
    /// 添加情绪条目,存在相消的情况
    /// </summary>
    private void CharacterMoodGainWithOpposite(int characterID, string mood, float value)
    {
        //Debug.Log("添加情绪条目" + mood + value.ToString());
        //Entities.ForEach((CharacterReceivedWordsProperty p_ReceivedWords, CharacterProperty characterProperty) =>
        //{

        //    if (characterProperty.ID == characterID)
        //    {
        //        Debug.Log("循环查找角色,匹配id" + characterProperty.ID.ToString() + "和" + characterID);

        //        if (characterMoodProperty.DialogueImmediateMind.ContainsKey(mood))
        //        {
        //            characterMoodProperty.DialogueImmediateMind[mood] += value;
        //        }
        //        else
        //        {
        //            characterMoodProperty.DialogueImmediateMind.Add(mood, value);
        //        }

        //        if (oppositeList.ContainsKey(mood))
        //        {
        //            Debug.Log("计算条目之间的排斥关系" + mood);

        //            HashSet<string> list = new HashSet<string>(characterMoodProperty.DialogueImmediateMind.Keys);
        //            Debug.Log(list);

        //            list.IntersectWith(oppositeList[mood]);
        //            float lossValue = 0;
        //            foreach (var item in list)
        //            {
        //                characterMoodProperty.DialogueImmediateMind[item] -= value;
        //                lossValue -= characterMoodProperty.DialogueImmediateMind[item];
        //            }
        //            //todo 这里的算法比较简单,未来可以改得更贴近现实一些
        //            //加上传入的值减去相斥条目的值为最终值
        //            characterMoodProperty.DialogueImmediateMind[mood] += lossValue;

        //            HashSet<string> deleteList = new HashSet<string>();

        //            foreach (var item in characterMoodProperty.DialogueImmediateMind)
        //            {
        //                if (item.Value <= 0)
        //                    deleteList.Add(item.Key);
        //            }

        //            foreach (var item in deleteList)
        //            {
        //                characterMoodProperty.DialogueImmediateMind.Remove(item);
        //            }
        //        }

        //    }
        //});
    }

    /// <summary>
    /// 初始化相消列表
    /// </summary>
    private void InitializeOppositeList()
    {
        using (StreamReader sr = new StreamReader("Assets//WheatDewCommunity//Data//MoodOppositeList.txt"))
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

    /// <summary>
    /// 在互斥列表中添加条目
    /// </summary>
    private void OppositeListAdd(string originTag, HashSet<string> tags)
    {
        HashSet<string> totalList = new HashSet<string>(tags);
        totalList.Add(originTag);
        foreach (var item in totalList)
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

    //测试
    /// <summary>
    /// 显示相消列表
    /// </summary>
    private void DisplayOppositeList()
    {
        foreach (var item in oppositeList)
        {
            string log = item.Key + ":";
            foreach (var opposite in item.Value)
                log += opposite + " ";
            Debug.Log(log);
        }
    }
}
