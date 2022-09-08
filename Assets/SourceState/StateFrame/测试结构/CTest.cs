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
        infolist.Add("����", "Сϣ");
        infolist.Add("����", "16");

        SCommand.Declare(@"��ӡ \S+? ��ƽ��", test);
        SCommand.Declare(@"������ ��� \S+? ��",sayinfo);

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
            Debug.Log("��ող����ʹ���");
        }
    }

    public void test(string value,CommandData commandData)
    {
        string[] values = value.Split(' ');
        Debug.Log(string.Format("��ӡ���Ϊ��{0}",Mathf.Pow(float.Parse(values[1]),2)));
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
