using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologyPage : MonoBehaviour
{
    public GridLayoutGroup gridLayoutGroup;
    public Transform content;
    [SerializeField] private TechnologyItem technologyItemPrefab;

    public TechnologyItem Create(string name)
    {
        TechnologyItem obj = Instantiate(technologyItemPrefab, content);
        obj.nameText.text = name;
        return obj;
    }
    
}
