using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Camera mapCamera;
    public Transform cameraParent;
    public GameObject statePrefab;


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateState();
        }

        CameraController();
    }

    //在点击处生成物体
    public void CreateState()
    {
        Ray ray=mapCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit result;
        if(Physics.Raycast(ray, out result,400,LayerMask.GetMask("Ground")))
        {
            var obj = Instantiate(statePrefab);
            obj.transform.position = result.point;
        }
    }

    public void CameraController()
    {
        var cameraPosition = mapCamera.transform.localPosition;

        cameraPosition.z += Input.GetAxisRaw("Mouse ScrollWheel")*10f;

        if (Input.GetMouseButton(2))
        {
            cameraParent.position += new Vector3(-Input.GetAxisRaw("Mouse X"),0 , -Input.GetAxisRaw("Mouse Y"));
        }


        mapCamera.transform.localPosition=cameraPosition;
    }
}
