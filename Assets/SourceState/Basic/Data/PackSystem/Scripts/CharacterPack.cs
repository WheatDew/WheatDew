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
            PackSystem.S.PackList.Add(character.key, this);
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

