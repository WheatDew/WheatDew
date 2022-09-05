using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public Light sun;
    public float dayTime;
    public float timer;

    private void Update()
    {
        sun.transform.Rotate(Vector3.right, Time.deltaTime / dayTime*360f,Space.Self);
    }
}
