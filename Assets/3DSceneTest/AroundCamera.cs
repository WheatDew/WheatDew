using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AroundCamera : MonoBehaviour
{
    public Transform aroundCenter;

    public void SetTopView()
    {
        aroundCenter.rotation = Quaternion.AngleAxis(90f, Vector3.right);
    }

    public void SetUpwardView()
    {
        aroundCenter.rotation = Quaternion.AngleAxis(-90f, Vector3.right);
    }

    public void SetFrontView()
    {
        aroundCenter.rotation = Quaternion.AngleAxis(0f, Vector3.right);
    }

    public void SetRearView()
    {
        aroundCenter.rotation = Quaternion.AngleAxis(180f, Vector3.right);
    }

    public void SetLeftView()
    {
        aroundCenter.rotation = Quaternion.AngleAxis(90f, Vector3.up);
    }

    public void SetRightView()
    {
        aroundCenter.rotation = Quaternion.AngleAxis(-90f, Vector3.up);
    }


}
