using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWordTest : MonoBehaviour
{
    private void Start()
    {
        SWord.keyWords = new HashSet<string> { "开始", "结束", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}", "{7}", "{8}", "{9}", "{0}" };

        SWord.Tokenizer("开始{这是一段文字内容}于结束");

        //foreach(var item in result)
        //{
        //    Debug.Log(item.Key);
        //}
    }
}
