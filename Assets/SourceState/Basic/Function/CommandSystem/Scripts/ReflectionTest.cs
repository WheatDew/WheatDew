using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Origin
{
    public class ReflectionTest : MonoBehaviour
    {
        public void Method()
        {

        }
        public void Notify()
        {

        }
        public int Count;
        public int Max;
        private string mName;
        private int mLevel;
        public string Name
        {
            get { return mName; }
            set { Name = value; }
        }

        public int Level
        {
            get { return mLevel; }
            set { mLevel = value; }
        }
        void Start()
        {
            List<string> ScrInfos = new List<string>();
            Type my = this.GetType();
            ScrInfos.Add(my.Name);//����
            ScrInfos.Add(my.Namespace);//���������ռ�
            ScrInfos.Add(my.Assembly.ToString());//������Ϣ
            FieldInfo[] fieinfos = my.GetFields();//��ȡ�ֶμ���(public)
            foreach (FieldInfo info in fieinfos)
            {
                ScrInfos.Add(info.ToString());
            }
            PropertyInfo[] proinfos = my.GetProperties();//��ȡ���Լ���(public)
            for (int i = 0; i < 2; i++)
            {
                ScrInfos.Add(proinfos[i].ToString());
            }
            MethodInfo[] methinfos = my.GetMethods();//��ȡ���еķ���
            for (int i = 0; i < 2; i++)
            {
                ScrInfos.Add(methinfos[i].ToString());
            }
            foreach (string str in ScrInfos)
            {
                Debug.Log(str);
            }
        }

    }
}


namespace Origin
{
    public class TestClass
    {
        public string name = "test";
    }
}
