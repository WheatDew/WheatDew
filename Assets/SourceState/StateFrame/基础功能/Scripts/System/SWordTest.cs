using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWordTest : MonoBehaviour
{
    private void Start()
    {
        SWord.keyWords = new HashSet<string> { "开始", "结束", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{0}" };

        //SWord.Tokenizer("开始{这是一段文字内容}于结束");

        SCommand.Declare(@"打印 \S+?", TestCommand);

        //SCommand.Execute(SWord.Tokenizer("打印测试信息"), null);
    }

    public void TestCommand(string commandName,CommandData commandData)
    {
        string s = commandName.Split(' ')[1];
        Debug.Log(s+"#"+commandName);
    }
}
