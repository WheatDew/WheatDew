using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STextTransform : MonoBehaviour
{
    //æ≤Ã¨±‰¡ø
    public static Dictionary<string, CTextTransform> inputTransformList = new Dictionary<string, CTextTransform>();
    public static CTextTransform focus;

    public PTextTransform textTransformPagePrefab;
    private PTextTransform textTransformPage;

    public void OpenInputTransformPage()
    {
        if (textTransformPage == null)
        {
            textTransformPage = Instantiate(textTransformPagePrefab,FindObjectOfType<Canvas>().transform);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OpenInputTransformPage();
        }
    }
}
