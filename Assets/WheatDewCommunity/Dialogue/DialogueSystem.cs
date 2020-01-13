﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[UpdateAfter(typeof(CharacterMindSystem))]
public class DialogueSystem : ComponentSystem
{
    public CorpusCommand corpusCommandPrefab;

    protected override void OnCreate()
    {
        
    }

    protected override void OnStartRunning()
    {
        InitializeDialogueCommandPrefab();
    }
    protected override void OnUpdate()
    {
        DialogueJob();
    }

    //简单函数
    /// <summary>
    /// 初始化对话命令的预制体
    /// </summary>
    private void InitializeDialogueCommandPrefab()
    {
        if (this.corpusCommandPrefab == null)
        {
            Entities.ForEach((CorpusCommandPrefab corpusCommandPrefab) =>
            {
                this.corpusCommandPrefab = corpusCommandPrefab.corpusCommand;
                //this.corpusCommandPrefab.gameObject.AddComponent<GameObjectEntity>();
                corpusCommandPrefab.gameObject.SetActive(false);
                Debug.Log("对话命令预制体初始化成功");
            });
        }
    }

    /// <summary>
    /// 根据对话属性的可能性生成语料库命令
    /// </summary>
    private void DialogueJob()
    {
        Entities.ForEach((DialogueProperty dialogueProperty,CharacterProperty characterProperty,CharacterPrepareActsProperty p_CharacterPrepareActs,CharacterMoodProperty p_CharacterMood) =>
        {
            if (dialogueProperty.dialogueChance&&dialogueProperty.target!=-1)
            {
                CorpusCommand corpusCommand = Object.Instantiate(corpusCommandPrefab);
                corpusCommand.origin = characterProperty.ID;
                corpusCommand.target = dialogueProperty.target;
                string s="";
                s += "行为:";
                //遍历预行为列表
                foreach (var tag in p_CharacterPrepareActs.PrepareDialogueActs)
                {
                    s += tag+" ";
                    corpusCommand.tags.Add(tag);
                }
                s += "情绪:";
                //遍历情绪列表
                foreach(var tag in p_CharacterMood.Expression)
                {
                    s += tag + " ";
                    corpusCommand.tags.Add(tag);
                }
                corpusCommand.gameObject.SetActive(true);
                dialogueProperty.dialogueChance = false;
                Debug.Log("生成语料库命令( "+s+")，origin=" + corpusCommand.origin.ToString() + "target=" + corpusCommand.target);
                //清除准备对话行动
                p_CharacterPrepareActs.PrepareDialogueActs.Clear();
            }
        });
    }
}
