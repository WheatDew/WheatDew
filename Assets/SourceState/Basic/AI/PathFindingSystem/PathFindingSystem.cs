using System;
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


        public void CommandInit()
        {
            CommandSystem.S.Declare("SetTargetPosition", SetTargetPositionCommand);
        }

        //√¸¡Ó
        private InfoData SetTargetPositionCommand(string[] values)
        {
            componentList[values[0]].agent.destination = CMath.ToVector3(values[1]);
            return null;
        }

    }
}

