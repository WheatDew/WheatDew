using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RModel : MonoBehaviour
{
    public GameObject[] models;
    public string[] names;

    private void Awake()
    {
        for(int i = 0; i < models.Length; i++)
        {
            SModel.Add(names[i], models[i]);
        }
    }
}
