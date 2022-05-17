using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTextTransform : MonoBehaviour
{
    STextTransform inputTransformSystem;

    private void Start()
    {
        inputTransformSystem = FindObjectOfType<STextTransform>();
        STextTransform.inputTransformList.Add(name, this);
        STextTransform.focus = this;
    }
}
