using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


//public delegate void task(string[] values, CommandData taskData);
//public delegate float floatTask(string[] values, CommandData taskData);
//public delegate int intTask(string[] values, CommandData taskData);

public delegate void command(string command, CommandData commandData);

public class SCommand
{

    //public static SCommand taskData;

    //private static Dictionary<string, task> tasks = new Dictionary<string, task>();
    //private static Dictionary<string, floatTask> floatCommands = new Dictionary<string, floatTask>();

    //�����б�
    private static Dictionary<Regex, command> commands = new Dictionary<Regex, command>();

    ////ע����Ϊ
    //public static void Declare(string name, task behaviour)
    //{
    //    tasks.Add(name, behaviour);
    //}
    //public static void Declare(string name,floatTask command)
    //{
    //    floatCommands.Add(name, command);
    //}


    //public static void Execute(string[] values)
    //{
    //    if (values != null && values.Length > 0 && tasks.ContainsKey(values[0]))
    //    {
    //        tasks[values[0]](values, new CommandData());
    //    }
    //}

    //public static float GetFloatData(string[] values)
    //{
    //    if (values != null && values.Length > 0 && floatCommands.ContainsKey(values[0]))
    //    {
    //        return floatCommands[values[0]](values, new CommandData());
    //    }
    //    else
    //    {
    //        string command = "";
    //        for(int i = 0; i < values.Length; i++)
    //        {
    //            command+=values[i];
    //        }
    //        Debug.LogError("�������:" + command);
    //        return 0f;
    //    }
    //}


    //���װ汾
    public static void Declare(string name,command command)
    {
        commands.Add(new Regex(name), command);
        string[] slices = name.Split(' ');
        for (int i = 0; i < slices.Length; i++)
        {
            if (!slices[i].Contains('?'))
            {
                SWord.keyWords.Add(slices[i]);
            }
        }
    }

    public static void Execute(string command,CommandData commandData=null)
    {
        try
        {
            if (command != null && command != "")
            {
                string[] slices = command.Split('\n');

                for (int i = 0; i < slices.Length; i++)
                {
                    RegexTest(slices[i])(command, commandData);
                    //string result = SWord.GetSentence(slices[i]);
                    //CommandModule commandModule = SWord.MatchModule(slices[i]);
                    //commands[commandModule.command](result, commandData);
                    //commands[command](slices[i]);
                }
            }
        }
        catch(System.Exception ex)
        {
            Debug.LogErrorFormat("ִ������Ϊ:{0}\n������ϢΪ:{1}", command,ex.ToString());
        }
    }

    //�ж��Ƿ�ƥ������
    public static command RegexTest(string command)
    {
        foreach(var item in commands)
        {
            //Debug.Log(string.Format("����{0},ƥ����") item.Key);
            if(item.Key.IsMatch(command))
                return item.Value;
        }
        return null;
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


