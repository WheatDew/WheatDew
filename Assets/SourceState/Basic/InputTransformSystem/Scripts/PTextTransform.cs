using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PTextTransform : MonoBehaviour
{
    public InputField x_p, y_p, z_p,x_r,y_r,z_r,x_s,y_s,z_s;

    private void Start()
    {
        x_p.onEndEdit.AddListener(delegate { SetFocusPosition(); });
        y_p.onEndEdit.AddListener(delegate { SetFocusPosition(); });
        z_p.onEndEdit.AddListener(delegate { SetFocusPosition(); });
        x_r.onEndEdit.AddListener(delegate { SetFocusRotation(); });
        y_r.onEndEdit.AddListener(delegate { SetFocusRotation(); });
        z_r.onEndEdit.AddListener(delegate { SetFocusRotation(); });
        x_s.onEndEdit.AddListener(delegate { SetFocusScale(); });
        y_s.onEndEdit.AddListener(delegate { SetFocusScale(); });
        z_s.onEndEdit.AddListener(delegate { SetFocusScale(); });
    }

    public void SetFocusPosition()
    {
        STextTransform.focus.transform.localPosition 
            = new Vector3(float.Parse(x_p.text), float.Parse(y_p.text), float.Parse(z_p.text));
    }

    public void SetFocusRotation()
    {
        STextTransform.focus.transform.localRotation
            = Quaternion.Euler(new Vector3(float.Parse(x_r.text), float.Parse(y_r.text), float.Parse(z_r.text)));
    }

    public void SetFocusScale()
    {
        STextTransform.focus.transform.localScale
            = new Vector3(float.Parse(x_s.text), float.Parse(y_s.text), float.Parse(z_s.text));
    }
}
