using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public delegate InfoData Command(string[] values);
    public class CommandSystem : MonoBehaviour
    {
        private static CommandSystem _s;
        public static CommandSystem S { get { return _s; } }

        private Dictionary<string, Command> commandList = new Dictionary<string, Command>();

        private void Awake()
        {
            if (!_s) _s = this;
        }

        public InfoData Declare(string name, Command command)
        {
            commandList.Add(name, command);
            return null;
        }

        public InfoData Execute(string command)
        {
            //拆分命令（如果命令是由多条命令组成的话）
            string[] commands = command.Split('&');

            //拆分命令头和命令值
            for (int i = 0; i < commands.Length; i++)
            {
                Debug.Log(commands[i]);
                string[] values = commands[i].Split(' ');
                if (values.Length > 0)
                    commandList[values[0]](values);
                else
                    Debug.Log("命令长度错误");
            }

            return null;
        }

    }

    
    public static class CMath
    {
        public static Vector3 ToVector3(string origin)
        {
            origin =origin[1..^1];
            string[] values = origin.Split(',');
            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }

        
    }

    public class CommandData
    {
        
    }

    public class InfoData
    {
        public string info;
        public int controller;
    }

    
}

