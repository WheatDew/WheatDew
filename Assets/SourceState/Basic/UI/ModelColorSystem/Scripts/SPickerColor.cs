using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPickerColor : MonoBehaviour
{
    public PPickerColor pickColorPagePrefab;
    private PPickerColor pickColorPage;

    public static CPickerColor focus;

    public void OpenPickColorPage()
    {
        if (pickColorPage == null)
        {
            pickColorPage = Instantiate(pickColorPagePrefab, FindObjectOfType<Canvas>().transform);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OpenPickColorPage();

        }
    }
}
