using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharacterCreateMode {Random }
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

    public List<GameObject> characterPrefabs=new List<GameObject>();
    public List<string> characterNames=new List<string>();
    public Dictionary<string,GameObject> characterDict=new Dictionary<string, GameObject>();
    public List<Transform> characterCreatePoints=new List<Transform>();

    private void Start()
    {
        InitCharacterPrefabs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            CreateCharacter("°×ÔªÔª",CharacterCreateMode.Random);
        }
    }

    public void BuildingTask()
    {
        
    }

    public void InitCharacterPrefabs()
    {
        for(int i = 0; i < characterPrefabs.Count; i++)
        {
            characterDict.Add(characterNames[i], characterPrefabs[i]);
        }
    }

    public void CreateCharacter(string characterName,CharacterCreateMode characterCreateMode)
    {
        switch (characterCreateMode)
        {
            case CharacterCreateMode.Random:
                GameObject obj = Instantiate(characterDict[characterName]);
                obj.transform.GetChild(0).position = characterCreatePoints[Random.Range(0, characterCreatePoints.Count - 1)].position;
                break;
        }
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
