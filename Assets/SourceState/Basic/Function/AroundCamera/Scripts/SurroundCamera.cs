using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Origin
{
    public class SurroundCamera : MonoBehaviour
    {
        public Transform targetCamera;
        public bool isControlled=true;
        private void Update()
        {
            if (isControlled&& EventSystem.current.IsPointerOverGameObject())//控制是否可控
            {
                if (Input.GetMouseButton(2))
                {
                    Vector3 settingPosition = transform.localPosition;
                    settingPosition.x += Input.GetAxis("Mouse X");
                    settingPosition.y += Input.GetAxis("Mouse Y");
                    transform.localPosition = settingPosition;
                }
                if (Input.GetMouseButton(1))
                {
                    transform.localRotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0);
                    Vector3 settingRotation = transform.localRotation.eulerAngles;
                    settingRotation.z = 0;
                    transform.localRotation = Quaternion.Euler(settingRotation);
                }
            }
        }
    }
}

