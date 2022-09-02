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

        //列表
        public Dictionary<string, CRange> citems = new Dictionary<string, CRange>();

        //模块
        public MSceneInfo sceneInfo;

        //UI父物体
        public Transform infoInSceneParent;

    }
}

