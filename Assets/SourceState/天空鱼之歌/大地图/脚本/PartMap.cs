using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMap : MonoBehaviour
{
    private MeshRenderer self;
    public Transform cameraPosition;
    [SerializeField] private MapCity statePrefab;
    [HideInInspector] public MapCity state;
    private void Start()
    {
        self = GetComponent<MeshRenderer>();
        self.material.color = new Color(Random.value, Random.value, Random.value, 1);
        state = Instantiate(statePrefab, transform);
        state.cameraPosition = cameraPosition;
    }

    private void OnMouseOver()
    {
        
    }
}
