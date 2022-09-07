using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SCommand.Declare(@"打印 \S+? 的平方", test);
        SWord.regexs.Add(new Regex(@"打印 \S+? 的平方"), new CommandModule(@"打印 \S+? 的平方"));
        SWord.words.Add("打印");
        SWord.words.Add("的平方");
    }

    public void test(string value,CommandData commandData)
    {
        Debug.Log("00");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            
        }
    }
}
