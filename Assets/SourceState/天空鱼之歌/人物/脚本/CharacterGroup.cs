using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroup : MonoBehaviour
{
    private static CharacterGroup _s;
    public static CharacterGroup s { get { return _s; } }

    private void Awake()
    {
        if (_s == null)
            _s = this;
    }

    private Dictionary<int, CCharacter> characterList = new Dictionary<int, CCharacter>();


    public void AddCharacter(int id,CCharacter character)
    {
        characterList.Add(id, character);
    }

    public void AddCharacter(CCharacter character)
    {
        characterList.Add(character.gameObject.GetInstanceID(), character);
    }

    public CCharacter GetCharacter(int id)
    {
        return characterList[id];
    }

    #region ¹¦ÄÜ

    public CCharacter GetClosestCharacter(CCharacter self)
    {
        CCharacter nearest=null;
        float currentDistance = 999;

        foreach(var item in characterList)
        {
            if(item.Value != self && item.Value.gameObject.tag == "Enemy")
            {
                var calculateDistance = Vector3.Distance(item.Value.transform.position, self.transform.position);
                if (calculateDistance < currentDistance)
                {
                    currentDistance = calculateDistance;
                    nearest = item.Value;
                }
            }

        }

        return nearest;
    }

    #endregion
}
