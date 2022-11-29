using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraView {ViewIndoor,ViewOutdoor};
public class ViewIndoorRange : MonoBehaviour
{
    ObstacleCamera obstacleCamera;

    public CameraView view;

    private void Start()
    {
        obstacleCamera = FindObjectOfType<ObstacleCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform== obstacleCamera.cameraSetting.Follow)
        {
            if (view == CameraView.ViewIndoor)
                obstacleCamera.SetIndoorView();
            else if(view == CameraView.ViewOutdoor)
                obstacleCamera.SetOutdoorView();
        }
    }
}
