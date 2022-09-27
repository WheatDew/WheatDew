using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRule : MonoBehaviour
{
    protected delegate string Rule();
    protected delegate string Result(string command);
    protected Dictionary<string, Rule> rules = new Dictionary<string, Rule>();

    protected void SetRule(string key,Rule rule)
    {
        rules.Add(key,rule);
    }

    protected Rule ApplyRule(string command)
    {

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
