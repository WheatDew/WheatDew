using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class CharacterPack : PackComponent
    {
        CCharacter character;

        public override void Init()
        {
            character = GetComponent<CCharacter>();
            PackSystem.S.cpackList.Add(character.key, this);
            print(character.key);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                PackSystem.S.SwitchPackPage(character.key);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                PackSystem.S.OpenTargetPack(character.key);
            }
        }
    }
}

