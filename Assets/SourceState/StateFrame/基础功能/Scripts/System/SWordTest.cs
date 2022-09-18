using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWordTest : MonoBehaviour
{
    private void Start()
    {
        SWord.keyWords = new HashSet<string> { "开始", "结束" };
        Dictionary<string,List<WordData>> result = new Dictionary<string,List<WordData>>();

        SWord.Combination(SWord.Tokenizer("开始于结束"), result);

        foreach(var item in result)
        {
            Debug.Log(item.Key);
        }
    }
}
