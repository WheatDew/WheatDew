using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWordTest : MonoBehaviour
{
    private void Start()
    {
        SWord.keyWords = new HashSet<string> { "��ʼ", "����", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{0}" };

        SWord.Tokenizer("��ʼ{����һ����������}�ڽ���");

        //foreach(var item in result)
        //{
        //    Debug.Log(item.Key);
        //}
    }
}
