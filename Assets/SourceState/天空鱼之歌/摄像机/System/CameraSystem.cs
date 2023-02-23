using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SkyWhale
{
    public class CameraSystem : MonoBehaviour
    {
        private static CameraSystem _instance;
        public static CameraSystem instance { get { return _instance; } }

        private void Awake()
        {
            if(_instance == null)
                _instance = this;

            InitCommand("清除所有摄像机",ClearAllCamera);
        }

        #region 层级

        public Transform CameraNode;

        #endregion


        #region 运行数据

        public Dictionary<string,GameObject> currentCameraList=new Dictionary<string, GameObject>();

        #endregion

        #region 存储数据
        public Dictionary<string, UnityAction<string>> commandData = new Dictionary<string, UnityAction<string>>();

        public Dictionary<string,GameObject> cameraData=new Dictionary<string, GameObject>();

        #endregion

        /// <summary>
        /// 初始化命令
        /// </summary>
        /// <param name="commandName">命令名</param>
        /// <param name="commandFunc">命令函数</param>
        public void InitCommand(string commandName,UnityAction<string> commandFunc)
        {
            commandData.Add(commandName, commandFunc);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandValue">命令字符串</param>
        public void ExecuteComand(string command)
        {
            string[] slices=command.Split('&');
            if (slices.Length == 1)
                commandData[slices[0]]("");
            if(slices.Length == 2)
                commandData[slices[0]](slices[1]);
        }
        
        //清除所有摄像机
        public void ClearAllCamera(string commandValue)
        {
            foreach(var item in currentCameraList)
            {
                Destroy(item.Value);
            }
            currentCameraList.Clear();
        }

        public void SetCamera(string commandValue)
        {

            ClearAllCamera("");

            currentCameraList.Add(commandValue, Instantiate(cameraData[commandValue],CameraNode));

        }

    }
}


