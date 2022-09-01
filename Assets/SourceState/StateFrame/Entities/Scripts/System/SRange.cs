using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State
{
    public class SRange : MonoBehaviour
    {

        private static SRange _s;
        public static SRange s { get { return _s; } }

        private void Awake()
        {
            if (_s == null) _s = this;
        }

        //�б�
        public Dictionary<string, CRange> citems = new Dictionary<string, CRange>();

        //ģ��
        public MSceneInfo sceneInfo;

        //UI������
        public Transform infoInSceneParent;

    }
}

