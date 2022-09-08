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


}

public class CommandModule
{
    public string command;

    public CommandModule(string command)
    {
        this.command = command;
    }
}
