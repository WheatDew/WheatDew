using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CTest : MonoBehaviour
{
    [TextArea(1,4)]public string PressKeyG;
    Dictionary<string,string> infolist=new Dictionary<string, string>();
    public bool isname=false;

    void Start()
    {
        infolist.Add("名字", "小希");
        infolist.Add("年龄", "16");

        SCommand.Declare(@"打印 \S+? 的平方", test);
        SCommand.Declare(@"告诉我 你的 \S+? 吧",sayinfo);

    }

    public void sayinfo(string value,CommandData commandData)
    {
        if (!isname)
        {
            string[] values = value.Split(' ');
            Debug.Log(infolist[values[2]]);
            isname = true;
        }
        else
        {
            Debug.Log("你刚刚不是问过吗？");
        }
    }

    public void test(string value,CommandData commandData)
    {
        string[] values = value.Split(' ');
        Debug.Log(string.Format("打印结果为：{0}",Mathf.Pow(float.Parse(values[1]),2)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            SCommand.Execute(PressKeyG,null);
        }
    }
}
