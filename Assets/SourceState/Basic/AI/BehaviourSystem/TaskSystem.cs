using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Origin
{
    public delegate void task(string[] values,TaskData taskData);
    public class TaskSystem : MonoBehaviour
    {
        private static TaskSystem _s;
        public static TaskSystem s { get { return _s; } }

        public static TaskData taskData;

        private Dictionary<string, task> behaviourList = new Dictionary<string, task>();
        public Dictionary<string, TaskComponent> components = new Dictionary<string, TaskComponent>();

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
        public IEnumerator Execute(string command,float time)
        {
            yield return new WaitForSeconds(time);
            CommandSystem.s.Execute(command);
        }

        //延时执行行为(字符串版本)
        public async void Execute(string value)
        {
            Debug.Log("执行" + value);
            string[] commands=value.Split('@');
            if (commands.Length == 2)
                await new WaitForSeconds(float.Parse(commands[1]));
            string[] values = commands[0].Split(' ');
            behaviourList[values[0]](values,new TaskData());
        }
        
    }

    public class TaskData
    {
        
    }
}

