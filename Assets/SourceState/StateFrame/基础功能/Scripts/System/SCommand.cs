using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public delegate void task(string[] values, CommandData taskData);

    public class SCommand 
    {

        public static SCommand taskData;

        private static Dictionary<string, task> behaviourList = new Dictionary<string, task>();


        //×¢²áÐÐÎª
        public static void Declare(string name, task behaviour)
        {
            behaviourList.Add(name, behaviour);
        }


        public static void Execute(string[] values)
        {
            if (values != null && values.Length > 0 && behaviourList.ContainsKey(values[0]))
            {
                behaviourList[values[0]](values, new CommandData());
            }
        }
    }

    public class CommandData
    {

    }
}

