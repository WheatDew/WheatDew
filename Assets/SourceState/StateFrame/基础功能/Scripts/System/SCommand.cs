using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public delegate void task(string[] values, CommandData taskData);
public delegate float floatTask(string[] values, CommandData taskData);
public delegate int intTask(string[] values, CommandData taskData);

public delegate void command(string command, CommandData commandData);

public class SCommand
{

    public static SCommand taskData;

    private static Dictionary<string, task> tasks = new Dictionary<string, task>();
    private static Dictionary<string, floatTask> floatCommands = new Dictionary<string, floatTask>();

    private static Dictionary<string, command> commands = new Dictionary<string, command>();

    //×¢²áÐÐÎª
    public static void Declare(string name, task behaviour)
    {
        tasks.Add(name, behaviour);
    }
    public static void Declare(string name,floatTask command)
    {
        floatCommands.Add(name, command);
    }


    public static void Execute(string[] values)
    {
        if (values != null && values.Length > 0 && tasks.ContainsKey(values[0]))
        {
            tasks[values[0]](values, new CommandData());
        }
    }

    public static float GetFloatData(string[] values)
    {
        if (values != null && values.Length > 0 && floatCommands.ContainsKey(values[0]))
        {
            return floatCommands[values[0]](values, new CommandData());
        }
        else
        {
            string command = "";
            for(int i = 0; i < values.Length; i++)
            {
                command+=values[i];
            }
            Debug.LogError("ÃüÁî´íÎó:" + command);
            return 0f;
        }
    }


    //¼òÒ×°æ±¾
    public static void Declare(string name,command command)
    {
        commands.Add(name, command);
        SWord.regexs.Add(new Regex(name), new CommandModule(name));
        string[] slices = name.Split(' ');
        for(int i = 0; i < slices.Length; i++)
        {
            if (!slices[i].Contains('?'))
            {
                SWord.words.Add(slices[i]);
            }
        }
    }

    public static void Execute(string command,CommandData commandData)
    {
        if (command != null && command != "")
        {
            string[] slices = command.Split('\n');

            for (int i = 0; i < slices.Length; i++)
            {
                string result = SWord.GetSentence(slices[i]);
                CommandModule commandModule = SWord.MatchModule(result);
                commands[commandModule.command](result, commandData);
            }
        }
    }

}

public delegate string replace(string key);
public class CommandData
{
    public string key;
    public Dictionary<string, replace> replaceDatas;

    public CommandData()
    {

    }

    public CommandData(string key, Dictionary<string, replace> replaceDatas)
    {
        this.key = key;
        this.replaceDatas = replaceDatas;
    }
}


