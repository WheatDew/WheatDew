using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public delegate void task(string[] values, CommandData taskData);

    public class SCommand : MonoBehaviour
    {
        private static SCommand _s;
        public static SCommand s { get { return _s; } }

        public static SCommand taskData;

        private Dictionary<string, task> behaviourList = new Dictionary<string, task>();

        private void Awake()
        {
            if (!_s) _s = this;
        }

        //注册行为
        public void Declare(string name, task behaviour)
        {
            behaviourList.Add(name, behaviour);
        }

        //延时执行行为(参数版本)
        public IEnumerator Execute(string command, float time)
        {
            yield return new WaitForSeconds(time);
            SCommand.s.Execute(command);
        }

        //延时执行行为(字符串版本)
        public async void Execute(string value)
        {
            if (value != null && value != "")
            {
                Debug.Log("执行" + value);
                string[] commands = value.Split('@');
                if (commands.Length == 2)
                    await new WaitForSeconds(float.Parse(commands[1]));
                string[] values = commands[0].Split(' ');
                behaviourList[values[0]](values, new CommandData());
            }
        }

        public void ExecuteAlone(string[] values)
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

