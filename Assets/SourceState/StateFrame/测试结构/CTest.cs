using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SCommand.Declare(@"��ӡ \S+? ��ƽ��", test);
        SWord.regexs.Add(new Regex(@"��ӡ \S+? ��ƽ��"), new CommandModule(@"��ӡ \S+? ��ƽ��"));
        SWord.words.Add("��ӡ");
        SWord.words.Add("��ƽ��");
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
