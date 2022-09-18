using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public static class SWord
{
    public static HashSet<string> words = new HashSet<string>();

    public static Dictionary<Regex,CommandModule> regexs=new Dictionary<Regex, CommandModule>();

    
    public static string GetSentence(string command)
    {
        List<string> result = new List<string>();

        int lastindex = 0;
        for(int index=0; index<command.Length; index++)
        {
            string temp=command[index..];
            for (int i = 0; i <= temp.Length; i++)
            {
                //Debug.Log(temp+" "+temp[..i]+" "+i.ToString());
                if (words.Contains(temp[..i]))
                {
                    if (lastindex != index)
                    {
                        result.Add(command[lastindex..index]);
                    }

                    result.Add(temp[..i]);

                    index += i-1;
                    lastindex = index+1;

                    break;
                }
                
            }
        }

        string display = "";
        foreach(var item in result)
        {
            display += item+" ";
        }
        return display;


    }

    public static CommandModule MatchModule(string command)
    {
        foreach(var item in regexs)
        {
            if (item.Key.IsMatch(command))
            {
                return item.Value;
            }
        }

        return null;
    }


    //ÐÂ·Ö´ÊÆ÷
    public static HashSet<string> keyWords = new HashSet<string>();

    public static List<WordData> Tokenizer(string sentence)
    {
        List<WordData> words = new List<WordData>();
        for (int i = 0; i < sentence.Length; i++)
        {
            for (int j = i + 1; j <= sentence.Length; j++)
            {
                //words.Add(sentence[i..j]);
                if (keyWords.Contains(sentence[i..j]))
                    words.Add(new WordData(sentence[i..j], i, j - 1));
            }
        }
        return words;
    }

    public static void Combination(List<WordData> words, Dictionary<string, List<WordData>> result, int start=0, string current="", List<WordData> currentWords=null)
    {
        string c = new string(current);
        List<WordData> cw;
        if (currentWords != null)
            cw = new List<WordData>(currentWords);
        else
        {
            currentWords = new List<WordData>();
            cw = new List<WordData>(currentWords);
        }


        for (int i = start; i < words.Count; i++)
        {
            if (c == "")
            {

                result.Add(words[i].word, new List<WordData> { words[i] });
                current = words[i].word;
            }
            else
            {
                currentWords.Add(words[i]);
                result.Add(c + " " + words[i].word, new List<WordData>(currentWords));
                current = c + " " + words[i].word;
            }
            Combination(words, result, i + 1, current, currentWords);
        }

    }
}

public class CommandModule
{
    public string command;

    public CommandModule(string command)
    {
        this.command = command;
    }
}

public class WordData
{
    public int start, end;
    public string word;

    public WordData(string word, int start, int end)
    {
        this.word = word;
        this.start = start;
        this.end = end;
    }


}
