using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class MSceneInfo : MonoBehaviour
{
    //[SerializeField] private InfoInScenePage prefab;
    //private InfoInScenePage current;

    //private void Start()
    //{
    //    SCommand.Declare("显示信息", DisplayInfoInSceneCommand);
    //    SCommand.Declare("关闭显示信息",ClosedInfoInSceneCommand );
    //}

    //public void DisplayInfoInSceneCommand(string[] values,CommandData info)
    //{
    //    DisplayInfoInScene(values[1]);
    //}

    //public void ClosedInfoInSceneCommand(string[] values, CommandData info)
    //{
    //    ClosedInfoInScene();
    //}



    ////功能函数
    //public void DisplayInfoInScene(string content)
    //{
    //    if (current == null)
    //    {
    //        current = Instantiate(prefab,SRange.infoInSceneParent);
    //        current.infoText.text = content;
    //    }
    //    else
    //    {
    //        current.infoText.text = content;
    //    }
    //}

    //public void ClosedInfoInScene()
    //{
    //    if (current != null)
    //    {
    //        Destroy(current.gameObject);
    //    }

    //}
}

