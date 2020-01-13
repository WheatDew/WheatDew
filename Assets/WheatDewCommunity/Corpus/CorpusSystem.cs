using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System.IO;


//语料库系统，获取命令生成最终对话文字
public class CorpusSystem : ComponentSystem
{

    //语料库元素结构体,两个成员分别为语句内容和句子标签
    struct Item
    {
        public string Sentence;
        public HashSet<string> Tags;
    }

    //保存对话命令的预制体
    private DialogueCommand dialogueCommandPrefab;

    //元素为语料库元素,索引为语句内容的语料库
    private Dictionary<string,Item> CorpusOrigin = new Dictionary<string,Item>();
    //元素为标签集合,索引为语句内容
    private Dictionary<string, HashSet<string>> CorpusWithTag = new Dictionary<string, HashSet<string>>();

    protected override void OnCreate()
    {
        //在创建管理器时从文件初始化语料库,设定对话命令
        ReadText();
    }

    protected override void OnStartRunning()
    {
        InitializeDialogueCommandPrefab();
    }



    protected override void OnUpdate()
    {
        //每一帧捕获语料库命令生成对话命令
        CorpusJob();
    }

    /// <summary>
    /// 给现有的条目添加标签
    /// </summary>
    public void TagAdd(string sentence,params string[] tags)
    {
        if (CorpusOrigin.ContainsKey(sentence))
        {
            foreach(var tag in tags)
            {
                CorpusOrigin[sentence].Tags.Add(tag);
                CorpusWithTagAdd(tag, sentence);
            }
        }
        else
        {
            CorpusOrigin.Add(sentence, new Item { Sentence = sentence });
            foreach (var tag in tags)
            {
                CorpusOrigin[sentence].Tags.Add(tag);
                CorpusWithTagAdd(tag, sentence);
            }
        }
    }

    /// <summary>
    /// 给现有的条目添加标签
    /// </summary>
    public void TagAdd(string sentence, List<string> tags)
    {
        if (CorpusOrigin.ContainsKey(sentence))
        {
            foreach (var tag in tags)
            {
                CorpusOrigin[sentence].Tags.Add(tag);
                //Debug.Log(string.Format("源列表添加sentence:{0} tag:{1}", sentence, tag));
                CorpusWithTagAdd(tag, sentence);
                //Debug.Log(string.Format("标签列表添加tag:{0} sentence:{1}", tag, sentence));
            }
        }
        else
        {
            CorpusOrigin.Add(sentence, new Item { Sentence = sentence,Tags=new HashSet<string>() });
            foreach (var tag in tags)
            {
                CorpusOrigin[sentence].Tags.Add(tag);
                //Debug.Log(string.Format("源列表添加sentence:{0} tag:{1}", sentence, tag));
                CorpusWithTagAdd(tag, sentence);
                //Debug.Log(string.Format("标签列表添加tag:{0} sentence:{1}", tag, sentence));
            }
        }
    }

    /// <summary>
    /// 获取符合条件的一条语句
    /// </summary>
    public string GetProperSentence(params string[] tags)
    {
        HashSet<string> result = CorpusWithTag[tags[0]];
        foreach(var tag in tags)
        {
            result.IntersectWith(CorpusWithTag[tag]);
        }
        IEnumerator<string> enumerator = result.GetEnumerator();
        enumerator.MoveNext();
        //Todo 这里可以随机
        return enumerator.Current;
    }

    /// <summary>
    /// 获取符合条件的一条语句
    /// </summary>
    public string GetProperSentence(List<string> tags)
    {
        string log1 = "";
        foreach(var tag in tags)
        {
            log1 += tag + " ";
        }

        Debug.Log(string.Format("搜索标签" + log1));
        if (tags.Count == 0)
        {
            Debug.Log("语料库为空");
            return "";
        }

        if (!CorpusWithTag.ContainsKey(tags[0]))
        {
            Debug.Log(string.Format("在语料库中找不到列表首标签{0}", tags[0]));
            return "……";
        }

        HashSet<string> result = new HashSet<string>(CorpusWithTag[tags[0]]);
        foreach (var tag in tags)
        {
            result.IntersectWith(CorpusWithTag[tag]);
        }
        IEnumerator<string> enumerator = result.GetEnumerator();
        enumerator.MoveNext();
        if (enumerator.Current==null)
        {
            string log = "";
            foreach(var item in tags)
            {
                log += item + " ";
            }
            Debug.Log(string.Format("在语料库中找不到符合条件({0})语句",log));
            DisplayTagCorpus();
            return "……";
        }

        Debug.Log(string.Format("从语料库返回{0}", enumerator.Current));
        return enumerator.Current;
    }

    /// <summary>
    /// 从文本初始化语料库
    /// </summary>
    public void ReadText()
    {
        using (StreamReader sr = new StreamReader("Assets//WheatDewCommunity//Data//Corpus.txt"))
        {
            
            string line;
            string tag = "";
            List<string> tags = new List<string>();

            while ((line = sr.ReadLine()) != null)
            {
                if (line[0] == '@')
                {
                    tag = line.Substring(1);
                    tags.Add(line.Substring(1));
                }
                else if (line[0]=='#')
                {
                    //目前识别后的处理方式和@一样
                    tag = line.Substring(1);
                    tags.Add(line.Substring(1));
                }
                else if (tag != "" && line != "")
                {
                    TagAdd(line, tags);

                    tag = "";
                    tags.Clear();
                }
            }
        }
        Debug.Log("语料库读入文件成功");
    }

    //简单函数
    /// <summary>
    /// 给以标签为索引的列表添加条目
    /// </summary>
    private void CorpusWithTagAdd(string tag, string sentence)
    {
        if (CorpusWithTag.ContainsKey(tag))
            CorpusWithTag[tag].Add(sentence);
        else
        {
            CorpusWithTag.Add(tag, new HashSet<string>());
            CorpusWithTag[tag].Add(sentence);
        }
    }

    /// <summary>
    /// 初始化对话命令的预制体
    /// </summary>
    private void InitializeDialogueCommandPrefab()
    {
        if (dialogueCommandPrefab == null)
        {
            Entities.ForEach((DialogueCommand dialogueCommand) =>
            {
                dialogueCommandPrefab = dialogueCommand;
                dialogueCommand.gameObject.SetActive(false);

            });
        }
        
    }

    //工作函数
    /// <summary>
    /// 捕获语料库命令生成最终对话命令
    /// </summary>
    private void CorpusJob()
    {
        Entities.ForEach((CorpusCommand corpusCommand) =>
        {

            DialogueCommand dialogueCommand = Object.Instantiate(dialogueCommandPrefab);
            dialogueCommand.origin = corpusCommand.origin;
            dialogueCommand.target = corpusCommand.target;
            dialogueCommand.content = GetProperSentence(corpusCommand.tags);

            dialogueCommand.gameObject.SetActive(true);
            string s="";
            foreach (var item in corpusCommand.tags) s += item+" ";


            Debug.Log("捕获语料库预制体成功( "+s+"),内容为" + dialogueCommand.content + "生成最终对话命令");
            Object.Destroy(corpusCommand.gameObject);
        });
    }

    //测试
    /// <summary>
    /// 测试源列表
    /// </summary>
    public void DisplayCorpus()
    {
        Debug.Log(CorpusOrigin.Count);
        foreach(var item in CorpusOrigin)
        {
            string log = item.Value.Sentence+"\n";
            foreach(var tag in item.Value.Tags)
            {
                log += tag+" ";
            }
            Debug.Log(log);
        }
    }

    /// <summary>
    /// 测试以标签为索引的列表
    /// </summary>
    public void DisplayTagCorpus()
    {
        Debug.Log(CorpusWithTag.Count);
        foreach (var item in CorpusWithTag)
        {
            string log = item.Key+"\n";
            foreach (var tag in item.Value)
            {
                log += tag + "\n";
            }
            Debug.Log(log);
        }
    }

    public void CreateCommand()
    {
        DialogueCommand cmd = Object.Instantiate(dialogueCommandPrefab);
        cmd.gameObject.SetActive(true);
    }

}
