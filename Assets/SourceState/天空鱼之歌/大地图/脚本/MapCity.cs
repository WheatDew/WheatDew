using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapCity : MonoBehaviour
{
    public TMP_Text stateName;
    public Transform cameraPosition;

    private void Update()
    {
        stateName.transform.LookAt(cameraPosition);
    }


}
