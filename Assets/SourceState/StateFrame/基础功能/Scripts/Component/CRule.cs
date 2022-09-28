using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRule : MonoBehaviour
{
    protected delegate string Rule();
    protected delegate string Result(string command);
    protected Dictionary<string, Rule> rules = new Dictionary<string, Rule>();
    protected Dictionary<string, Result> results = new Dictionary<string, Result>();

    protected void SetRule(string key,Rule rule)
    {
        rules.Add(key,rule);
    }

    protected string ApplyRule(string command)
    {
        Result result = null;
        string[] commands=command.Split('\n');
        for(int i = 0; i < commands.Length; i++)
        {
            string[] slices = commands[i].Split(' ');
            for(int j = 0; j < slices.Length; j++)
            {
                if (rules.ContainsKey(slices[j]))
                    result += result;
                //ToDo:
            }
        }



        return null;
        //return (string s) => { return ""; };
        //string[] slices = command.Split(' ');

        //for(int i = 0; i < slices.Length; i++)
        //{
        //    if (rules.ContainsKey(slices[i]))
        //    {
        //        return Rule;
        //    }
        //}
    }
}
