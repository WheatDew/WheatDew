using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class SCommandTest : MonoBehaviour
    {
        private void Start()
        {
            //SCommand.Add("测试命令", TestCommand);
            //SCommand.Excute("测试命令", "测试命令执行成功");
        }

        public string TestCommand(string s)
        {
            print(s);
            return "";
        }
    }
}

