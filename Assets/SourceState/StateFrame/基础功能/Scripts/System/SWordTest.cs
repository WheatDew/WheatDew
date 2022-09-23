using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWordTest : MonoBehaviour
{
    private void Start()
    {
        SWord.keyWords = new HashSet<string> { "��ʼ", "����", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{0}" };

        //SWord.Tokenizer("��ʼ{����һ����������}�ڽ���");

        SCommand.Declare(@"��ӡ \S+?", TestCommand);

        //SCommand.Execute(SWord.Tokenizer("��ӡ������Ϣ"), null);
    }

    public void TestCommand(string commandName,CommandData commandData)
    {
        string s = commandName.Split(' ')[1];
        Debug.Log(s+"#"+commandName);
    }
}
