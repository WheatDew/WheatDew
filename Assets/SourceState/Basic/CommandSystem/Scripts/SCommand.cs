using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public delegate int Command(string address);
    public class SCommand : MonoBehaviour
    {
        private static Dictionary<string, Command> commandList = new Dictionary<string, Command>();

        public static int Declare(string name, Command command)
        {
            commandList.Add(name, command);
            return 0;
        }

        public static int Execute(string name, string address)
        {
            commandList[name](address);
            return 0;
        }

        public static int Subscribe(string name, Command command)
        {
            commandList[name] += command;
            return 0;
        }

        public static int Unsubscribe(string name, Command command)
        {
            commandList[name] -= command;
            return 0;
        }

    }



}

