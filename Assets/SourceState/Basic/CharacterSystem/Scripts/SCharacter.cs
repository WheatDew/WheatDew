using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Origin
{
    public class SCharacter : MonoBehaviour
    {
        public List<CCharacter> characterTestList = new List<CCharacter>();
        public static Dictionary<string, CCharacter> characterList = new Dictionary<string, CCharacter>();

        private void Start()
        {
            
        }


        public void CreateCharacter(string value)
        {
            
        }

        

        //²âÊÔ½ÇÉ«º¯Êý
        public static void CreateCharacter(string name, CCharacter character)
        {
            characterList.Add(name, character);
        }

    }
}

