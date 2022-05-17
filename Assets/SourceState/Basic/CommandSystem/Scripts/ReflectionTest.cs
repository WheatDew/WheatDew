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
            ScrInfos.Add(my.Name);//类名
            ScrInfos.Add(my.Namespace);//所属命名空间
            ScrInfos.Add(my.Assembly.ToString());//程序集信息
            FieldInfo[] fieinfos = my.GetFields();//获取字段集合(public)
            foreach (FieldInfo info in fieinfos)
            {
                ScrInfos.Add(info.ToString());
            }
            PropertyInfo[] proinfos = my.GetProperties();//获取属性集合(public)
            for (int i = 0; i < 2; i++)
            {
                ScrInfos.Add(proinfos[i].ToString());
            }
            MethodInfo[] methinfos = my.GetMethods();//获取所有的方法
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
