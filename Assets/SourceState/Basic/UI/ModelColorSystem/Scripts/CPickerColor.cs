using ColorUiTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPickerColor : MonoBehaviour
{
    public Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        SPickerColor.focus = this;
    }
}
