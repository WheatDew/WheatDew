using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PathFindingSystem : MonoBehaviour
    {
        private static PathFindingSystem _s;
        public static PathFindingSystem s { get { return _s; } }

        private void Awake()
        {
            if (!_s) _s = this;
        }

        public Dictionary<string, PathFindingComponent> componentList = new Dictionary<string, PathFindingComponent>();

        #region 任务


        #endregion

        #region 功能

        public void SetTargetPosition(string key,Vector3 targetPosition)
        {
            componentList[key].agent.destination = targetPosition;
        }

        #endregion
    }
}

