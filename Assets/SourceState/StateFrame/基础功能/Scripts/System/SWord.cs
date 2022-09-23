using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;

public static class SWord
{
    public static HashSet<string> words = new HashSet<string>();

    //public static Dictionary<Regex,CommandModule> regexs=new Dictionary<Regex, CommandModule>();

    //ת���б�
    static Dictionary<string, int> convertList = new Dictionary<string, int> { { "{0}", 0 }, { "{1}", 1 }, { "{2}", 2 } };
    
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

    //public static CommandModule MatchModule(string command)
    //{
    //    foreach(var item in regexs)
    //    {
    //        if (item.Key.IsMatch(command))
    //        {
    //            return item.Value;
    //        }
    //    }

    //    return null;
    //}


    //�·ִ���
    public static HashSet<string> keyWords = new HashSet<string>();

    //�ִ�
    public static string Tokenizer(string sentence)
    {
        //ת�����������
        List<string> replace = new List<string>();
        sentence = SentenceReplace(sentence,replace);

        //�ִ�
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

        //���
        Dictionary<string, List<WordData>> result = new Dictionary<string, List<WordData>>();
        Combination(words, result);

        

        Debug.Log(string.Format("�������ĳ���Ϊ��{0};ԭ���Ϊ��{1}", result.Count,sentence));
        //��ȡ������
        List<WordData> resultList = result[GetMaxResult(result)];
        List<int> indexs = new List<int>();
        for(int i = 0; i < resultList.Count; i++)
        {
            int index = sentence.IndexOf(resultList[i].word);
            Debug.Log(index);
            if (!indexs.Contains(index))
            {
                indexs.Add(index);
            }
            if (!indexs.Contains(index + resultList[i].word.Length))
            {
                indexs.Add(index + resultList[i].word.Length);
            }
        }
        QuickSort(indexs, 0, indexs.Count - 1);

        string final = "";
        for(int i = 0; i < indexs.Count; i++)
        {
            if (i == indexs.Count - 1)
            {

                final +=ConvertMarkWords(sentence[indexs[i]..],replace);
            }
            else
            {
                if (i == 0 && indexs[i]!=0)
                {
                    final += ConvertMarkWords(sentence[..indexs[i]], replace) + " ";
                }
                final += ConvertMarkWords(sentence[indexs[i]..indexs[i+1]], replace) + " ";
            }
            
        }

        return final;
    }

    //����ǵĴ���ת��
    public static string ConvertMarkWords(string input,List<string> replace)
    {
        //Debug.Log(string.Format("��׽�ʣ�{0}", input));
        if (convertList.ContainsKey(input))
        {
            Debug.Log(replace[convertList[input]]);
            return replace[convertList[input]];
        }

        return input;
    }

    //���
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

    //��ȡ��ϵ������
    

    //�ж��Ƿ����
    public static string GetMaxResult(Dictionary<string, List<WordData>> result)
    {
        string temp=null;

        foreach(var item in result)
        {
            if (temp == null)
                temp = item.Key;
            else
            {
                if (item.Key.Contains(temp))
                {
                    temp = item.Key;
                }
            }
        }
        return temp;
    }

    public static string SentenceReplace(string sentence,List<string> contents)
    {
        int index = 0;
        string[] slices = sentence.Split('{', '}');
        //Debug.Log(sentence);
        if (sentence[0] != '{')
        {
            for (int i = 1; i < slices.Length; i += 2)
            {
                contents.Add(slices[i]);
                slices[i] = string.Format("{{0}}", index);
            }
        }
        else
        {
            for (int i = 0; i < slices.Length; i += 2)
            {
                slices[i] = string.Format("{{0}}", index);
            }
        }
        string result = "";
        for (int i = 0; i < slices.Length; i++)
        {
            result+=slices[i];
        }

        return result;
    }

    //��ȡ������ֵ���ҷ����������λ��
    private static int Division(List<int> list, int left, int right)
    {
        while (left < right)
        {
            int num = list[left]; //����Ԫ����Ϊ����
            if (num > list[left + 1])
            {
                list[left] = list[left + 1];
                list[left + 1] = num;
                left++;
            }
            else
            {
                int temp = list[right];
                list[right] = list[left + 1];
                list[left + 1] = temp;
                right--;
            }
            Console.WriteLine(string.Join(",", list));
        }
        Console.WriteLine("--------------\n");
        return left; //ָ��Ĵ�ʱ�����λ��
    }
    private static void QuickSort(List<int> list, int left, int right)
    {
        if (left < right)
        {
            int i = Division(list, left, right);
            //���������߲��ֽ�������
            QuickSort(list, i + 1, right);
            //��������ұ߲��ֽ�������
            QuickSort(list, left, i - 1);
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
