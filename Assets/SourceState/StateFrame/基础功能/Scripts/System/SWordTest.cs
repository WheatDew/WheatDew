using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWordTest : MonoBehaviour
{
    private void Start()
    {
        SWord.keyWords = new HashSet<string> { "��ʼ", "����" };
        Dictionary<string,List<WordData>> result = new Dictionary<string,List<WordData>>();

        SWord.Combination(SWord.Tokenizer("��ʼ�ڽ���"), result);

        foreach(var item in result)
        {
            Debug.Log(item.Key);
        }
    }
}
