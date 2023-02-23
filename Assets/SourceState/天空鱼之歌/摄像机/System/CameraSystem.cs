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

            InitCommand("������������",ClearAllCamera);
        }

        #region �㼶

        public Transform CameraNode;

        #endregion


        #region ��������

        public Dictionary<string,GameObject> currentCameraList=new Dictionary<string, GameObject>();

        #endregion

        #region �洢����
        public Dictionary<string, UnityAction<string>> commandData = new Dictionary<string, UnityAction<string>>();

        public Dictionary<string,GameObject> cameraData=new Dictionary<string, GameObject>();

        #endregion

        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <param name="commandName">������</param>
        /// <param name="commandFunc">�����</param>
        public void InitCommand(string commandName,UnityAction<string> commandFunc)
        {
            commandData.Add(commandName, commandFunc);
        }

        /// <summary>
        /// ִ������
        /// </summary>
        /// <param name="commandValue">�����ַ���</param>
        public void ExecuteComand(string command)
        {
            string[] slices=command.Split('&');
            if (slices.Length == 1)
                commandData[slices[0]]("");
            if(slices.Length == 2)
                commandData[slices[0]](slices[1]);
        }
        
        //������������
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


