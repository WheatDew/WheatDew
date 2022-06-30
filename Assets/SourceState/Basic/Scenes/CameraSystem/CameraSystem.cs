using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private static CameraSystem _s;
    public static CameraSystem s { get { return _s; } }

    private void Awake()
    {
        if(!_s)_s = this;
    }

    public Cinemachine.CinemachineBrain camera;

    public void DisableCameraMoving()
    {
        camera.enabled = false;
    }
}
