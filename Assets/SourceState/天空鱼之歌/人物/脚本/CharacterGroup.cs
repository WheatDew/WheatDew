using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroup : MonoBehaviour
{
    static CharacterGroup _s;
    public static CharacterGroup s { get { return _s; } }

    private void Awake()
    {
        if (_s == null)
            _s = this;
    }


    public Dictionary<string, CCharacter> characterList = new Dictionary<string, CCharacter>();
    public List<StrollPoint> strollPoints=new List<StrollPoint>();

    public void BuildingTask()
    {
        
    }

    public Vector3 GetStrollPoint()
    {
        for(int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, strollPoints.Count);
            //Debug.Log()
            if (strollPoints[index].inside.Count == 0)
                return strollPoints[index].transform.position;
        }
        return Vector3.zero;
    }
}
