using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public delegate InfoData Command(params string[] address);
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

        public int Execute(string name,params string[] address)
        {
            commandList[name](address);
            return 0;
        }
        public int Execute(string command)
        {
            string[] temp = command.Split(',');
            string[] values =new string[temp.Length - 1];
            for(int i = 0; i < values.Length; i++)
            {
                values[i] = temp[i + 1];
            }
            commandList[temp[0]](values);
            return 0;
        }

        //public int Subscribe(string name, Command command)
        //{
        //    commandList[name] += command;
        //    return 0;
        //}

        //public int Unsubscribe(string name, Command command)
        //{
        //    commandList[name] -= command;
        //    return 0;
        //}

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

