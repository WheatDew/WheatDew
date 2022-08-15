using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Origin
{
    public class PrecipePageItem : MonoBehaviour,IPointerClickHandler
    {
        public TMP_Text title;
        public Image icon;

        [HideInInspector] public RecipePage recipePage;
        public string[] recipe;

        public void OnPointerClick(PointerEventData eventData)
        {
            string command = "ProduceItem " + recipePage.guest.key;
            for (int i = 0; i < recipe.Length; i++)
            {
                command += " " + recipe[i];
            }

            Origin.TaskSystem.s.Execute(command);
            Press();
        }

        public async void Press()
        {
            icon.color = Color.gray;
            await new WaitForSeconds(0.2f);
            icon.color = Color.white;
        }
    }
}

