using ColorUiTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPickerColor : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<ColorPicker>().onPicker.AddListener(color =>
        {
            SPickerColor.focus.material.color = color;
            print(color);
        });
    }
}
